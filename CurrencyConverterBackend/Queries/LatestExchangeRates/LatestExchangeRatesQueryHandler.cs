using CurrencyConverterBackend.Models;
using CurrencyConverterBackend.Utilities;

namespace CurrencyConverterBackend.Queries.LatestExchangeRates
{
    public class LatestExchangeRatesQueryHandler : IQueryHandler<LatestExchangeRatesQuery, Response<ExchangeRateResponse>>
    {
        private readonly ApiServiceClient _client;
        public LatestExchangeRatesQueryHandler(ApiServiceClient client)
        {
            _client = client;
        }

        public async Task<Response<ExchangeRateResponse>> HandleAsync(LatestExchangeRatesQuery query)
        {
            return new Response<ExchangeRateResponse>()
            {
                Data = await _client.GetLatestRates(query.BaseCurrency),
                Message = "Latest Exchange Rates found successfully!",
                Success = true
            };
        }
    }
}
