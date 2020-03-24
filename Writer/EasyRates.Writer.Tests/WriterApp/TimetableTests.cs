using System;
using EasyRates.WriterApp;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace EasyRates.Writer.Tests.WriterApp
{
    public class TimetableTests
    {
        [Fact]
        public void Timetable_Simple()
        {
            var period = TimeSpan.FromHours(1);
            var anchor = TimeSpan.FromMinutes(1);
            
            var timetable = new Timetable(anchor, period);
            
            var current = new DateTime(1,1,3,0,1,0);

            for (int i = 0; i < 36; i++)
            {
                var next = timetable.GetNextMoment(current);
                var expectedNext = current + period;

                next.Should().Be(expectedNext);

                current += period;
            }
        }
    }
}