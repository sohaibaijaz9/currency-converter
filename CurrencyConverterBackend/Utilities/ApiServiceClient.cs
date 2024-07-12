using CurrencyConverterBackend.Models;
using Newtonsoft.Json;
using Polly;
using RestSharp;

namespace CurrencyConverterBackend.Utilities
{
    public class ApiServiceClient
    {
        private static int _maxRetryAttempts = 2;
        private static TimeSpan _pauseBetweenFailures = TimeSpan.FromSeconds(2);

        private readonly RestClient _client;

        public ApiServiceClient(string clientBaseUrl) 
        { 
            _client = new RestClient(clientBaseUrl);
           
        }

        public async Task<ExchangeRateResponse> GetLatestRates(string baseCurrency)
        {
            var request = new RestRequest("latest", Method.Get);
            request.AddQueryParameter("base", baseCurrency.ToUpper());
           
            //Retry policy for retrying api calls on failure
            var retryPolicy = Policy
               .HandleResult<RestResponse<ExchangeRateResponse>>(x => !x.IsSuccessful)
               .WaitAndRetryAsync(_maxRetryAttempts, x => _pauseBetweenFailures);


            var response = await retryPolicy.ExecuteAsync(() =>_client.ExecuteAsync<ExchangeRateResponse>(request));

            if (!response.IsSuccessful)
            {
                throw new Exception($"Error getting latest exchange rates: {response.ErrorException?.Message}");
            }

            return JsonConvert.DeserializeObject<ExchangeRateResponse>(response.Content);
        }

        public async Task<ConversionResponse> ConvertCurrency(string fromCurrency, string toCurrency, decimal amount)
        {
            var request = new RestRequest("latest", Method.Get);

            request.AddQueryParameter("from", fromCurrency.ToUpper());
            request.AddQueryParameter("to", toCurrency.ToUpper());
            request.AddQueryParameter("amount", amount.ToString());

            var retryPolicy = Policy
              .HandleResult<RestResponse<ConversionResponse>>(x => !x.IsSuccessful)
              .WaitAndRetryAsync(_maxRetryAttempts, x => _pauseBetweenFailures);


            var response = await retryPolicy.ExecuteAsync(() => _client.ExecuteAsync<ConversionResponse>(request));

            if (!response.IsSuccessful) 
            {
                throw new Exception($"Error in currency conversion: {response.ErrorException?.Message}");    
            }

            return JsonConvert.DeserializeObject<ConversionResponse>(response.Content);
        }

        public async Task<HistoricalRatesResponse> GetHistoricalExchangeRates(string startDate, string endDate, string baseCurrency, int page, int pageSize)
        {
            var request = new RestRequest($"{startDate}..{endDate}", Method.Get);
            
            request.AddQueryParameter("base", baseCurrency.ToUpper());
            request.AddQueryParameter("page", page.ToString());
            request.AddQueryParameter("page_size", pageSize.ToString());

            var retryPolicy = Policy
               .HandleResult<RestResponse<HistoricalRatesResponse>>(x => !x.IsSuccessful)
               .WaitAndRetryAsync(_maxRetryAttempts, x => _pauseBetweenFailures);


            var response = await retryPolicy.ExecuteAsync(() => _client.ExecuteAsync<HistoricalRatesResponse>(request));

            if (!response.IsSuccessful)
            {
                throw new Exception($"Error fetching historical exchange rates: {response.ErrorException?.Message}");
            }

            return JsonConvert.DeserializeObject<HistoricalRatesResponse>(response.Content);
        }
    }
}
