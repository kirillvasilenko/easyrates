using AspNetCore.Common;
using EasyRates.ErrorMessages.InMemory;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyRates.Tests.Ui.ReaderApp.AspNet
{
    public static class ControllerResultsTestsExtension
    {
        public static void CheckInternalServerError(
            this ObjectResult result,
            string message)
        {
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            var error = (ErrorDetails)result.Value;
            error.Status.Should().Be(StatusCodes.Status500InternalServerError);
            error.Code.Should().Be(ErrorCode.InternalServerError);
            error.Message.Should().Be(message);
        }
        
        public static void CheckErrorDetails(
            this BadRequestObjectResult result,
            string errorCode,
            string message)
        {
            var error = (ErrorDetails)result.Value;
            error.Status.Should().Be(StatusCodes.Status400BadRequest);
            error.Code.Should().Be(errorCode);
            error.Message.Should().Be(message);
        }
    }
}