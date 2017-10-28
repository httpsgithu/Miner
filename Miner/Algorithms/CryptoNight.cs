using System;
using System.Diagnostics;
using Org.BouncyCastle.Crypto.Engines;
using BigMath;
using Newtonsoft.Json;
using CryptoHash;
using HD.Algorithms;

namespace HD
{
  /// <summary>
  /// I believe this is the only information which differs per thread.
  /// 
  /// TODO perf: reuse the arrays
  /// </summary>
  public class cryptonight_ctx
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

    public cryptonight_ctx()
    {
      for (int i = 0; i < blocks.Length; i++)
      {
        blocks[i] = new byte[16];
      }
    }
  }

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
    public readonly cryptonight_ctx ctx = new cryptonight_ctx();

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
      iTarget = t32_to_t64(hex2bin(target, 8));
      InitNonce(nonceOverride);
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


    public void ProcessStep2()
    {
      piNonce++;
    }

    public void ProcessStep3()
    {
      KeccakDigest.keccak(bWorkBlob, iWorkSize, ctx.keccakHash, 200);
    }

    public string GetResultJson()
    {
      NiceHashResultJson json = new NiceHashResultJson(requestId, jobId, piNonce, ctx.bResult);
      return JsonConvert.SerializeObject(json);
    }

    public void ProcessStep4()
    {
      for (int i = 0; i < ctx.key.Length; i++)
      {
        ctx.key[i] = ctx.keccakHash[i];
      }

      ctx.aes.Init(ctx.key);
    }

    public void ProcessStep5()
    {
      ExtractBlocksFromHash();
    }

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

    /// <summary>
    /// One AES round per block
    /// </summary>
    public void ProcessStep6()
    {
      EncryptBlocks();
    }

    void EncryptBlocks()
    {
      for (int blockIndex = 0; blockIndex < ctx.blocks.Length; blockIndex++)
      {
        ctx.aes.ProcessBlock(ctx.blocks[blockIndex], 0, ctx.blocks[blockIndex], 0);
      }
    }

    /// <summary>
    /// Copy blocks into the scratchpad
    /// </summary>
    public void ProcessStep7()
    {
      for (int blockIndex = 0; blockIndex < ctx.blocks.Length; blockIndex++)
      {
        byte[] block = ctx.blocks[blockIndex];
        for (int byteIndex = 0; byteIndex < block.Length; byteIndex++)
        {
          ctx.scratchpad[blockIndex * block.Length + byteIndex] = block[byteIndex];
        }
      }
    }

    /// <summary>
    /// Complete the scratchpad
    /// </summary>
    public void ProcessStep8()
    {
      for (int scratchIndex = 1; scratchIndex < 16384; scratchIndex++)
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

    uint hex2bin(string input, uint len)
    {
      bool error = false;
      byte[] output = new byte[len / 2];
      for (int i = 0; i < len; i += 2)
      {
        output[i / 2] = (byte)((hf_hex2bin((byte)input[i], ref error) << 4) | hf_hex2bin((byte)input[i + 1], ref error));
        if (error)
        {
          throw new Exception(); // error handling...
        }
      }
      return BitConverter.ToUInt32(output, 0);
    }

    public int hashID;

    public void ProcessStep14()
    {
      hashID = ctx.keccakHash[0] & 0b0011;
    }
    public void ProcessStep15()
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

    byte hf_hex2bin(byte c, ref bool err)
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

      err = true;
      return 0;
    }

    uint calc_nicehash_nonce(
          uint start,
          uint resume)
    {
      return start | (resume * iThreadCount + iThreadNo) << 18;
    }
    
    public static byte[] StringToByteArray(String hex)
    {
      int NumberChars = hex.Length;
      byte[] bytes = new byte[NumberChars / 2];
      for (int i = 0; i < NumberChars; i += 2)
        bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
      return bytes;
    }

    static ulong t32_to_t64(uint t)
    {
      return 0xFFFFFFFFFFFFFFFF / (0xFFFFFFFF / ((ulong)t));
    }
  }

}