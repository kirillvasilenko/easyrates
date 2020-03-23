using System;
using System.Threading.Tasks;
using EasyRates.WriterApp;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Time;
using Xunit;
using Xunit.Abstractions;

namespace EasyRates.Tests.App.WriterApp
{
    public class WriterAppTests
    {
        private EasyRates.WriterApp.WriterApp app;
        
        private Mock<ITimetable> timetable = new Mock<ITimetable>();
        
        private ILogger<EasyRates.WriterApp.WriterApp> logger;

        private RatesUpdaterTest.InvokeCase[] cases;

        private RatesUpdaterTest updater;
        
        public WriterAppTests(ITestOutputHelper output)
        {
            logger = output.BuildLoggerFor<EasyRates.WriterApp.WriterApp>();
            
            TimeProviderTest.SetAndGetTimeProviderTestInstanceIfNeed();

            cases = new[]
            {
                new RatesUpdaterTest.InvokeCase(),
                new RatesUpdaterTest.InvokeCase(),
                new RatesUpdaterTest.InvokeCase()
            };
            updater = new RatesUpdaterTest(cases);
            app = new EasyRates.WriterApp.WriterApp(updater, timetable.Object, logger);
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