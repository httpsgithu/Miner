using System;
using System.Diagnostics;
using Newtonsoft.Json;
using CryptoHash;
using HD.Algorithms;
using System.Security.Cryptography;

namespace HD
{
  public class CryptoNight
  {
    #region Data
    // Magic numbers
    const int iWorkSize = 76;
    const int nonceIndexInWorkBlob = 39;
    public const int sizeOfKeccakHash = 200;
    public const int sizeOfScratchpad = 2097152;
    public const int sizeOfBlock = 16;
    public const int numberOfBlocks = 8;
    public const int hashValOffsetInResult = 24;
    public const int sizeOfKey = 32;
    public const int sizeOfResult = 32;
    const int sizeOfScratchpadSegment = 128;
    const int numberOfScratchpadSegments = sizeOfScratchpad / sizeOfScratchpadSegment;
    const int hardLoopIterrationCount = 524288;
    const int scratchpadAddressBitmask = 0x1FFFF0;

    // TODO this is thread stuff
    const int iThreadCount = 1;
    const int iThreadNo = 0;
    const int iResumeCnt = 0;

    public readonly byte[] bWorkBlob = new byte[iWorkSize]; // TODO is this shared or per thread

    /// <summary>
    /// Not readonly as it changes each time a new job is requested.
    /// </summary>
    public string jobId;

    /// <summary>
    /// Defines the goal RE the difficultly requirement.
    /// </summary>
    public ulong iTarget;

    long requestId;

    /// <summary>
    /// TODO threads need to lock
    /// </summary>
    public uint piNonce
    {
      get
      {
        return BitConverter.ToUInt32(bWorkBlob, nonceIndexInWorkBlob);
      }
      set
      {
        value.GetBytes(bWorkBlob, nonceIndexInWorkBlob);
      }
    }

    event Action<string> onComplete;
    #endregion

    #region Public API
    public void Process(
      Action<string> onComplete,
      int requestId,
      string jobId,
      string blob,
      string target,
      string nonceOverride = null)
    {
      this.onComplete = onComplete;
      this.requestId = requestId;
      this.jobId = jobId;
      blob.ToByteArrayFromHex(bWorkBlob);
      iTarget = t32_to_t64(hex2bin(target));
      InitNonce(nonceOverride);

      RunPerThread();
    }
    #endregion

