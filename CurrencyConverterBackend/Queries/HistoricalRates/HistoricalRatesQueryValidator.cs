using FluentValidation;

namespace CurrencyConverterBackend.Queries.HistoricalRates
{
    public class HistoricalRatesQueryValidator : AbstractValidator<HistoricalRatesQuery>
    {
        public HistoricalRatesQueryValidator()
        {
            RuleFor(query => query.BaseCurrency)
                .NotEmpty().WithMessage("Base currency cannot be empty.");

            RuleFor(query => query.StartDate)
                .NotEmpty().WithMessage("Start date cannot be empty.")
                .Matches(@"^\d{4}-\d{2}-\d{2}$").WithMessage("Start date must be in yyyy-MM-dd format.");

            RuleFor(query => query.EndDate)
               .NotEmpty().WithMessage("End date cannot be empty.")
               .Matches(@"^\d{4}-\d{2}-\d{2}$").WithMessage("End date must be in yyyy-MM-dd format.");

            RuleFor(query => query.Page)
                .GreaterThan(0).WithMessage("Page number must be greater than zero.");

            RuleFor(query => query.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than zero.");
        }
    }
}
