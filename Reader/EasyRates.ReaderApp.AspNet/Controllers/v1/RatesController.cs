using System.Threading.Tasks;
using EasyRates.ReaderApp.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyRates.ReaderApp.AspNet.Controllers.v1
{
    /// <summary>
    /// Rates service.
    /// </summary>
    [Route("api/v{version:apiVersion}/rates")]
    [ApiController]
    public class RatesController : ControllerBase
    {

        private readonly IReaderRateService rateService;

        /// <summary>
        /// Constructs <see cref="RatesController"/>
        /// </summary>
        /// <param name="rateService"></param>
        public RatesController(
            IReaderRateService rateService) 
        {
            this.rateService = rateService;
        }
        
        /// <summary>
        /// Get current currency rate.
        /// </summary>
        /// <param name="from">Currency from</param>
        /// <param name="to">Currency to</param>
        /// <response code="200">Current currency rate.</response>
        /// <response code="400">
        /// 1. Rate not found.
        /// 2. Invalid currency name.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("{from}/{to}")]
        [ProducesResponseType(typeof(RatesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetRate(
            string from, 
            string to)
        {
            var result = await rateService.GetRate(from, to); 
            return Ok(result);
        }
        
        /// <summary>
        /// Get all current rates for currency.
        /// </summary>
        /// <param name="from">Currency from</param>
        /// <response code="200">All current currency rates.</response>
        /// <response code="400">
        /// 1. No one rate found.
        /// 2. Invalid currency name.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("{from}")]
        [ProducesResponseType(typeof(RatesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetRates(
            string from)
        {
            var result = await rateService.GetRates(from);
            return Ok(result);
        }
    }
}