using FluentValidation;

namespace CurrencyConverterBackend.Queries.LatestExchangeRates
{
    public class LatestExchangeRatesQueryValidator : AbstractValidator<LatestExchangeRatesQuery>
    {
        public LatestExchangeRatesQueryValidator()
        {
            RuleFor(query => query.BaseCurrency)
                .NotEmpty().WithMessage("Base currency cannot be empty.");
        }
    }
}
