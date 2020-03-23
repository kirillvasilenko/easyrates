using System;

namespace Time
{
    public class TimeProviderSystem:TimeProvider
    {
        public override DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }

        public override DateTimeOffset GetUtcNowOffset()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}