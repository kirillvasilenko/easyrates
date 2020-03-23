using System;
using System.ComponentModel;
using Microsoft.Extensions.Internal;

namespace Time
{
    public abstract class TimeProvider:ISystemClock
    {
        public static DateTime UtcNow => instance.GetUtcNow();

        public static DateTimeOffset UtcNowOffset => instance.GetUtcNowOffset();

        public static DateTime Today => UtcNow.Date;

        public abstract DateTime GetUtcNow();
        
        public abstract DateTimeOffset GetUtcNowOffset();

        public static TimeProvider Instance => instance;


        protected static volatile TimeProvider instance = new TimeProviderSystem();

        DateTimeOffset ISystemClock.UtcNow => UtcNow;
    }
}