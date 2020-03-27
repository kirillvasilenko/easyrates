using System;

namespace EasyRates.WriterApp
{
    public interface ITimetable
    {
        DateTime GetNextMoment(DateTime currentTime);
    }
}