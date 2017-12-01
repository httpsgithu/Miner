using System;
using System.Text;
using System.Runtime.InteropServices;

namespace HD
{
  /// <summary>
  /// Facade for the XMR C++ dll which does the mining itself.
  /// Xmr-stak-cpu running cryptonight.
  /// </summary>
  public class Xmr
  {
    #region C++ API
    [DllImport(
      "xmr-stak-cpu.dll",
      CharSet = CharSet.Ansi,
      CallingConvention = CallingConvention.Cdecl,
      BestFitMapping = false,
      ThrowOnUnmappableChar = true)]
    static extern bool Start(
      string configurationJson);

    [DllImport(
      "xmr-stak-cpu.dll",
      CharSet = CharSet.Ansi,
      CallingConvention = CallingConvention.Cdecl,
      BestFitMapping = false,
      ThrowOnUnmappableChar = true)]
    static extern double GetTotalHashRate();

    [DllImport(
      "xmr-stak-cpu.dll",
      CharSet = CharSet.Ansi,
      CallingConvention = CallingConvention.Cdecl,
      BestFitMapping = false,
      ThrowOnUnmappableChar = true)]
    static extern bool SleepFor(
      double sleepRate);
    #endregion

    #region Properties
    public double totalHashRate
    {
      get
      {
        double result = GetTotalHashRate() / 1000000;
        if (double.IsNaN(result) || result < 0)
        {
          Log.Warning($"Negative {nameof(totalHashRate)} after calc.. got {result} from {GetTotalHashRate()}");
          result = 0;
        }

        return result;
      }
    }
    #endregion

    #region Write
    /// <remarks>
    /// The Json requires whitespace at the start of the string.
    /// </remarks>
    public void StartMining(
      string wallet,
      int numberOfThreads,
      string workerName,
      string stratumUrl)
    {
      string configurationJson = GenerateConfigJson(wallet, numberOfThreads, workerName, stratumUrl);

      Debug.Assert(string.IsNullOrWhiteSpace(configurationJson) == false);
      Log.Info($"Xmr start mining {stratumUrl} for wallet:{wallet} as worker:{workerName}");

      Start(configurationJson);
    }

    public bool SetSleepFor(
      double sleepRate)
    {
      return SleepFor(sleepRate);
    }
    #endregion

    #region Private
    static string GenerateConfigJson(
      string wallet,
      int numberOfThreads,
      string workerName,
      string stratumUrl)
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
""pool_address"" : """);
      builder.Append(stratumUrl);
      builder.Append(@""",
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
    #endregion
  }
}