    unsafe void RunPerThread()
    {
      byte[] resultData = new byte[sizeOfResult];
      fixed (byte* resultDataPointer = resultData)
      {
        ulong* piHashVal = (ulong*)(resultDataPointer + hashValOffsetInResult);

        byte[] ctxkeccakHash = new byte[sizeOfKeccakHash];
        byte[] ctxscratchpad = new byte[sizeOfScratchpad];
        byte[] ctxkey = new byte[sizeOfKey];

        AesEngine aes = new AesEngine();
        using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
        {



          byte* blockData = stackalloc byte[numberOfBlocks * sizeOfBlock];
          ulong* blockDataUlong = (ulong*)blockData;
          uint* blockDataUint = (uint*)blockData;

          ulong memoryHardLoop_Afirst;
          ulong memoryHardLoop_Asecond;
          ulong memoryHardLoop_Bfirst;
          ulong memoryHardLoop_Bsecond;

          byte[] keyAsBytes = new byte[AesEngine.numberOfUintsPerKey * sizeof(uint)];
          fixed (byte* keyAsByteData = keyAsBytes)
          {
            uint* keyAsUint = (uint*)keyAsByteData;
            ulong* keyAsUlong = (ulong*)keyAsByteData;
            byte[] tempBlock = new byte[sizeOfBlock];
            fixed (byte* tempBlockBytes = tempBlock)
            {
              uint* tempBlockAsUint = (uint*)tempBlockBytes;
              ulong* tempBlockAsUlong = (ulong*)tempBlockBytes;


              while (true) // TODO move the loop into code not test
              {
                fixed (byte* scratchpadBytes = ctxscratchpad) // TODO fixed outsid ethe loop
                {
                  ulong* scratchpadLong = (ulong*)scratchpadBytes;
                  piNonce++; // TODO thinking maybe a shared counter starting from a random number -- but note valid bounds

                  KeccakDigest.keccak(bWorkBlob, iWorkSize, ctxkeccakHash, sizeOfKeccakHash);
                  ExtractAndInitAesKey(aes, true, ctxkey, ctxkeccakHash);
                  ExtractBlocksFromHash(blockDataUlong, ctxkeccakHash);

                  for (int scratchIndex = 0; scratchIndex < numberOfScratchpadSegments; scratchIndex++)
                  {
                    for (int blockIndex = 0; blockIndex < numberOfBlocks; blockIndex++)
                    {
                      aes.ProcessBlock(blockDataUint + blockIndex * sizeOfBlock / sizeof(uint));
                    }

                    for (int blockIndex = 0; blockIndex < numberOfBlocks; blockIndex++)
                    {
                      int index = (scratchIndex * numberOfBlocks * sizeOfBlock + blockIndex * sizeOfBlock) / sizeof(ulong);
                      scratchpadLong[index] = blockDataUlong[blockIndex * 2];
                      scratchpadLong[index + 1] = blockDataUlong[blockIndex * 2 + 1];
                    }
                  }

                  fixed (byte* key = ctxkey)
                  {
                    ulong* longKey = (ulong*)key;

                    fixed (byte* hash2 = ctxkeccakHash)
                    {
                      ulong* longHash = (ulong*)hash2;

                      memoryHardLoop_Afirst = longKey[0] ^ longHash[sizeOfKey / sizeof(long)];
                      memoryHardLoop_Asecond = longKey[1] ^ longHash[sizeOfKey / sizeof(long) + 1];
                      memoryHardLoop_Bfirst = longKey[2] ^ longHash[sizeOfKey / sizeof(long) + 2];
                      memoryHardLoop_Bsecond = longKey[3] ^ longHash[sizeOfKey / sizeof(long) + 3];
                    }
                  }

                  // TODO test ctx locally

                  int idx0Address = (int)(memoryHardLoop_Afirst & scratchpadAddressBitmask) / sizeof(long);



                  // TODO don't need bytes anymore?
                  // TODO test stackalloc scratchpad?

                  for (int i = 0; i < hardLoopIterrationCount; i++)
                  {
                    tempBlockAsUlong[0] = scratchpadLong[idx0Address];
                    tempBlockAsUlong[1] = scratchpadLong[idx0Address + 1];

                    keyAsUlong[0] = memoryHardLoop_Afirst;
                    keyAsUlong[1] = memoryHardLoop_Asecond;

                    AesEngine.Encrypt(tempBlockAsUint, keyAsUint);

                    //aesProvider.Mode = CipherMode.;
                    //aesProvider.BlockSize = 128;
                    //aesProvider.FeedbackSize = ;
                    //aesProvider.IV = ;
                    //aesProvider.Key = keyAsBytes;
                    //aesProvider.Mode = CipherMode.ECB;
                    //aesProvider.Mode = ;
                    //aesProvider.Padding = ;
                    //var encryptor = aesProvider.CreateEncryptor();
                    //encryptor.TransformBlock(tempBlock, 0, sizeOfBlock, tempBlock, 0);


                    //aesProvider.BlockSize = sizeOfBlock * 8;
                    //aesProvider.Key = keyAsBytes;
                    ////aesProvider.IV = tempBlock;
                    //ICryptoTransform aesEncrypter = aesProvider.CreateEncryptor();
                    //aesEncrypter.TransformBlock(tempBlock, 0, sizeOfBlock, tempBlock, 0);

                    scratchpadLong[idx0Address] = tempBlockAsUlong[0] ^ memoryHardLoop_Bfirst;
                    scratchpadLong[idx0Address + 1] = tempBlockAsUlong[1] ^ memoryHardLoop_Bsecond;

                    idx0Address = (int)(tempBlockAsUlong[0] & scratchpadAddressBitmask) / sizeof(long);

                    memoryHardLoop_Bfirst = tempBlockAsUlong[0];
                    memoryHardLoop_Bsecond = tempBlockAsUlong[1];

                    tempBlockAsUlong[0].UnsignedMultiply128(scratchpadLong[idx0Address],
                      out ulong mulIntFirst, out ulong mulIntSecond);

                    unchecked
                    {
                      memoryHardLoop_Afirst += mulIntSecond;
                      memoryHardLoop_Asecond += mulIntFirst;
                    }

                    tempBlockAsUlong[0] = scratchpadLong[idx0Address];
                    tempBlockAsUlong[1] = scratchpadLong[idx0Address + 1];

                    scratchpadLong[idx0Address] = memoryHardLoop_Afirst;
                    scratchpadLong[idx0Address + 1] = memoryHardLoop_Asecond;

                    memoryHardLoop_Afirst ^= tempBlockAsUlong[0];
                    memoryHardLoop_Asecond ^= tempBlockAsUlong[1];

                    idx0Address = (int)(memoryHardLoop_Afirst & scratchpadAddressBitmask) / sizeof(long);
                  }


                  ExtractAndInitAesKey(aes, false, ctxkey, ctxkeccakHash);
                  ExtractBlocksFromHash(blockDataUlong, ctxkeccakHash);

                  for (int scratchIndex = 0; scratchIndex < numberOfScratchpadSegments; scratchIndex++)
                  {
                    for (int blockIndex = 0; blockIndex < 8; blockIndex++)
                    {
                      int index = (scratchIndex * 128 + blockIndex * sizeOfBlock) / sizeof(ulong);
                      blockDataUlong[blockIndex * 2] ^= scratchpadLong[index];
                      blockDataUlong[blockIndex * 2 + 1] ^= scratchpadLong[index + 1];
                    }

                    EncryptBlocks(aes, blockDataUint);
                  }
                }


                for (int blockIndex = 0; blockIndex < 8; blockIndex++)
                {
                  for (int byteIndex = 0; byteIndex < 16; byteIndex++)
                  {
                    ctxkeccakHash[64 + byteIndex + blockIndex * 16] = blockData[blockIndex * sizeOfBlock + byteIndex];
                  }
                }


                ulong[] tempLong = new ulong[sizeOfKeccakHash / sizeof(ulong) / sizeof(byte)];
                for (int i = 0; i < tempLong.Length; i++)
                {
                  tempLong[i] = BitConverter.ToUInt64(ctxkeccakHash, i * sizeof(ulong) / sizeof(byte));
                }

                KeccakDigest.keccakf(tempLong, KeccakDigest.KECCAK_ROUNDS);

                for (int longIndex = 0; longIndex < tempLong.Length; longIndex++)
                {
                  byte[] longData = BitConverter.GetBytes(tempLong[longIndex]);
                  for (int byteIndex = 0; byteIndex < 8; byteIndex++)
                  {
                    ctxkeccakHash[longIndex * sizeof(ulong) + byteIndex] = longData[byteIndex];
                  }
                }


                Digest hash;
                switch (ctxkeccakHash[0] & 3)
                {
                  case 0:
                    {
                      hash = new BLAKE256();
                      break;
                    }
                  case 1:
                    {
                      hash = new Groestl256();
                      break;
                    }
                  case 2:
                    {
                      hash = new JH256();
                      break;
                    }
                  case 3:
                    {
                      hash = new Skein256();
                      break;
                    }
                  default:
                    Debug.Fail("Missing hash?!");
                    hash = null;
                    break;
                }

                hash.update(ctxkeccakHash);
                hash.digest(resultData, 0, 32);

                if (*piHashVal < iTarget)
                {
                  NiceHashResultJson json = new NiceHashResultJson(requestId, jobId, piNonce, resultData);
                  onComplete(JsonConvert.SerializeObject(json));
                  return; // TODO should loop for another job
                }
              }
            }
          }
        }
      }
    }

