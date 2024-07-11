using CurrencyConverterBackend.Models;
using Newtonsoft.Json;
using RestSharp;

namespace CurrencyConverterBackend.Utilities
{
    public class ApiServiceClient
    {
        private readonly RestClient _client;

        public ApiServiceClient(string clientBaseUrl) 
        { 
            _client = new RestClient(clientBaseUrl);
        }

        public async Task<ExchangeRateResponse> GetLatestRates(string baseCurrency)
        {
            var request = new RestRequest("latest", Method.Get);
            
            request.AddQueryParameter("base", baseCurrency);

            var response = await _client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Error getting latest exchange rates: {response.ErrorMessage}");
            }

            return JsonConvert.DeserializeObject<ExchangeRateResponse>(response.Content);
        }

        public async Task<ConversionResponse> ConvertCurrency(string fromCurrency, string toCurrency, decimal amount)
        {
            var request = new RestRequest("convert", Method.Get);

            request.AddQueryParameter("from", fromCurrency);
            request.AddQueryParameter("to", toCurrency);
            request.AddQueryParameter("amount", amount.ToString());

            var response = await _client.ExecuteAsync(request);

            if(!response.IsSuccessful) 
            {
                throw new Exception($"Error in currency conversion: {response.ErrorMessage}");    
            }

            return JsonConvert.DeserializeObject<ConversionResponse>(response.Content);
        }

        public async Task<HistoricalRatesResponse> GetHistoricalExchangeRates(string startDate, string endDate, string baseCurrency, int page, int pageSize)
        {
            var request = new RestRequest($"historical/{startDate}..{endDate}", Method.Get);
            
            request.AddQueryParameter("base", baseCurrency);
            request.AddQueryParameter("page", page.ToString());
            request.AddQueryParameter("page_size", pageSize.ToString());

            var response = await _client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Error fetching historical exchange rates: {response.ErrorMessage}");
            }

            return JsonConvert.DeserializeObject<HistoricalRatesResponse>(response.Content);
        }
    }
}
