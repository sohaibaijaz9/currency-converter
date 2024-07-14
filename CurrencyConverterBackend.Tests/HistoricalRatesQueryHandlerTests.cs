using CurrencyConverterBackend.Queries.HistoricalRates;
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
    public class HistoricalRatesQueryHandlerTests
    {
        private readonly ApiServiceClient _apiClient;
        private readonly HistoricalRatesQueryValidator _validator;

        public HistoricalRatesQueryHandlerTests()
        {
            var configuration = BaseConfig.LoadConfiguration();

            _apiClient = new ApiServiceClient(configuration.GetSection("ExternalApi").GetValue<string>("BaseUrl"));
            _validator = new HistoricalRatesQueryValidator();
        }


        [Fact]
        public async Task HandleAsync_ValidCommand_ReturnsHistoricalRatesResponse()
        {
            var handler = new HistoricalRatesQueryHandler(_apiClient, _validator);
            var query = new HistoricalRatesQuery
            {
                StartDate = "2024-01-01",
                EndDate = "2024-02-01",
                BaseCurrency = "USD",
                Page = 1,
                PageSize = 20,

            };

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Historical Exchange Rates found successfully!");
            result.Data.Should().NotBeNull();
            result.Data.Amount.Should().BeGreaterThan(0);
            result.Data.Base.Should().Be("USD");
            result.Data.Rates.Count.Should().BeGreaterThan(0);
            result.Data.Rates.FirstOrDefault().Should().NotBeNull();
        }

        [Fact]
        public async Task HandleAsync_InvalidCommand_ThrowsValidationException()
        {
            var handler = new HistoricalRatesQueryHandler(_apiClient, _validator);
            var query = new HistoricalRatesQuery();
            var validationFailures = await _validator.ValidateAsync(query);

            //Act
            Func<Task> act = async () => await handler.HandleAsync(query);

            //Asset
            await act.Should().ThrowAsync<ValidationException>()
           .Where(ex => ex.Errors.Count() == validationFailures.Errors.Count);
        }
    }
}
