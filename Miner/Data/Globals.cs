using Newtonsoft.Json;
using System;

namespace HD
{
  public static class Globals
  {
    // max port 65535
    public const int zeroMqPortServer = 62817;
    public const int zeroMqPortClient = 62818;

    public static readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
    {
      TypeNameHandling = TypeNameHandling.All, // TODO is auto okay?
    };
    public const string launchedByInstallerArg = "Installed";
  }
}
