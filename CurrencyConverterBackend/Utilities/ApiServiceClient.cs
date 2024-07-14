using CurrencyConverterBackend.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Polly;
using RestSharp;

namespace CurrencyConverterBackend.Utilities
{
    public class ApiServiceClient
    {
        private static TimeSpan _pauseBetweenFailures = TimeSpan.FromSeconds(2);
        private readonly SemaphoreSlim _rateLimitSemaphore;

        private readonly RestClient _client;
        private readonly string _clientBaseUrl;
        private readonly IConfiguration _configuration;

        public ApiServiceClient(string clientBaseUrl) 
        { 
            _configuration = LoadConfiguration(); 
            _clientBaseUrl = clientBaseUrl;
            _client = new RestClient(clientBaseUrl);
            _rateLimitSemaphore = new SemaphoreSlim(1, _configuration.GetSection("ExternalApi").GetValue<int>("ConcurrentRequests"));
        }

        public async Task<ExchangeRateResponse> GetLatestRates(string baseCurrency)
        {
            await _rateLimitSemaphore.WaitAsync();

            try
            {
                var request = new RestRequest("latest", Method.Get);
                request.AddQueryParameter("base", baseCurrency.ToUpper());


                //Retry policy for retrying api calls on failure
                var retryPolicy = Policy
                   .HandleResult<RestResponse<ExchangeRateResponse>>(x => !x.IsSuccessful)
                   .WaitAndRetryAsync(_configuration.GetSection("ExternalApi").GetValue<int>("MaxRetryAttempts"), x => _pauseBetweenFailures);


                var response = await retryPolicy.ExecuteAsync(() => _client.ExecuteAsync<ExchangeRateResponse>(request));

                if (!response.IsSuccessful)
                {
                    throw new Exception($"Error getting latest exchange rates: {response.ErrorException?.Message}");
                }

                return JsonConvert.DeserializeObject<ExchangeRateResponse>(response.Content);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _rateLimitSemaphore.Release();  
            }
            
        }

        public async Task<ConversionResponse> ConvertCurrency(string fromCurrency, string toCurrency, decimal amount)
        {
            await _rateLimitSemaphore.WaitAsync();

            try
            {
                var request = new RestRequest("latest", Method.Get);

                request.AddQueryParameter("from", fromCurrency.ToUpper());
                request.AddQueryParameter("to", toCurrency.ToUpper());
                request.AddQueryParameter("amount", amount.ToString());

                var retryPolicy = Policy
                  .HandleResult<RestResponse<ConversionResponse>>(x => !x.IsSuccessful)
                  .WaitAndRetryAsync(_configuration.GetSection("ExternalApi").GetValue<int>("MaxRetryAttempts"), x => _pauseBetweenFailures);


                var response = await retryPolicy.ExecuteAsync(() => _client.ExecuteAsync<ConversionResponse>(request));

                if (!response.IsSuccessful)
                {
                    throw new Exception($"Error in currency conversion: {response.ErrorException?.Message}");
                }

                return JsonConvert.DeserializeObject<ConversionResponse>(response.Content);
            }
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
                _rateLimitSemaphore.Release(); 
            }
           
        }

        public async Task<HistoricalRatesResponse> GetHistoricalExchangeRates(string startDate, string endDate, string baseCurrency, int page, int pageSize)
        {
            await _rateLimitSemaphore.WaitAsync();

            try
            {
                var request = new RestRequest($"{startDate}..{endDate}", Method.Get);

                request.AddQueryParameter("base", baseCurrency.ToUpper());
                request.AddQueryParameter("page", page.ToString());
                request.AddQueryParameter("page_size", pageSize.ToString());

                var retryPolicy = Policy
                   .HandleResult<RestResponse<HistoricalRatesResponse>>(x => !x.IsSuccessful)
                   .WaitAndRetryAsync(_configuration.GetSection("ExternalApi").GetValue<int>("MaxRetryAttempts"), x => _pauseBetweenFailures);


                var response = await retryPolicy.ExecuteAsync(() => _client.ExecuteAsync<HistoricalRatesResponse>(request));

                if (!response.IsSuccessful)
                {
                    throw new Exception($"Error fetching historical exchange rates: {response.ErrorException?.Message}");
                }

                return JsonConvert.DeserializeObject<HistoricalRatesResponse>(response.Content);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _rateLimitSemaphore.Release();  
            }
            
        }

        public IConfiguration LoadConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../../../CurrencyConverterBackend"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return configurationBuilder.Build();
        }
    }
}
