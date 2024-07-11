using CurrencyConverterBackend.Models;
using CurrencyConverterBackend.Utilities;
using FluentValidation;

namespace CurrencyConverterBackend.Commands.CurrencyConversion
{
    public class CurrencyConversionCommandHandler : ICommandHandler<CurrencyConversionCommand, Response<ConversionResponse>>
    {
        private readonly ApiServiceClient _client;
        private readonly CurrencyConversionCommandValidator _validator;

        public CurrencyConversionCommandHandler(ApiServiceClient client, CurrencyConversionCommandValidator validator)
        {
            _client = client;
            _validator = validator; 
        }

        public async Task<Response<ConversionResponse>> HandleAsync(CurrencyConversionCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if(!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return new Response<ConversionResponse>()
            {
                Data = await _client.ConvertCurrency(command.FromCurrency, command.ToCurrency, command.Amount),
                Message = "Currency Conversion Successful!",
                Success = true
            };

        }
    }
}
