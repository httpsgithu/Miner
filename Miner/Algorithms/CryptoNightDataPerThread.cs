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
    public readonly byte[] keccakHash = new byte[CryptoNight.sizeOfKeccakHash];
    public readonly byte[] scratchpad = new byte[CryptoNight.sizeOfScratchpad];
    public readonly byte[] bResult = new byte[CryptoNight.sizeOfResult];


    public readonly byte[] key = new byte[CryptoNight.sizeOfKey];


    public ulong piHashVal
    {
      get
      {
        return BitConverter.ToUInt64(bResult, CryptoNight.hashValOffsetInResult);
      }
    }
  }
}