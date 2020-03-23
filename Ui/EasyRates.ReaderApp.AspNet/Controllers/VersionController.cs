using System;
using System.Threading.Tasks;
using AppVersion;
using AspNetCore.Common;
using ErrorMessages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EasyRates.ReaderApp.AspNet.Controllers
{
    /// <summary>
    /// All about version of application. Available to anyone.
    /// </summary>
    [Route("api/version")]
    public class VersionController : BaseController
    {
        private readonly IAppVersion version;
        private readonly ILogger<VersionController> logger;

        /// <summary>
        /// Constructs <see cref="VersionController"/>
        /// </summary>
        /// <param name="version"></param>
        /// <param name="errorMessages"></param>
        /// <param name="logger"></param>
        public VersionController(
            IAppVersion version,
            IErrorMessages errorMessages,
            ILogger<VersionController> logger):base(errorMessages)
        {
            this.version = version;
            this.logger = logger;
        }
        
        /// <summary>
        /// Gets api version.
        /// </summary>
        /// <response code="200">Version of API.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("api")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetApiVersion()
        {
            try
            {
                return Ok(version.OnlyMajorMinor);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return await InternalServerError();       
            }
        }
        
        /// <summary>
        /// Gets application version.
        /// </summary>
        /// <response code="200">Version of application.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("app")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAppVersion()
        {
            try
            {
                return Ok(version.Version);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return await InternalServerError();       
            }
        }
        
        /// <summary>
        /// Gets informational version of application.
        /// </summary>
        /// <response code="200">Informational version of application.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("informational")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetInformationalVersion()
        {
            try
            {
                return Ok(version.InformationalVersion);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return await InternalServerError();       
            }
        }
    }
    
}