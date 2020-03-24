using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;

namespace EasyRates.WriterApp
{
    public class WriterApp:IWriterApp
    {
        private readonly IRatesUpdater ratesUpdater;
        private readonly ITimetable timetable;
        private readonly ISystemClock clock;
        private readonly ILogger<WriterApp> logger;

        
        // Control cycle
        

        private CancellationTokenSource cts = new CancellationTokenSource();
        
        private readonly ManualResetEvent stopped = new ManualResetEvent(true);

        private bool disposed;
        
        public WriterApp(
            IRatesUpdater ratesUpdater,
            ITimetable timetable,
            ISystemClock clock,
            ILogger<WriterApp> logger)
        {
            this.ratesUpdater = ratesUpdater;
            this.timetable = timetable;
            this.clock = clock;
            this.logger = logger;
        }
        
        #region Public
        
        public void Start()
        {
            CheckIsntDisposed();
            stopped.Reset();
            cts = new CancellationTokenSource();
            Task.Factory.StartNew(WorkCycle);
        }

        public void Stop()
        {
            cts?.Cancel();
            stopped.WaitOne();
            cts?.Dispose();
            cts = null;
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }
            Stop();
            disposed = true;
        }

        #endregion
        
        #region Private
        
        private async Task WorkCycle()
        {
            var watch = new Stopwatch();
            var currentMoment = UtcNow;
            while (true)
            {
                watch.Restart();
                try
                {
                    cts.Token.ThrowIfCancellationRequested();
                    
                    logger.LogDebug("Updating rates...");
                    await ratesUpdater.UpdateRates(cts.Token);
                    
                    watch.Stop();
                    logger.LogDebug($"Updating takes {watch.ElapsedMilliseconds} ms.");

                    var nextMoment = timetable.GetNextMoment(currentMoment);
                    var now = UtcNow;
                    
                    // should already be started
                    if (nextMoment <= now)
                    {
                        currentMoment = now;
                        continue;
                    }
                    // need to wait next moment
                    logger.LogDebug($"Lets sleep {nextMoment - now}");
                    await Task.Delay(nextMoment - now, cts.Token);
                    currentMoment = nextMoment;
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception e)
                {
                    logger.LogError(e, e.Message);
                }
            }

            stopped.Set();
        }

        private DateTime UtcNow => clock.UtcNow.UtcDateTime;

        private void CheckIsntDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(WriterApp));
            }
        }
        
        #endregion

        
    }
}