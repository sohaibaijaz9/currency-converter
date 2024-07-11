namespace CurrencyConverterBackend.Models
{
    public class ExchangeRateResponse
    {
        public decimal Amount { get; set; }
        public string Base { get; set; }
        public DateTime Date { get; set; }
        public IDictionary<string, decimal> Rates { get; set; }
    }
}
