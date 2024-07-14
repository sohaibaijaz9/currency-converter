using CurrencyConverterBackend.Models;
using CurrencyConverterBackend.Queries.LatestExchangeRates;
using CurrencyConverterBackend.Utilities;
using FluentValidation;

namespace CurrencyConverterBackend.Queries.HistoricalRates
{
    public class HistoricalRatesQueryHandler : IQueryHandler<HistoricalRatesQuery, Response<HistoricalRatesResponse>>
    {
        private readonly ApiServiceClient _client;
        private readonly HistoricalRatesQueryValidator _validator;
        public HistoricalRatesQueryHandler(ApiServiceClient client, HistoricalRatesQueryValidator validator)
        {
            _client = client;   
            _validator = validator;
        }

        public async Task<Response<HistoricalRatesResponse>> HandleAsync(HistoricalRatesQuery query)
        {
            var validationResult = await _validator.ValidateAsync(query);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return new Response<HistoricalRatesResponse>()
            {
                Data = await _client.GetHistoricalExchangeRates(query.StartDate, query.EndDate, query.BaseCurrency, query.Page, query.PageSize),
                Message = "Historical Exchange Rates found successfully!",
                Success = true
            };
        }
    }
}
