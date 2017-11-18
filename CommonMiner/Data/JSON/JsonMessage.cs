using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Diagnostics;

namespace HD
{
  public sealed class JsonMessage
  {
    // TODO this design is not scalable..
    // maybe use a string class name instead?
    public enum MessageType
    {
      Default, MiningStats
    }

    [JsonProperty]
    MessageType messageType;

    [JsonProperty]
    string bodyJson;

    public JsonMessage() { }

    public JsonMessage(
      object message)
    {
      if (message is MiningStats stats)
      {
        messageType = MessageType.MiningStats;
        bodyJson = JsonConvert.SerializeObject(message);
      }
      else
      {
        Debug.Fail("hmm");
      }
    }

    [JsonIgnore]
    public AbstractMessage message
    {
      get
      {
        switch (messageType)
        {
          case MessageType.MiningStats:
            return JsonConvert.DeserializeObject<MiningStats>(bodyJson);
          default:
            Debug.Fail("missing");
            return null;
        }
      }
    }
  }
}
