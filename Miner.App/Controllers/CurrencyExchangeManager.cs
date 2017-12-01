using HD.Data.JSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Controllers
{
    public static class CurrencyExchangeManager
    {
        const string API_URL = "https://api.fixer.io/latest?base=";
        static Dictionary<string, (DateTime lastUpdated, CurrencyExchangeRates values)> rates = new Dictionary<string, (DateTime, CurrencyExchangeRates)>();

        public static CurrencyExchange From(decimal amount, string baseCurrency, bool forceUpdate = false)
        {
            baseCurrency = baseCurrency.ToUpperInvariant();
            if (AreRatesOutdated(baseCurrency) || forceUpdate)
            {
                Task.Run(() => { Fetch(baseCurrency); });
                return new CurrencyExchange(null, 0);
            }
            return new CurrencyExchange(rates[baseCurrency].values, amount);
        }

        private static bool AreRatesOutdated(string baseCurrency)
        {
            if (!rates.ContainsKey(baseCurrency))
                return true;

            // Update once per hour
            if ((DateTime.UtcNow - rates[baseCurrency].lastUpdated).TotalHours >= 1)
                return true;

            return false;
        }

        private static void Fetch(string baseCurrency)
        {
            var dataString = Encoding.UTF8.GetString(HDWebClient.GetBytes($"{API_URL}{baseCurrency}"));
            var obj = JsonConvert.DeserializeObject<CurrencyExchangeRates>(dataString);
            rates[baseCurrency] = (lastUpdated: DateTime.UtcNow, values: obj);
        }
    }
}
