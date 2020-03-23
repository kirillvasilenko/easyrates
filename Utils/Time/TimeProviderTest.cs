using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Time
{
    /// <summary>
    /// Each test may needs reference to TimeProviderTest instance.
    /// Tests can run parallel.
    /// The second test must not reset global TimeProvider.Instance,
    /// otherwise the first test may pass wrong, because its logic will
    /// work with one TimeProvider.Instance during half time, the other half time with another one.
    /// So we need the same instance of TimeProviderTest for each test, that keeps
    /// time for each thread independently.
    /// </summary>
    public class TimeProviderTest : TimeProvider
    {
        #region Static
        
        private static readonly object SyncObj = new object();
        
        public static TimeProviderTest SetAndGetTimeProviderTestInstanceIfNeed()
        {
            if (instance is TimeProviderTest timeProviderTest)
            {
                return timeProviderTest;
            }

            lock (SyncObj)
            {
                if (instance is TimeProviderTest timeProviderTest1)
                {
                    return timeProviderTest1;
                }
                instance = new TimeProviderTest();
                return (TimeProviderTest) instance;
            }
        }
        
        #endregion

        private TimeProviderTest()
        {
        }

        private readonly DateTimeOffset defaultTime = DateTimeOffset.UtcNow;
        
        private readonly ConcurrentDictionary<int, DateTimeOffset> timeByThread = new ConcurrentDictionary<int, DateTimeOffset>();
        
        public void SetUtcNow(DateTimeOffset time)
        {
            timeByThread[Thread.CurrentThread.ManagedThreadId] = time;
        }

        public void Add(TimeSpan span)
        {
            SetDefaultTimeIfNeed();
            timeByThread[Thread.CurrentThread.ManagedThreadId] = timeByThread[Thread.CurrentThread.ManagedThreadId].Add(span);
        }
        
        public void WaitOneMoreSecond(TimeSpan span)
        {
            Add(span + TimeSpan.FromSeconds(1));
        }

        public override DateTime GetUtcNow()
        {
            return GetUtcNowOffset().UtcDateTime;
        }

        public override DateTimeOffset GetUtcNowOffset()
        {
            SetDefaultTimeIfNeed();
            return timeByThread[Thread.CurrentThread.ManagedThreadId];
        }
        
        private void SetDefaultTimeIfNeed()
        {
            if (!timeByThread.ContainsKey(Thread.CurrentThread.ManagedThreadId))
            {
               timeByThread[Thread.CurrentThread.ManagedThreadId] = defaultTime;    
            }
        }
    }
}