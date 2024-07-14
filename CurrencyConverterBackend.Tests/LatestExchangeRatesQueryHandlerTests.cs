using CurrencyConverterBackend.Commands.CurrencyConversion;
using CurrencyConverterBackend.Queries.LatestExchangeRates;
using CurrencyConverterBackend.Tests.Utilities;
using CurrencyConverterBackend.Utilities;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverterBackend.Tests
{
    public class LatestExchangeRatesQueryHandlerTests
    {

        private readonly ApiServiceClient _apiClient;
        private readonly LatestExchangeRatesQueryValidator _validator;

        public LatestExchangeRatesQueryHandlerTests()
        {
            var configuration =  BaseConfig.LoadConfiguration();

            _apiClient = new ApiServiceClient(configuration.GetSection("ExternalApi").GetValue<string>("BaseUrl"));
            _validator = new LatestExchangeRatesQueryValidator();
        }


        [Fact]
        public async Task HandleAsync_ValidCommand_ReturnsLatestExchangeRatesResponse()
        {
            var handler = new LatestExchangeRatesQueryHandler(_apiClient, _validator);
            var query = new LatestExchangeRatesQuery
            {
                BaseCurrency = "USD"
            };

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Latest Exchange Rates found successfully!");
            result.Data.Should().NotBeNull();
            result.Data.Amount.Should().BeGreaterThan(0);
            result.Data.Base.Should().Be("USD");
            result.Data.Rates.ToList().Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task HandleAsync_InvalidCommand_ThrowsValidationException()
        {
            var handler = new LatestExchangeRatesQueryHandler(_apiClient, _validator);
            var query = new LatestExchangeRatesQuery();
            var validationFailures = await _validator.ValidateAsync(query);

            //Act
            Func<Task> act = async () => await handler.HandleAsync(query);

            //Asset
            await act.Should().ThrowAsync<ValidationException>()
           .Where(ex => ex.Errors.Count() == validationFailures.Errors.Count);
        }
    }
}
