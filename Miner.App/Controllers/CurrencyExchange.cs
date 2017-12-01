using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Data.JSON;

namespace HD.Controllers
{
    public class CurrencyExchange
    {
        private CurrencyExchangeRates rates;
        private decimal amount;

        public string Name { get { return rates.@base; } }

        internal CurrencyExchange(CurrencyExchangeRates rates, decimal amount)
        {
            this.rates = rates;
            this.amount = amount;
        }

        public decimal To(string targetCurrency)
        {
            targetCurrency = targetCurrency.ToUpperInvariant();

            // trying to convert to the same currency we're already in?
            if (targetCurrency == Name)
                return amount;

            if (rates.rates.ContainsKey(targetCurrency))
                return rates.rates[targetCurrency] * amount;

            throw new InvalidOperationException($"Invalid currency name '{targetCurrency}'");
        }
    }
}
