using Newtonsoft.Json;
using System;

namespace HD
{
  /// <summary>
  /// Communicates the config required to start mining frontend -> middleware.
  /// </summary>
  [JsonObject(MemberSerialization.OptIn)]
  public class StartMiningRequest
  {
    [JsonProperty]
    public readonly string wallet;

    [JsonProperty]
    public readonly int numberOfThreads;

    [JsonProperty]
    public readonly string workerName;

    [JsonProperty]
    public readonly string stratumUrl;

    public StartMiningRequest(
      string wallet,
      int numberOfThreads,
      string workerName,
      string stratumUrl)
    {
      Debug.Assert(string.IsNullOrWhiteSpace(wallet) == false);
      Debug.Assert(numberOfThreads > 0);
      Debug.Assert(string.IsNullOrWhiteSpace(workerName) == false);
      Debug.Assert(string.IsNullOrWhiteSpace(stratumUrl) == false);

      this.wallet = wallet;
      this.numberOfThreads = numberOfThreads;
      this.workerName = workerName;
      this.stratumUrl = stratumUrl;
    }
  }
}
