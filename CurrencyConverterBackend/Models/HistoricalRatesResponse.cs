namespace CurrencyConverterBackend.Models
{
    public class HistoricalRatesResponse
    {
        public double Amount { get; set; }
        public string Base { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public IDictionary<string, IDictionary<string, double>> Rates { get; set; }
    }
}
