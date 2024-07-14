using CurrencyConverterBackend.Commands.CurrencyConversion;
using CurrencyConverterBackend.Models;
using CurrencyConverterBackend.Tests.Utilities;
using CurrencyConverterBackend.Utilities;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace CurrencyConverterBackend.Tests
{
    public class CurrencyConverisonCommandHandlerTests
    {
        private readonly ApiServiceClient _apiClient;
        private readonly CurrencyConversionCommandValidator _validator;

        public CurrencyConverisonCommandHandlerTests()
        {
            var configuration = BaseConfig.LoadConfiguration();

            _apiClient = new ApiServiceClient(configuration.GetSection("ExternalApi").GetValue<string>("BaseUrl"));
            _validator = new CurrencyConversionCommandValidator(configuration);
        }


        [Fact]
        public async Task HandleAsync_ValidCommand_ReturnsConversionResponse()
        {
            var handler = new CurrencyConversionCommandHandler(_apiClient, _validator);
            var command = new CurrencyConversionCommand
            {
                FromCurrency = "USD",
                ToCurrency = "EUR",
                Amount = 100
            };

            //Act
            var result = await handler.HandleAsync(command);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Currency Conversion Successful!");
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task HandleAsync_InvalidCommand_ThrowsValidationException()
        {
            var handler = new CurrencyConversionCommandHandler(_apiClient, _validator);
            var command = new CurrencyConversionCommand();
            var validationFailures = await _validator.ValidateAsync(command);

            //Act
            Func<Task> act = async () => await handler.HandleAsync(command);

            //Asset
            await act.Should().ThrowAsync<ValidationException>()
           .Where(ex => ex.Errors.Count() == validationFailures.Errors.Count);
        }
    }
}
