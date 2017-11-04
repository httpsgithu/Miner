using System;

namespace HD
{
  [Serializable]
  public class MiningStats : JsonMessage
  {
    public string algorithm;

    public int hashRate;

    public int acceptedHashRate;
  }
}