    #region Write Helpers
    void InitNonce(
     string nonceOverride)
    {
      piNonce = calc_nicehash_nonce(piNonce, iResumeCnt);
      if (nonceOverride != null)
      {
        piNonce = uint.Parse(nonceOverride, System.Globalization.NumberStyles.HexNumber);
      }
    }

    unsafe void ExtractBlocksFromHash(
      ulong* blocks,
      byte[] ctxkeccakHash)
    {
      fixed (byte* hashBytes = ctxkeccakHash)
      {
        ulong* hashLong = (ulong*)hashBytes;

        for (int blockIndex = 0; blockIndex < numberOfBlocks; blockIndex++)
        {
          blocks[blockIndex * 2] = hashLong[(64 + blockIndex * 16) / sizeof(ulong)];
          blocks[blockIndex * 2 + 1] = hashLong[(64 + blockIndex * 16) / sizeof(ulong) + 1];
        }
      }
    }

    unsafe void EncryptBlocks(
      AesEngine aes,
      uint* blocks)
    {
      for (int blockIndex = 0; blockIndex < 8; blockIndex++)
      {
        aes.ProcessBlock(blocks + blockIndex * sizeOfBlock / sizeof(uint));
      }
    }

    void ExtractAndInitAesKey(
      AesEngine aes,
      bool useFirstSegmentVsSecond,
      byte[] ctxkey,
      byte[] ctxkeccakHash)
    {
      for (int i = 0; i < sizeOfKey; i++)
      {
        int index = i;
        if (useFirstSegmentVsSecond == false)
        {
          index += sizeOfKey;
        }
        ctxkey[i] = ctxkeccakHash[index];
      }

      aes.Init(ctxkey);
    }
    #endregion

    #region Read Helpers
    uint hex2bin(
      string input)
    {
      byte[] output = new byte[4]; // TODO perf: this could be cached, one per thread.
      for (int i = 0; i < 8; i += 2)
      {
        output[i / 2] = (byte)((hf_hex2bin((byte)input[i]) << 4) | hf_hex2bin((byte)input[i + 1]));
      }
      return BitConverter.ToUInt32(output, 0);
    }

    byte hf_hex2bin(
      byte c)
    {
      if (c >= '0' && c <= '9')
      {
        return (byte)(c - '0');
      }
      else if (c >= 'a' && c <= 'f')
      {
        return (byte)(c - 'a' + 0xA);
      }
      else if (c >= 'A' && c <= 'F')
      {
        return (byte)(c - 'A' + 0xA);
      }

      Debug.Fail("Hex conversion failed");
      return 0;
    }

    uint calc_nicehash_nonce(
          uint start,
          uint resume)
    {
      // TODO select a random starting nonce in range
      return start | (resume * iThreadCount + iThreadNo) << 18;
    }

    static ulong t32_to_t64(
      uint t)
    {
      return 0xFFFFFFFFFFFFFFFF / (0xFFFFFFFF / ((ulong)t));
    }
    #endregion
  }
}