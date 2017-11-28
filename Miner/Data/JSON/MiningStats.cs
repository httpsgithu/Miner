using Newtonsoft.Json;
using System;

namespace HD
{
  /// <summary>
  /// Communicates mining performance middleware -> frontend.
  /// </summary>
  [JsonObject(MemberSerialization.OptIn)]
  public class MiningStats
  {
    [JsonProperty]
    public readonly string algorithm;

    [JsonProperty]
    public readonly double hashRate;

    public MiningStats(
      string algorithm,
      double hashRate)
    {
      Debug.Assert(string.IsNullOrWhiteSpace(algorithm) == false);
      Debug.Assert(hashRate >= 0, $"Negative {nameof(hashRate)}.. got {hashRate}");

      this.algorithm = algorithm;
      this.hashRate = hashRate;
    }
  }
}
