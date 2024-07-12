﻿namespace CurrencyConverterBackend.Models
{
    public class ConversionResponse
    {
        public decimal Amount { get; set; }
        public string Base { get; set; }
        public string Date { get; set; }
        public IDictionary<string, decimal> Rates { get; set; }
    }
}
