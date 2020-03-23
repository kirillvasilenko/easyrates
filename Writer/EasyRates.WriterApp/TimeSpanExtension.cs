using System;

namespace EasyRates.WriterApp
{
    public static class TimeSpanExtension
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
    }
}