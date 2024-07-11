using CurrencyConverterBackend.Models;
using CurrencyConverterBackend.Utilities;

namespace CurrencyConverterBackend.Queries.HistoricalRates
{
    public class HistoricalRatesQueryHandler : IQueryHandler<HistoricalRatesQuery, Response<HistoricalRatesResponse>>
    {
        private readonly ApiServiceClient _client;
        public HistoricalRatesQueryHandler(ApiServiceClient client)
        {
            _client = client;   
        }

        public async Task<Response<HistoricalRatesResponse>> HandleAsync(HistoricalRatesQuery query)
        {
            return new Response<HistoricalRatesResponse>()
            {
                Data = await _client.GetHistoricalExchangeRates(query.StartDate, query.EndDate, query.BaseCurrency, query.Page, query.PageSize),
                Message = "Historical Exchange Rates found successfully!",
                Success = true
            };
        }
    }
}
