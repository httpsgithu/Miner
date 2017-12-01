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
        static int isUpdating = 0;

        public static CurrencyExchange From(decimal amount, HD.Currencies baseCurrency, bool forceUpdate = false)
        {
            var currencyName = baseCurrency.ToString();
            if (AreRatesOutdated(currencyName) || forceUpdate)
            {
                // Don't have rates yet, return -1 for now
                Task.Run(() => { Fetch(currencyName); });
                return new CurrencyExchange(null, -1);
            }
            return new CurrencyExchange(rates[currencyName].values, amount);
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
            if (System.Threading.Interlocked.CompareExchange(ref isUpdating, 1, 0) == 0)
            {
                var dataString = Encoding.UTF8.GetString(HDWebClient.GetBytes($"{API_URL}{baseCurrency}"));
                var obj = JsonConvert.DeserializeObject<CurrencyExchangeRates>(dataString);
                rates[baseCurrency] = (lastUpdated: DateTime.UtcNow, values: obj);
                isUpdating = 0;
            }
        }
    }
}
