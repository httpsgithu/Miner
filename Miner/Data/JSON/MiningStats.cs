using Newtonsoft.Json;
using System;

namespace HD
{
  [Serializable]
  public class MiningStats : IMessage
  {
    [JsonProperty]
    public readonly string algorithm;

    [JsonProperty]
    public readonly double hashRate;

    public MiningStats(
      string algorithm,
      double hashRate)
    {
      this.algorithm = algorithm;
      this.hashRate = hashRate;
    }
  }
}
