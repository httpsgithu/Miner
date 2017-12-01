using Newtonsoft.Json;
using System;

namespace HD
{
  [JsonObject(MemberSerialization.OptIn)]
  public class WorkIsStalled
  {
    [JsonProperty]
    public readonly bool isStalled;

    public WorkIsStalled(
      bool isStalled)
    {
      this.isStalled = isStalled;
    }
  }
}
