using System;
using Newtonsoft.Json;

namespace HD
{
  public static class MinerGlobalVariables
  {
    /// <summary>
    /// Used for TCP communication with the middleware
    /// </summary>
    /// <remarks>max port 65535</remarks>
    public const int internalServerPort =
#if DEBUG
      62816;
#else
      62817;
#endif

    /// <summary>
    /// Should be used for all JSON serializing and deserializing.
    /// </summary>
    public static readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
      {
        TypeNameHandling = TypeNameHandling.All, // TODO is auto okay?
      };
  }
}
