using Newtonsoft.Json;
using System;

namespace HD
{
  public class StartMiningRequest : IMessage
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
      this.wallet = wallet;
      this.numberOfThreads = numberOfThreads;
      this.workerName = workerName;
    }
  }
}
