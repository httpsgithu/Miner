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

    [JsonProperty]
    public readonly double acceptedHashRate;

    public MiningStats(
      string algorithm,
      double hashRate,
      double acceptedHashRate)
    {
      this.algorithm = algorithm;
      this.hashRate = hashRate;
      this.acceptedHashRate = acceptedHashRate;
    }
  }
}
