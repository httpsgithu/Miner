using System;

namespace HD
{
  [Serializable]
  public class MiningStats : AbstractMessage
  {
    public string algorithm;

    public int hashRate;

    public int acceptedHashRate;

    public MiningStats(
      string algorithm,
      int hashRate,
      int acceptedHashRate)
    {
      this.algorithm = algorithm;
      this.hashRate = hashRate;
      this.acceptedHashRate = acceptedHashRate;
    }
  }
}
