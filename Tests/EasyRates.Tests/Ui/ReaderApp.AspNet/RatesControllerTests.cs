using System;
using System.Threading.Tasks;
using AutoFixture;
using EasyRates.ErrorMessages.InMemory;
using EasyRates.Model;
using EasyRates.Reader.Model;
using EasyRates.ReaderApp;
using EasyRates.ReaderApp.AspNet.Controllers;
using EasyRates.ReaderApp.Dto;
using ErrorMessages;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EasyRates.Tests.Ui.ReaderApp.AspNet
{
    public class RatesControllerTests
    {
        private Fixture fixture = new Fixture();

        private RatesController controller;
        
        private Mock<IReaderRateService> svc = new Mock<IReaderRateService>(MockBehavior.Strict);
        
        private Mock<IErrorMessages> errors = new Mock<IErrorMessages>(MockBehavior.Strict);
        
        private Mock<ILogger<RatesController>> logger = new Mock<ILogger<RatesController>>(MockBehavior.Loose);
        
        public RatesControllerTests()
        {
            controller = new RatesController(svc.Object, errors.Object, logger.Object);
        }

        [Fact]
        public async Task GetRate_Ok()
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
        public async Task GetRate_InvalidCurrencyName()
        {
            var from = fixture.Create<string>();
            var to = fixture.Create<string>();
            var errorMessage = fixture.Create<string>();

            svc.Setup(s => s.GetRate(from, to)).Throws(new InvalidCurrencyNameException(from));
            errors.Setup(e => e.GetMessage(ErrorCode.InvalidCurrencyName, It.IsAny<string>(), It.IsAny<object[]>())).ReturnsAsync(errorMessage);
            
            var result = await controller.GetRate(from, to);

            var badResult = (BadRequestObjectResult) result;
            
            badResult.CheckErrorDetails(ErrorCode.InvalidCurrencyName, errorMessage);
        }
        
        [Fact]
        public async Task GetRate_RateNotFound()
        {
            var from = fixture.Create<string>();
            var to = fixture.Create<string>();
            var errorMessage = fixture.Create<string>();
            
            svc.Setup(s => s.GetRate(from, to)).Throws(new RateNotFoundException(from, to));
            errors.Setup(e => e.GetMessage(ErrorCode.RateNotFound, It.IsAny<string>(), It.IsAny<object[]>())).ReturnsAsync(errorMessage);
            
            var result = await controller.GetRate(from, to);

            var badResult = (BadRequestObjectResult) result;
            
            badResult.CheckErrorDetails(ErrorCode.RateNotFound, errorMessage);
        }
        
        [Fact]
        public async Task GetRate_InternalError()
        {
            var from = fixture.Create<string>();
            var to = fixture.Create<string>();
            var errorMessage = fixture.Create<string>();
            
            svc.Setup(s => s.GetRate(from, to)).Throws(new Exception());
            errors.Setup(e => e.GetMessage(ErrorCode.InternalServerError, It.IsAny<string>(), It.IsAny<object[]>())).ReturnsAsync(errorMessage);
            
            var result = await controller.GetRate(from, to);

            var badResult = (ObjectResult) result;
            
            badResult.CheckInternalServerError(errorMessage);
        }

        [Fact]
        public async Task GetRates_Ok()
        {
            var from = fixture.Create<string>();
            var response = fixture.Create<RatesResponse>();

            svc.Setup(s => s.GetRates(from)).ReturnsAsync(response);

            var result = await controller.GetRates(from);

            var okResult = (OkObjectResult) result;
            okResult.Value.Should().Be(response);
        }
        
        [Fact]
        public async Task GetRates_InvalidCurrencyName()
        {
            var from = fixture.Create<string>();
            var errorMessage = fixture.Create<string>();

            svc.Setup(s => s.GetRates(from)).Throws(new InvalidCurrencyNameException(from));
            errors.Setup(e => e.GetMessage(ErrorCode.InvalidCurrencyName, It.IsAny<string>(), It.IsAny<object[]>())).ReturnsAsync(errorMessage);
            
            var result = await controller.GetRates(from);

            var badResult = (BadRequestObjectResult) result;
            
            badResult.CheckErrorDetails(ErrorCode.InvalidCurrencyName, errorMessage);
        }
        
        [Fact]
        public async Task GetRates_RateNotFound()
        {
            var from = fixture.Create<string>();
            var errorMessage = fixture.Create<string>();
            
            svc.Setup(s => s.GetRates(from)).Throws(new NoOneRateFoundException(from));
            errors.Setup(e => e.GetMessage(ErrorCode.NoOneRateFound, It.IsAny<string>(), It.IsAny<object[]>())).ReturnsAsync(errorMessage);
            
            var result = await controller.GetRates(from);

            var badResult = (BadRequestObjectResult) result;
            
            badResult.CheckErrorDetails(ErrorCode.NoOneRateFound, errorMessage);
        }
        
        [Fact]
        public async Task GetRates_InternalError()
        {
            var from = fixture.Create<string>();
            var errorMessage = fixture.Create<string>();
            
            svc.Setup(s => s.GetRates(from)).Throws(new Exception());
            errors.Setup(e => e.GetMessage(ErrorCode.InternalServerError, It.IsAny<string>(), It.IsAny<object[]>())).ReturnsAsync(errorMessage);
            
            var result = await controller.GetRates(from);

            var badResult = (ObjectResult) result;
            
            badResult.CheckInternalServerError(errorMessage);
        }
    }
}