using System;
using System.Collections.Generic;
using System.Linq;
using Time;

namespace EasyRates.WriterApp
{
    public class Timetable:ITimetable
    {
        private readonly TimeSpan basis;
        private readonly TimeSpan[] momentsToWork;
        
        public Timetable(TimeSpan anchorTime, TimeSpan period)
        {
            basis = TimeSpan.FromDays(1);
            anchorTime = anchorTime.Normalize(basis);
            period = period.Normalize(basis);
            
            var moments = new List<TimeSpan>();

            var current = anchorTime;
            while (current < anchorTime + basis)
            {
                moments.Add(current.Normalize(basis));
                current += period;
            }

            moments.Sort();
            
            momentsToWork = moments.ToArray();
        }

        public DateTime GetNextMoment(DateTime currentTime)
        {
            var currentMoment = TimeSpan.FromTicks(currentTime.Ticks).Normalize(basis);
            var baseTimePart = currentTime - currentMoment;
            
            if (momentsToWork.Any(m => m > currentMoment))
            {
                return baseTimePart + momentsToWork.First(m => m > currentMoment);    
            }
            
            return baseTimePart + basis + momentsToWork.First();
        }
    }
}