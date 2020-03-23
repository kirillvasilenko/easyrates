using System;
using System.Threading.Tasks;
using AspNetCore.Common;
using EasyRates.ErrorMessages.InMemory;
using EasyRates.Model;
using EasyRates.Reader.Model;
using EasyRates.ReaderApp.Dto;
using ErrorMessages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EasyRates.ReaderApp.AspNet.Controllers
{
    /// <summary>
    /// Rates service.
    /// </summary>
    [Route("rates")]
    [ApiController]
    public class RatesController : BaseController
    {

        private readonly IReaderRateService rateService;
        
        private readonly ILogger<RatesController> logger;

        /// <summary>
        /// Constructs <see cref="RatesController"/>
        /// </summary>
        /// <param name="rateService"></param>
        /// <param name="errorMessages"></param>
        /// <param name="logger"></param>
        public RatesController(
            IReaderRateService rateService,
            IErrorMessages errorMessages,
            ILogger<RatesController> logger) : base(errorMessages)
        {
            this.rateService = rateService;
            this.logger = logger;
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
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetRate(
            string from, 
            string to)
        {
            try
            {
                var result = await rateService.GetRate(from, to);
                return Ok(result);
            }
            catch (InvalidCurrencyNameException e)
            {
                return await BadRequest(ErrorCode.InvalidCurrencyName, e.Message, e.CurrencyName);
            }
            catch (RateNotFoundException e)
            {
                return await BadRequest(ErrorCode.RateNotFound, e.Message, e.From, e.To);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return await InternalServerError();       
            }
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
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetRates(
            string from)
        {
            try
            {
                var result = await rateService.GetRates(from);
                return Ok(result);
            }
            catch (InvalidCurrencyNameException e)
            {
                return await BadRequest(ErrorCode.InvalidCurrencyName, e.Message, e.CurrencyName);
            }
            catch (NoOneRateFoundException e)
            {
                return await BadRequest(ErrorCode.NoOneRateFound, e.Message, e.From);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return await InternalServerError();       
            }
        }
    }
}