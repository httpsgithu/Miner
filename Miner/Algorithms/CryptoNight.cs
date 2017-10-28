using System;
using System.Diagnostics;
using Newtonsoft.Json;
using CryptoHash;
using HD.Algorithms;

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

    public readonly byte[] bWorkBlob = new byte[iWorkSize];

    /// <summary>
    /// Not readonly as it changes each time a new job is requested.
    /// </summary>
    public string jobId;

    /// <summary>
    /// This is the only per-thread information
    /// </summary>
    public readonly CryptoNightDataPerThread ctx = new CryptoNightDataPerThread();

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
    #endregion

    #region Public API
    public void Process(
      int requestId,
      string jobId,
      string blob,
      string target,
      string nonceOverride = null)
    {
      this.requestId = requestId;
      this.jobId = jobId;
      blob.ToByteArrayFromHex(bWorkBlob);
      iTarget = t32_to_t64(hex2bin(target));
      InitNonce(nonceOverride);
    }

    public string GetResultJson()
    {
      NiceHashResultJson json = new NiceHashResultJson(requestId, jobId, piNonce, ctx.bResult);
      return JsonConvert.SerializeObject(json);
    }
    #endregion

    public void Step2_IncrementNonce()
    {
      piNonce++; // TODO thinking maybe a shared counter starting from a random number -- but note valid bounds
    }

    public void Step3_HashAndExtractBlocks()
    {
      KeccakDigest.keccak(bWorkBlob, iWorkSize, ctx.keccakHash, sizeOfKeccakHash);
      ExtractAndInitAesKey(true);
      ExtractBlocksFromHash();
    }

    public void Step4_EncryptBlocksCreateScratchpad()
    {
      for (int scratchIndex = 0; scratchIndex < numberOfScratchpadSegments; scratchIndex++)
      {
        for (int blockIndex = 0; blockIndex < numberOfBlocks; blockIndex++)
        {
          ctx.aes.ProcessBlock(ctx.blocks[blockIndex], 0, ctx.blocks[blockIndex], 0);
        }

        for (int blockIndex = 0; blockIndex < numberOfBlocks; blockIndex++)
        {
          byte[] block = ctx.blocks[blockIndex];
          for (int byteIndex = 0; byteIndex < sizeOfBlock; byteIndex++)
          {
            int index = scratchIndex * numberOfBlocks * sizeOfBlock + blockIndex * sizeOfBlock + byteIndex;
            ctx.scratchpad[index] = block[byteIndex];
          }
        }
      }
    }

    public void Step5_InitHardLoopAAndB()
    {
      for (int i = 0; i < sizeOfBlock; i++)
      {
        ctx.memoryHardLoop_A[i] = (byte)(ctx.key[i] ^ ctx.keccakHash[sizeOfKey + i]);
        ctx.memoryHardLoop_B[i] = (byte)(ctx.key[i + sizeOfBlock] ^ ctx.keccakHash[sizeOfKey + i + sizeOfBlock]);
      }
    }

    public void ProcessStep10()
    {
      ulong idx0 = BitConverter.ToUInt64(ctx.memoryHardLoop_A, 0);

      // TODO can we reuse these?
      byte[] cx = new byte[sizeOfBlock];
      uint[] key = new uint[AesEngine.numberOfUintsPerKey];

      for (int i = 0; i < hardLoopIterrationCount; i++)
      {
        for (int tempIndex = 0; tempIndex < sizeOfBlock; tempIndex++)
        {
          cx[tempIndex] = ctx.scratchpad[(int)(idx0 & scratchpadAddressBitmask) + tempIndex];
        }

        for (int keyIndex = 0; keyIndex < AesEngine.numberOfUintsPerKey; keyIndex++)
        {
          key[keyIndex] = BitConverter.ToUInt32(ctx.memoryHardLoop_A, keyIndex * 4);
        }

        ctx.aes.Encrypt(cx, 0, key, cx, 0);

        for (int bIndex = 0; bIndex < sizeOfBlock; bIndex++)
        {
          ctx.scratchpad[(int)(idx0 & scratchpadAddressBitmask) + bIndex] = (byte)(cx[bIndex] ^ ctx.memoryHardLoop_B[bIndex]);
        }

        idx0 = BitConverter.ToUInt64(cx, 0);

        for (int bIndex = 0; bIndex < sizeOfBlock; bIndex++)
        {
          ctx.memoryHardLoop_B[bIndex] = cx[bIndex];
        }

        ulong cl, ch;
        cl = BitConverter.ToUInt64(ctx.scratchpad, (int)(idx0 & scratchpadAddressBitmask));
        ch = BitConverter.ToUInt64(ctx.scratchpad, (int)(idx0 & scratchpadAddressBitmask) + sizeof(ulong));

        idx0.UnsignedMultiply128(cl, out ulong mulIntLow, out ulong mulIntHigh);
        ulong aIntLow = BitConverter.ToUInt64(ctx.memoryHardLoop_A, 0);
        ulong aIntHigh = BitConverter.ToUInt64(ctx.memoryHardLoop_A, 8);
        unchecked
        {
          aIntLow += mulIntHigh;
          aIntHigh += mulIntLow;
        }
        aIntLow.GetBytes(ctx.memoryHardLoop_A, 0);
        aIntHigh.GetBytes(ctx.memoryHardLoop_A, sizeof(ulong));

        for (int aIndex = 0; aIndex < sizeOfBlock; aIndex++)
        {
          cx[aIndex] = ctx.scratchpad[(int)(idx0 & scratchpadAddressBitmask) + aIndex];
        }

        for (int aIndex = 0; aIndex < sizeOfBlock; aIndex++)
        {
          ctx.scratchpad[(int)(idx0 & scratchpadAddressBitmask) + aIndex] = ctx.memoryHardLoop_A[aIndex];
          ctx.memoryHardLoop_A[aIndex] ^= cx[aIndex];
        }

        idx0 = BitConverter.ToUInt64(ctx.memoryHardLoop_A, 0);
      }
    }

    public void ProcessStep11()
    {
      ExtractAndInitAesKey(false);
    }



    public void ProcessStep12()
    {
      ExtractBlocksFromHash();

      //for (int byteIndex = 0; byteIndex < 128; byteIndex++)
      //{
      //  byte hashValue = ctx.hash_state[byteIndex + 64];
      //  byte scratchValue = ctx.long_state[byteIndex];
      //  ctx.long_state[byteIndex] = (byte)(hashValue ^ scratchValue);
      //}

      for (int scratchIndex = 0; scratchIndex < numberOfScratchpadSegments; scratchIndex++)
      {
        for (int blockIndex = 0; blockIndex < 8; blockIndex++)
        {
          for (int byteIndex = 0; byteIndex < 16; byteIndex++)
          {
            ctx.blocks[blockIndex][byteIndex] ^=
              ctx.scratchpad[byteIndex + scratchIndex * 128 + blockIndex * ctx.blocks[blockIndex].Length];
          }
        }

        EncryptBlocks();
      }


      for (int blockIndex = 0; blockIndex < 8; blockIndex++)
      {
        for (int byteIndex = 0; byteIndex < 16; byteIndex++)
        {
          ctx.keccakHash[64 + byteIndex + blockIndex * 16] = ctx.blocks[blockIndex][byteIndex];
        }
      }

    }

    public void ProcessStep13()
    {
      ulong[] tempLong = new ulong[sizeOfKeccakHash / sizeof(ulong) / sizeof(byte)];
      for (int i = 0; i < tempLong.Length; i++)
      {
        tempLong[i] = BitConverter.ToUInt64(ctx.keccakHash, i * sizeof(ulong) / sizeof(byte));
      }

      KeccakDigest.keccakf(tempLong, KeccakDigest.KECCAK_ROUNDS);

      for (int longIndex = 0; longIndex < tempLong.Length; longIndex++)
      {
        byte[] longData = BitConverter.GetBytes(tempLong[longIndex]);
        for (int byteIndex = 0; byteIndex < 8; byteIndex++)
        {
          ctx.keccakHash[longIndex * sizeof(ulong) + byteIndex] = longData[byteIndex];
        }
      }
    }

    
    public void Step15_FinalHash()
    {
      Digest hash;
      switch (ctx.keccakHash[0] & 3)
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

      hash.update(ctx.keccakHash);
      hash.digest(ctx.bResult, 0, 32);
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

    void ExtractBlocksFromHash()
    {
      for (int blockIndex = 0; blockIndex < 8; blockIndex++)
      {
        for (int byteIndex = 0; byteIndex < 16; byteIndex++)
        {
          ctx.blocks[blockIndex][byteIndex] = ctx.keccakHash[64 + blockIndex * 16 + byteIndex];
        }
      }
    }

    void EncryptBlocks()
    {
      for (int blockIndex = 0; blockIndex < 8; blockIndex++)
      {
        ctx.aes.ProcessBlock(ctx.blocks[blockIndex], 0, ctx.blocks[blockIndex], 0);
      }
    }

    void ExtractAndInitAesKey(
      bool useFirstSegmentVsSecond)
    {
      for (int i = 0; i < sizeOfKey; i++)
      {
        int index = i;
        if (useFirstSegmentVsSecond == false)
        {
          index += sizeOfKey;
        }
        ctx.key[i] = ctx.keccakHash[index];
      }

      ctx.aes.Init(ctx.key);
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