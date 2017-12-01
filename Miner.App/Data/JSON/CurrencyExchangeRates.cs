using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Data.JSON
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CurrencyExchangeRates
    {
        [JsonProperty]
        public readonly string @base;

        [JsonProperty]
        public readonly DateTime date;

        [JsonProperty]
        public readonly Dictionary<string, decimal> rates;
    }
}
