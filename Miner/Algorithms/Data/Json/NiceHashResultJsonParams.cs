using System;

namespace HD.Algorithms
{
  public class NiceHashResultJsonParams
  {
    /// <summary>
    /// This is the connection ID.  Sent by NiceHash only in the first packet.
    /// 
    /// TODO fire up
    /// </summary>
    public string id = "421970933313569"; 
    public string job_id { get; set; }
    public string nonce { get; set; }
    public string result { get; set; }

    public NiceHashResultJsonParams(
      string job_id,
      uint nonce,
      byte[] result)
    {
      this.job_id = job_id;
      this.nonce = BitConverter.GetBytes(nonce).ToHexString();
      this.result = result.ToHexString();
    }
  }
}
