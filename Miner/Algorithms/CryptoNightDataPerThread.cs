using System;

namespace HD.Algorithms
{
  /// <summary>
  /// I believe this is the only information which differs per thread.
  /// 
  /// TODO perf: reuse the arrays
  /// </summary>
  public class CryptoNightDataPerThread
  {



    public readonly byte[] bResult = new byte[CryptoNight.sizeOfResult];

    public ulong piHashVal
    {
      get
      {
        return BitConverter.ToUInt64(bResult, CryptoNight.hashValOffsetInResult);
      }
    }
  }
}