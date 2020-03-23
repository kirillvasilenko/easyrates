using System;

namespace Time
{
    public static class DateTimeExtension
    {
        public static TimeSpan Normalize(this TimeSpan time, TimeSpan basis)
        {
            if (time > basis)
            {
                var resultTicks = time.Ticks % basis.Ticks;
                return TimeSpan.FromTicks(resultTicks);
            }

            return time;
        }
        
        public static TimeSpan ToAbsoluteExpirationRelativeNow(this DateTime expirationTime, TimeSpan defaultIfAlreadyExpired)
        {
            var now = TimeProvider.UtcNow;
            if (expirationTime > now)
            {
                return expirationTime - now;
            }

            return defaultIfAlreadyExpired;
        }
        
        public static DateTime ToDateTimeUtc(this long timestampUnix)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestampUnix);
        } 
        
        public static DateTime TruncateMilliseconds(this DateTime dateTime)
        {
            return new DateTime(
                dateTime.Ticks - (dateTime.Ticks % TimeSpan.TicksPerSecond), 
                dateTime.Kind
            );
        }
        
        public static DateTimeOffset TruncateMilliseconds(this DateTimeOffset dateTime)
        {
            return new DateTimeOffset(
                dateTime.Ticks - (dateTime.Ticks % TimeSpan.TicksPerSecond),
                dateTime.Offset
            );
        }
    }
}