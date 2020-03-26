using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace EasyRates.ReaderApp.AspNet
{
    public class ProblemDetailsErrorResponseProvider:IErrorResponseProvider
    {
        public IActionResult CreateResponse(ErrorResponseContext context)
        {
            var details = new ProblemDetails
            {
                Type = $"https://httpstatuses.com/{context.StatusCode}",
                Title = context.ErrorCode,
                Detail = context.Message,
                Status = context.StatusCode,
            };
            
            return new ObjectResult(details)
            {
                StatusCode = context.StatusCode
            };
        }
    }
}