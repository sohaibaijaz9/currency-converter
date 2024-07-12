using FluentValidation;

namespace CurrencyConverterBackend.Commands.CurrencyConversion
{
    public class CurrencyConversionCommandValidator : AbstractValidator<CurrencyConversionCommand>, IValidator
    {
        private readonly List<string> _restrictedCurrencies;

        public CurrencyConversionCommandValidator(IConfiguration configuration)
        {
            _restrictedCurrencies = configuration.GetSection("RestrictedCurrencies").Get<List<string>>();

            RuleFor(query => query.FromCurrency)
               .NotEmpty().WithMessage("From currency cannot be empty.");

            RuleFor(query => query.ToCurrency)
               .NotEmpty().WithMessage("To currency cannot be empty.");

            RuleFor(command => command.FromCurrency)
                .Must(currency => !_restrictedCurrencies.Contains(currency?.ToUpper()))
                .WithMessage($"Conversion from restricted currency is not allowed.");

            RuleFor(command => command.ToCurrency)
                .Must(currency => !_restrictedCurrencies.Contains(currency?.ToUpper()))
                .WithMessage($"Conversion to currency is not allowed.");

            RuleFor(command => command.Amount)
                .GreaterThan(0)
                .WithMessage($"Amount must be greater than zero.");

        }

    }
}
