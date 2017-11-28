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

    public StartMiningRequest(
      string wallet,
      int numberOfThreads,
      string workerName)
    {
      Debug.Assert(string.IsNullOrWhiteSpace(wallet) == false);
      Debug.Assert(numberOfThreads > 0);
      Debug.Assert(string.IsNullOrWhiteSpace(workerName) == false);

      this.wallet = wallet;
      this.numberOfThreads = numberOfThreads;
      this.workerName = workerName;
    }
  }
}
