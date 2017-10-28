using System;

namespace HD.Algorithms
{
  public class NiceHashResultJson
  {
    public string method = "submit";
    public NiceHashResultJsonParams @params { get; set; }
    public long id { get; set; }

    public NiceHashResultJson(
      long id,
      string jobId,
      uint nonce,
      byte[] result)
    {
      this.id = id;
      @params = new NiceHashResultJsonParams(jobId, nonce, result);
    }
  }
}
