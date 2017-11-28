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
    public long sleepForInNanoseconds;

    public SetSleepFor(
      long sleepFor)
    {
      Debug.Assert(sleepFor >= 0);

      this.sleepForInNanoseconds = sleepFor;
    }
  }
}
