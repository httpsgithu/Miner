using System;

namespace HD
{
  public class NewJob
  {
    public long Id { get; set; }
    public object Error { get; set; }
    public string Jsonrpc { get; set; }
    public Result Result { get; set; }
  }

  public class Result
  {
    public Job Job { get; set; }
    public string Id { get; set; }
    public string Status { get; set; }
  }


  public class NewBlock
  {
    public Job @params { get; set; }
    public string jsonrpc { get; set; }
    public string method { get; set; }
  }

  public class Job
  {
    public string Job_Id { get; set; }
    public string Blob { get; set; }
    public string Target { get; set; }
  }
}
