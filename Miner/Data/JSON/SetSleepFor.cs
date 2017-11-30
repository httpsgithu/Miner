using Newtonsoft.Json;
using System;

namespace HD
{
  /// <summary>
  /// Communicates how long to sleep frontend -> middleware.
  /// </summary>
  [JsonObject(MemberSerialization.OptIn)]
  public class SetSleepFor
  {
    [JsonProperty]
    public double sleepRate;

    public SetSleepFor(
      double sleepFor)
    {
      Debug.Assert(sleepFor >= 0);

      this.sleepRate = sleepFor;
    }
  }
}
