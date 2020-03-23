using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Time;

namespace EasyRates.WriterApp
{
    public class WriterApp:IWriterApp
    {
        private readonly IRatesUpdater ratesUpdater;
        private readonly ITimetable timetable;
        private readonly ILogger<WriterApp> logger;

        
        // Control cycle
        

        private CancellationTokenSource cts = new CancellationTokenSource();
        
        private ManualResetEvent writerStopped = new ManualResetEvent(false);

        private bool disposed;
        
        public WriterApp(
            IRatesUpdater ratesUpdater,
            ITimetable timetable,
            ILogger<WriterApp> logger)
        {
            this.ratesUpdater = ratesUpdater;
            this.timetable = timetable;
            this.logger = logger;
        }
        
        #region Public
        
        public void Start()
        {
            CheckIsntDisposed();
            Task.Factory.StartNew(WorkCycle);
        }

        public void Stop()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }
            cts.Cancel();
            writerStopped.WaitOne();
            disposed = true;
        }

        #endregion
        
        #region Private
        
        private async Task WorkCycle()
        {
            var watch = new Stopwatch();
            var currentMoment = TimeProvider.UtcNow;
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
                    var now = TimeProvider.UtcNow;
                    
                    // should already be started
                    if (nextMoment < now)
                    {
                        currentMoment = now;
                        continue;
                    }
                    
                    logger.LogDebug($"Lets sleep {nextMoment - now}");
                    await Task.Delay(nextMoment - now, cts.Token);
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

            writerStopped.Set();
        }
        
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