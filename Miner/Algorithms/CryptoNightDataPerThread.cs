using Org.BouncyCastle.Crypto.Engines;
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
    public readonly byte[] keccakHash = new byte[200];
    public readonly byte[] scratchpad = new byte[2097152];
    public readonly byte[] bResult = new byte[32];

    public readonly byte[] memoryHardLoop_A = new byte[16];
    public readonly byte[] memoryHardLoop_B = new byte[16];
    /// <summary>
    /// byte[8][16]
    /// </summary>
    public readonly byte[][] blocks = new byte[8][];
    public readonly byte[] key = new byte[32];

    public readonly AesEngine aes = new AesEngine();

    public ulong piHashVal
    {
      get
      {
        return BitConverter.ToUInt64(bResult, 24);
      }
    }

    public CryptoNightDataPerThread()
    {
      for (int i = 0; i < blocks.Length; i++)
      {
        blocks[i] = new byte[16];
      }
    }
  }
}
