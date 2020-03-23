using System;

namespace EasyRates.WriterApp.Console.Di
{
    public class TimetableSettings
    {
        public TimeSpan AnchorTime { get; set; } = TimeSpan.FromHours(0);

        public TimeSpan UpdatePeriod { get; set; } = TimeSpan.FromHours(1);

    }
}