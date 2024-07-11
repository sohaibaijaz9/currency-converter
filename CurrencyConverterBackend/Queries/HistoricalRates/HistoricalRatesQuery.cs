namespace CurrencyConverterBackend.Queries.HistoricalRates
{
    public class HistoricalRatesQuery
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string BaseCurrency { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
