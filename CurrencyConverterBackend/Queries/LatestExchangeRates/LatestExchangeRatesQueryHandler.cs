using CurrencyConverterBackend.Models;
using CurrencyConverterBackend.Utilities;
using FluentValidation;

namespace CurrencyConverterBackend.Queries.LatestExchangeRates
{
    public class LatestExchangeRatesQueryHandler : IQueryHandler<LatestExchangeRatesQuery, Response<ExchangeRateResponse>>
    {
        private readonly ApiServiceClient _client;
        private readonly LatestExchangeRatesQueryValidator _validator;
        public LatestExchangeRatesQueryHandler(ApiServiceClient client, LatestExchangeRatesQueryValidator validator)
        {
            _client = client;
            _validator = validator;
        }

        public async Task<Response<ExchangeRateResponse>> HandleAsync(LatestExchangeRatesQuery query)
        {

            var validationResult = await _validator.ValidateAsync(query);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return new Response<ExchangeRateResponse>()
            {
                Data = await _client.GetLatestRates(query.BaseCurrency),
                Message = "Latest Exchange Rates found successfully!",
                Success = true
            };
        }
    }
}
