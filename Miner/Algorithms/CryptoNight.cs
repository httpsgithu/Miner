using System;
using System.Diagnostics;
using BigMath;
using Newtonsoft.Json;
using CryptoHash;
using HD.Algorithms;

namespace HD
{
  public class CryptoNight
  {
    #region Data
    // TODO this is thread stuff
    const int iThreadCount = 1;
    const int iThreadNo = 0;
    const int iResumeCnt = 0; 

    const int iWorkSize = 76;

    public readonly byte[] bWorkBlob = new byte[76];

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
        return BitConverter.ToUInt32(bWorkBlob, 39);
      }
      set
      {
        value.GetBytes(bWorkBlob, 39);
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
    #endregion

    public void Step2_IncrementNonce()
    {
      piNonce++;
    }

    public void Step3_KeccakHash()
    {
      KeccakDigest.keccak(bWorkBlob, iWorkSize, ctx.keccakHash, 200);
    }

    public void Step4_InitAesKey()
    {
      // Extract key
      for (int i = 0; i < ctx.key.Length; i++)
      {
        ctx.key[i] = ctx.keccakHash[i];
      }

      ctx.aes.Init(ctx.key);
    }

    public void Step5_ExtractBlocksToEncrypt()
    {
      ExtractBlocksFromHash();
    }

    /// <summary>
    /// One AES round per block
    /// </summary>
    //public void Step6_EncryptBlocks()
    //{
    //  EncryptBlocks();
    //}

    ///// <summary>
    ///// Copy blocks into the scratchpad
    ///// </summary>
    //public void Step7_CopyBlocksToScratchpad()
    //{
    //  for (int blockIndex = 0; blockIndex < ctx.blocks.Length; blockIndex++)
    //  {
    //    byte[] block = ctx.blocks[blockIndex];
    //    for (int byteIndex = 0; byteIndex < block.Length; byteIndex++)
    //    {
    //      ctx.scratchpad[blockIndex * block.Length + byteIndex] = block[byteIndex];
    //    }
    //  }
    //}

    /// <summary>
    /// Complete the scratchpad
    /// </summary>
    public void Step7_LoopScratchpad()
    {
      for (int scratchIndex = 0; scratchIndex < 16384; scratchIndex++)
      {
        for (int blockIndex = 0; blockIndex < ctx.blocks.Length; blockIndex++)
        {
          ctx.aes.ProcessBlock(ctx.blocks[blockIndex], 0, ctx.blocks[blockIndex], 0);
        }

        for (int blockIndex = 0; blockIndex < ctx.blocks.Length; blockIndex++)
        {
          byte[] block = ctx.blocks[blockIndex];
          for (int byteIndex = 0; byteIndex < block.Length; byteIndex++)
          {
            ctx.scratchpad[scratchIndex * ctx.blocks.Length * block.Length + blockIndex * block.Length + byteIndex] = block[byteIndex];
          }
        }
      }
    }

    public void ProcessStep9()
    {
      for (int i = 0; i < ctx.key.Length; i++)
      {
        if (i < 16)
        {
          ctx.memoryHardLoop_A[i] = (byte)(ctx.key[i] ^ ctx.keccakHash[32 + i]);
        }
        else
        {
          ctx.memoryHardLoop_B[i - 16] = (byte)(ctx.key[i] ^ ctx.keccakHash[32 + i]);
        }
      }
    }

    public void ProcessStep10()
    {
      ulong idx0 = BitConverter.ToUInt64(ctx.memoryHardLoop_A, 0);

      for (int i = 0; i < 524288; i++)
      {
        byte[] cx = new byte[16];
        for (int tempIndex = 0; tempIndex < cx.Length; tempIndex++)
        {
          cx[tempIndex] = ctx.scratchpad[(int)(idx0 & 0x1FFFF0) + tempIndex];
        }

        uint[] key = new uint[4];
        for (int keyIndex = 0; keyIndex < key.Length; keyIndex++)
        {
          key[keyIndex] = BitConverter.ToUInt32(ctx.memoryHardLoop_A, keyIndex * 4);
        }

        ctx.aes.DoRounds(cx, 0, key, cx, 0);

        for (int bIndex = 0; bIndex < 16; bIndex++)
        {
          ctx.scratchpad[(int)(idx0 & 0x1FFFF0) + bIndex] = (byte)(cx[bIndex] ^ ctx.memoryHardLoop_B[bIndex]);
        }

        idx0 = BitConverter.ToUInt64(cx, 0);

        for (int bIndex = 0; bIndex < 16; bIndex++)
        {
          ctx.memoryHardLoop_B[bIndex] = cx[bIndex];
        }

        ulong cl, ch;
        cl = BitConverter.ToUInt64(ctx.scratchpad, (int)(idx0 & 0x1FFFF0));
        ch = BitConverter.ToUInt64(ctx.scratchpad, (int)(idx0 & 0x1FFFF0) + sizeof(ulong));

        Int128 mul = new Int128(0, idx0) * new Int128(0, cl);
        Int128 aInt = new Int128(BitConverter.ToUInt64(ctx.memoryHardLoop_A, 8), BitConverter.ToUInt64(ctx.memoryHardLoop_A, 0));

        unchecked
        {
          aInt = new Int128(mul.Low + aInt.High, mul.High + aInt.Low);
        }

        byte[] mulHigh = BitConverter.GetBytes(aInt.High);
        byte[] mulLow = BitConverter.GetBytes(aInt.Low);

        for (int aIndex = 0; aIndex < 16; aIndex++)
        {
          if (aIndex >= 8)
          {
            ctx.memoryHardLoop_A[aIndex] = mulHigh[aIndex - 8];
          }
          else
          {
            ctx.memoryHardLoop_A[aIndex] = mulLow[aIndex];
          }
        }

        byte[] temp = new byte[16]; // TODO
        for (int aIndex = 0; aIndex < 16; aIndex++)
        {
          temp[aIndex] = ctx.scratchpad[(int)(idx0 & 0x1FFFF0) + aIndex];
        }

        for (int aIndex = 0; aIndex < 16; aIndex++)
        {
          ctx.scratchpad[(int)(idx0 & 0x1FFFF0) + aIndex] = ctx.memoryHardLoop_A[aIndex];
          ctx.memoryHardLoop_A[aIndex] ^= temp[aIndex];
        }

        idx0 = BitConverter.ToUInt64(ctx.memoryHardLoop_A, 0);
      }
    }

    public void ProcessStep11()
    {
      // Should we not be sharing this key?
      for (int i = 0; i < ctx.key.Length; i++)
      {
        ctx.key[i] = ctx.keccakHash[i + 32];
      }

      ctx.aes.Init(ctx.key);
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

      for (int scratchIndex = 0; scratchIndex < 16384; scratchIndex++)
      {
        for (int blockIndex = 0; blockIndex < ctx.blocks.Length; blockIndex++)
        {
          for (int byteIndex = 0; byteIndex < 16; byteIndex++)
          {
            ctx.blocks[blockIndex][byteIndex] ^=
              ctx.scratchpad[byteIndex + scratchIndex * 128 + blockIndex * ctx.blocks[blockIndex].Length];
          }
        }

        EncryptBlocks();
      }


      for (int blockIndex = 0; blockIndex < ctx.blocks.Length; blockIndex++)
      {
        for (int byteIndex = 0; byteIndex < 16; byteIndex++)
        {
          ctx.keccakHash[64 + byteIndex + blockIndex * 16] = ctx.blocks[blockIndex][byteIndex];
        }
      }

    }

    public void ProcessStep13()
    {
      ulong[] tempLong = new ulong[200 / sizeof(ulong) / sizeof(byte)];
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


    public int hashID;

    public void ProcessStep14()
    {
      hashID = ctx.keccakHash[0] & 0b0011;
    }

    public void Step15_FinalHash()
    {
      Digest hash;
      switch (hashID)
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
    void ExtractBlocksFromHash()
    {
      for (int blockIndex = 0; blockIndex < ctx.blocks.Length; blockIndex++)
      {
        for (int byteIndex = 0; byteIndex < 16; byteIndex++)
        {
          ctx.blocks[blockIndex][byteIndex] = ctx.keccakHash[64 + blockIndex * 16 + byteIndex];
        }
      }
    }

    void EncryptBlocks()
    {
      for (int blockIndex = 0; blockIndex < ctx.blocks.Length; blockIndex++)
      {
        ctx.aes.ProcessBlock(ctx.blocks[blockIndex], 0, ctx.blocks[blockIndex], 0);
      }
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