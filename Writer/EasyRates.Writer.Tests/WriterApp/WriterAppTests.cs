using System;
using System.Threading.Tasks;
using EasyRates.WriterApp;
using FluentAssertions;
using Microsoft.Extensions.Internal;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace EasyRates.Writer.Tests.WriterApp
{
    public class WriterAppTests
    {
        private readonly EasyRates.WriterApp.WriterApp app;

        private readonly Mock<ITimetable> timetable = new Mock<ITimetable>();

        private readonly Mock<ISystemClock> clock = new Mock<ISystemClock>();

        private RatesUpdaterMock.InvokeCase[] cases;

        private RatesUpdaterMock updater;

        private DateTime currentTime = DateTime.UtcNow;

        public WriterAppTests(ITestOutputHelper outputHelper)
        {
            var logger = outputHelper.BuildLoggerFor<EasyRates.WriterApp.WriterApp>();
            clock.Setup(x => x.UtcNow).Returns(currentTime);
            
            cases = new[]
            {
                new RatesUpdaterMock.InvokeCase(),
                new RatesUpdaterMock.InvokeCase(),
                new RatesUpdaterMock.InvokeCase()
            };
            updater = new RatesUpdaterMock(cases);
            app = new EasyRates.WriterApp.WriterApp(updater, timetable.Object, clock.Object, logger);
        }


        [Fact]
        public async Task Start_Starts_Stop_Stops()
        {
            var aLittleTime = TimeSpan.FromMilliseconds(50);
            timetable.Setup(t => t.GetNextMoment(It.IsAny<DateTime>()))
                .Returns(new Func<DateTime, DateTime>(d => d.Add(aLittleTime)));

            app.Start();

            await Task.Delay(aLittleTime);

            app.Stop();

            var invokesCount = updater.InvokesCount;
            invokesCount.Should().BeGreaterThan(0);

            await Task.Delay(aLittleTime);

            updater.InvokesCount.Should().Be(invokesCount);

            Assert.Throws<ObjectDisposedException>(() => app.Start());
        }

        [Fact]
        public async Task WriterApp_WorksOnTimer()
        {
            var oneCycleTime = TimeSpan.FromMilliseconds(100);
            timetable.Setup(t => t.GetNextMoment(It.IsAny<DateTime>()))
                .Returns(new Func<DateTime, DateTime>(d => d.Add(oneCycleTime)));

            app.Start();

            await Task.Delay(oneCycleTime * 3);

            app.Stop();

            updater.InvokesCount.Should().BeInRange(3, 4);
        }

        [Fact]
        public async Task WriterApp_KeepWorking_IfException()
        {
            var oneCycleTime = TimeSpan.FromMilliseconds(100);
            timetable.Setup(t => t.GetNextMoment(It.IsAny<DateTime>()))
                .Returns(new Func<DateTime, DateTime>(d => d.Add(oneCycleTime)));

            cases[1].ThrowException = true;

            app.Start();

            await Task.Delay(oneCycleTime * 3);

            app.Stop();

            updater.InvokesCount.Should().BeGreaterThan(2);
        }
    }
}