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

    public decimal To(HD.Currency targetCurrency)
    {
      var currencyName = targetCurrency.ToString();

      // trying to convert to the same currency we're already in?
      if (rates == null || currencyName == Name)
        return amount;

      if (rates.rates.ContainsKey(currencyName))
        return rates.rates[currencyName] * amount;

      throw new InvalidOperationException($"Invalid currency name '{currencyName}'");
    }
  }
}
