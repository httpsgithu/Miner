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

    public readonly byte[] memoryHardLoop_A = new byte[CryptoNight.sizeOfBlock];
    public readonly byte[] memoryHardLoop_B = new byte[CryptoNight.sizeOfBlock];
    /// <summary>
    /// byte[8][16]
    /// </summary>
    public readonly byte[][] blocks = new byte[CryptoNight.numberOfBlocks][];
    public readonly byte[] key = new byte[CryptoNight.sizeOfKey];

    public readonly AesEngine aes = new AesEngine();

    public ulong piHashVal
    {
      get
      {
        return BitConverter.ToUInt64(bResult, CryptoNight.hashValOffsetInResult);
      }
    }

    public CryptoNightDataPerThread()
    {
      for (int i = 0; i < CryptoNight.numberOfBlocks; i++)
      {
        blocks[i] = new byte[CryptoNight.sizeOfBlock];
      }
    }
  }
}
