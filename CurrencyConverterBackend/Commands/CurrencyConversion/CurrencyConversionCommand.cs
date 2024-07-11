namespace CurrencyConverterBackend.Commands.CurrencyConversion
{
    public class CurrencyConversionCommand
    {
        public string FromCurrency {  get; set; }   
        public string ToCurrency { get; set; }  
        public decimal Amount { get; set; }

    }
}
