namespace CurrencyConverterBackend.Models
{
    public class HistoricalRatesResponse
    {
        public double Amount { get; set; }
        public string Base { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IDictionary<DateTime, IDictionary<string, double>> Rates { get; set; }
    }
}
