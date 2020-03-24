using System.Threading.Tasks;
using AutoFixture;
using EasyRates.ReaderApp;
using EasyRates.ReaderApp.AspNet.Controllers;
using EasyRates.ReaderApp.Dto;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EasyRates.Reader.Tests.ReaderAppAspNet
{
    public class RatesControllerTests
    {
        private readonly Fixture fixture = new Fixture();

        private readonly RatesController controller;
        
        private readonly Mock<IReaderRateService> svc = new Mock<IReaderRateService>(MockBehavior.Strict);
        
        public RatesControllerTests()
        {
            controller = new RatesController(svc.Object);
        }

        [Fact]
        public async Task GetRate()
        {
            var from = fixture.Create<string>();
            var to = fixture.Create<string>();
            var response = fixture.Create<RatesResponse>();

            svc.Setup(s => s.GetRate(from, to)).ReturnsAsync(response);

            var result = await controller.GetRate(from, to);

            var okResult = (OkObjectResult) result;
            okResult.Value.Should().Be(response);
        }
        
        [Fact]
        public async Task GetRates()
        {
            var from = fixture.Create<string>();
            var response = fixture.Create<RatesResponse>();

            svc.Setup(s => s.GetRates(from)).ReturnsAsync(response);

            var result = await controller.GetRates(from);

            var okResult = (OkObjectResult) result;
            okResult.Value.Should().Be(response);
        }
    }
}