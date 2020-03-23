using System.Threading.Tasks;
using ErrorMessages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Common
{
    public abstract class BaseController: ControllerBase
    {
        private readonly IErrorMessages errorMessages;

        protected BaseController(IErrorMessages errorMessages)
        {
            this.errorMessages = errorMessages;
        }
        
        /// <summary>
        /// Makes <see cref="BadRequestObjectResult"/> with inner <see cref="ErrorDetails"/> object.
        /// </summary>
        /// <param name="errorCode">Error code</param>
        /// <param name="defaultMessage">Message</param>
        /// <param name="messageArgs"></param>
        /// <returns>Instance of <see cref="BadRequestObjectResult"/></returns>
        protected async Task<ObjectResult> Forbidden(
            string errorCode,
            string defaultMessage,
            params object[] messageArgs)
        {
            var message = await errorMessages.GetMessage(errorCode, defaultMessage, messageArgs);
            return new ObjectResult(
                ErrorDetails.Make(StatusCodes.Status403Forbidden, errorCode, message)
            )
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }

        /// <summary>
        /// Makes <see cref="BadRequestObjectResult"/> with inner <see cref="ErrorDetails"/> object.
        /// </summary>
        /// <param name="errorCode">Error code</param>
        /// <param name="defaultMessage">Message</param>
        /// <param name="messageArgs"></param>
        /// <returns>Instance of <see cref="BadRequestObjectResult"/></returns>
        protected async Task<BadRequestObjectResult> BadRequest(
            string errorCode,
            string defaultMessage,
            params object[] messageArgs)
        {
            var message = await errorMessages.GetMessage(errorCode, defaultMessage, messageArgs);
            return new BadRequestObjectResult(
                ErrorDetails.Make(
                    StatusCodes.Status400BadRequest,
                    errorCode,
                    message));
        }

        /// <summary>
        /// Makes <see cref="ObjectResult"/> with StatusCode=500 and inner <see cref="ErrorDetails"/> object.
        /// </summary>
        /// <returns>Instance of <see cref="ObjectResult"/></returns>
        protected async Task<ObjectResult> InternalServerError(
            string errorCode,
            string defaultMessage,
            params object[] messageArgs)
        {
            var message = await errorMessages.GetMessage(errorCode, defaultMessage, messageArgs);
            return new ObjectResult(
                ErrorDetails.Make(StatusCodes.Status500InternalServerError, errorCode, message)
            )
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
        
        /// <summary>
        /// Makes <see cref="ObjectResult"/> with StatusCode=500 and inner result object.
        /// </summary>
        /// <returns>Instance of <see cref="ObjectResult"/></returns>
        protected ObjectResult InternalServerError(
            object result)
        {
            return new ObjectResult(result)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
        
        /// <summary>
        /// Makes <see cref="ObjectResult"/> with StatusCode=500 and inner <see cref="ErrorDetails"/> object.
        /// </summary>
        /// <returns>Instance of <see cref="ObjectResult"/></returns>
        protected async Task<ObjectResult> InternalServerError()
        {
            var message = await errorMessages.GetMessage(CommonErrorCode.InternalServerError, "InternalServerError");
            return new ObjectResult(
                ErrorDetails.Make(StatusCodes.Status500InternalServerError, CommonErrorCode.InternalServerError, message)
            )
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}