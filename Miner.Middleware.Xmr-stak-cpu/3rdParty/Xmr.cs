using System;
using System.Diagnostics;
using System.Text;

namespace HD
{
  public static class Xmr
  {
    public static string GenerateConfigJson(
      string wallet,
      int numberOfThreads,
      string workerName)
    {
      StringBuilder builder = new StringBuilder();

      builder.Append(@"
""cpu_threads_conf"" :
[
");
      for (int i = 0; i < numberOfThreads; i++)
      {
        builder.Append(@"{ ""low_power_mode"" : false, ""no_prefetch"" : true, ""affine_to_cpu"" : ");
        int aff = i < Environment.ProcessorCount / 2 ? i * 2 : (i - Environment.ProcessorCount / 2) * 2 + 1;
        Debug.Assert(aff < Environment.ProcessorCount);
        builder.Append(aff);
        builder.Append(
@"},
");
      }
      builder.Append(@"],
""use_slow_memory"" : ""warn"",
""nicehash_nonce"" : true,
""aes_override"" : null,
""use_tls"" : false,
""tls_secure_algo"" : true,
""tls_fingerprint"" : """",
""pool_address"" : ""cryptonight.usa.nicehash.com:3355"",
""wallet_address"" : """);
      builder.Append(wallet);
      if (string.IsNullOrEmpty(workerName) == false)
      {
        builder.Append(@".");
        builder.Append(workerName);
      }
      builder.Append(@""",
""pool_password"" : ""x"",
""call_timeout"" : 10,
""retry_time"" : 10,
""giveup_limit"" : 0,
""verbose_level"" : 4,
""h_print_time"" : 3,
""daemon_mode"" : false,
""output_file"" : ""output.log"", 
""httpd_port"" : 0,
""prefer_ipv4"" : true
"); 
      return builder.ToString();
    }
  }
}
