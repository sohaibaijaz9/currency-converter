using CurrencyConverterBackend.Commands;
using CurrencyConverterBackend.Commands.CurrencyConversion;
using CurrencyConverterBackend.Models;
using CurrencyConverterBackend.Queries;
using CurrencyConverterBackend.Queries.HistoricalRates;
using CurrencyConverterBackend.Queries.LatestExchangeRates;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverterBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly ICommandHandler<CurrencyConversionCommand, Response<ConversionResponse>> _conversionCommandHandler;
        private readonly IQueryHandler<LatestExchangeRatesQuery, Response<ExchangeRateResponse>> _exchangeRatesQueryHandler;
        private readonly IQueryHandler<HistoricalRatesQuery, Response<HistoricalRatesResponse>> _historicalRatesQueryHandler;

        public ExchangeController(ICommandHandler<CurrencyConversionCommand, Response<ConversionResponse>> conversionCommandHandler, IQueryHandler<LatestExchangeRatesQuery, Response<ExchangeRateResponse>> exchangeRatesQueryHandler, IQueryHandler<HistoricalRatesQuery, Response<HistoricalRatesResponse>> historicalRatesQueryHandler)
        {
            _conversionCommandHandler = conversionCommandHandler;
            _exchangeRatesQueryHandler = exchangeRatesQueryHandler;
            _historicalRatesQueryHandler = historicalRatesQueryHandler;
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestExchangeRates([FromQuery] LatestExchangeRatesQuery query)
        {
            try
            {
                var result = await _exchangeRatesQueryHandler.HandleAsync(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(400, new Response<ExchangeRateResponse>()
                {
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpPost("convert")]
        public async Task<IActionResult> ConvertCurrency([FromBody] CurrencyConversionCommand command)
        {
            try
            {
                var result = await _conversionCommandHandler.HandleAsync(command);
                return Ok(result);  
            }
            catch(Exception ex)
            {
                return StatusCode(400, new Response<ExchangeRateResponse>()
                {
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpGet("historical")]
        public async Task<IActionResult> GetHistoricalRates([FromQuery] HistoricalRatesQuery query)
        {
            try
            {
                var result = await _historicalRatesQueryHandler.HandleAsync(query);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(400, new Response<ExchangeRateResponse>()
                {
                    Message = ex.Message,
                    Success = false
                });
            }
        }
    }
}
