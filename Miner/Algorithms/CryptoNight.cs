using System;
using HD;
using System.Diagnostics;
using DotNetStratumMiner;
using Org.BouncyCastle.Crypto.Engines;
using BigMath;

namespace HD
{
  public class cryptonight_ctx
  {
    public readonly byte[] hash_state = new byte[224]; // Need only 200, explicit align  (TODO reduce to 200)
    public readonly byte[] long_state = new byte[2097152]; // scratchpad
    public readonly byte[] ctx_info = new byte[24]; //Use some of the extra memory for flags
  }

  public class job_result
  {
    public byte[] bResult;
    public string sJobID;
    public uint iNonce;

    public job_result(
      string sJobID)
    {
      var bResult = new byte[32]; // TODO where does this come from
      Debug.Assert(bResult.Length == 32);

      this.bResult = bResult;
      this.sJobID = sJobID;
    }
  };

  public class CryptoNight
  {
    const int iThreadCount = 1;
    const int iThreadNo = 0;
    const int iResumeCnt = 0; // What's this for?

    public byte[] bWorkBlob = new byte[112];
    public job_result result;
    public cryptonight_ctx ctx;

    public int iWorkSize;
    public ulong iJobDiff;
    public ulong iTarget;
    public uint iCount;

    public byte[] a;
    public byte[] b;

    public byte[][] blocks;
    byte[] key;

    public AesEngine aes;

    public ulong piHashVal
    {
      get
      {
        return BitConverter.ToUInt64(result.bResult, 24);
      }
    }
    public uint piNonce
    {
      get
      {
        return BitConverter.ToUInt32(bWorkBlob, 39);
      }
      set
      {
        byte[] data = BitConverter.GetBytes(value);
        bWorkBlob[39] = data[0];
        bWorkBlob[40] = data[1];
        bWorkBlob[41] = data[2];
        bWorkBlob[42] = data[3];
      }
    }

    public void Process(NewJob newJob)
    {
      byte[] databyte = Utilities.HexStringToByteArray(newJob.Result.Job.Blob);
      iWorkSize = databyte.Length;
      byte[] targetbyte = Utilities.HexStringToByteArray(newJob.Result.Job.Target);
      ctx = new cryptonight_ctx();


      // 76 byte array
      //byte[] blob = StringToByteArray(newJob.Result.Job.Blob);
      for (int i = 0; i < databyte.Length; i++)
      {
        bWorkBlob[i] = databyte[i];
      }



      iTarget = t32_to_t64(hex2bin(newJob.Result.Job.Target, 8));
      iJobDiff = t64_to_diff(iTarget);

      iCount = 0;
      result = new job_result(
        newJob.Result.Job.Job_Id);
      result.iNonce = calc_nicehash_nonce(piNonce, iResumeCnt);
    }

    public void ProcessStep2()
    {
      piNonce = ++result.iNonce;
    }

    public void ProcessStep3()
    {
      KeccakDigest.keccak(bWorkBlob, iWorkSize, ctx.hash_state, 200);
    }

    public void ProcessStep4()
    {
      key = new byte[32];
      for (int i = 0; i < key.Length; i++)
      {
        key[i] = ctx.hash_state[i];
      }


      aes = new AesEngine();
      aes.Init(key);

      // TODO unit test for the key before we go on.

    }

    public void ProcessStep5()
    {
      blocks = new byte[8][]; // 8 blocks
      for (int blockIndex = 0; blockIndex < blocks.Length; blockIndex++)
      {
        blocks[blockIndex] = new byte[16]; // 16 bytes per block
        for (int byteIndex = 0; byteIndex < 16; byteIndex++)
        {
          blocks[blockIndex][byteIndex] = ctx.hash_state[64 + blockIndex * 16 + byteIndex];
        }
      }
    }

    /// <summary>
    /// One AES round per block
    /// </summary>
    public void ProcessStep6()
    {
      for (int blockIndex = 0; blockIndex < blocks.Length; blockIndex++)
      {
        aes.ProcessBlock(blocks[blockIndex], 0, blocks[blockIndex], 0);
      }
    }

    /// <summary>
    /// Copy blocks into the scratchpad
    /// </summary>
    public void ProcessStep7()
    {
      for (int blockIndex = 0; blockIndex < blocks.Length; blockIndex++)
      {
        byte[] block = blocks[blockIndex];
        for (int byteIndex = 0; byteIndex < block.Length; byteIndex++)
        {
          ctx.long_state[blockIndex * block.Length + byteIndex] = block[byteIndex];
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
        for (int blockIndex = 0; blockIndex < blocks.Length; blockIndex++)
        {
          aes.ProcessBlock(blocks[blockIndex], 0, blocks[blockIndex], 0);
        }

        for (int blockIndex = 0; blockIndex < blocks.Length; blockIndex++)
        {
          byte[] block = blocks[blockIndex];
          for (int byteIndex = 0; byteIndex < block.Length; byteIndex++)
          {
            ctx.long_state[scratchIndex * blocks.Length * block.Length + blockIndex * block.Length + byteIndex] = block[byteIndex];
          }
        }
      }
    }

    public void ProcessStep9()
    {
      a = new byte[16];
      b = new byte[16];
      for (int i = 0; i < key.Length; i++)
      {
        if (i < 16)
        {
          a[i] = (byte)(key[i] ^ ctx.hash_state[32 + i]);
        }
        else
        {
          b[i - 16] = (byte)(key[i] ^ ctx.hash_state[32 + i]);
        }
      }

    }


    public void ProcessStep10()
    {
      ulong idx0 = BitConverter.ToUInt64(a, 0);

      // Optim - 90% time boundary
      for (int i = 0; i < 524288; i++)
      {
        // cx = scratchpad[scratchpad_address];
        byte[] cx = new byte[16];
        for (int tempIndex = 0; tempIndex < cx.Length; tempIndex++)
        {
          cx[tempIndex] = ctx.long_state[(int)(idx0 & 0x1FFFF0) + tempIndex];
        }

        // scratchpad_address = to_scratchpad_address(a)
        // scratchpad[scratchpad_address] = aes_round(scratchpad[scratchpad_address], a)

        uint[] key = new uint[4];
        for (int keyIndex = 0; keyIndex < key.Length; keyIndex++)
        {
          key[keyIndex] = BitConverter.ToUInt32(a, keyIndex * 4);
        }

        aes.DoRounds(cx, 0, key, cx, 0);

        // scratchpad[scratchpad_address] = b xor scratchpad[scratchpad_address]
        for (int bIndex = 0; bIndex < 16; bIndex++)
        {
          ctx.long_state[(int)(idx0 & 0x1FFFF0) + bIndex] ^= b[bIndex];
        }

        idx0 = BitConverter.ToUInt64(cx, 0);

        // b = temp;
        for (int bIndex = 0; bIndex < 16; bIndex++)
        {
          b[bIndex] = cx[bIndex];
        }

        // scratchpad_address = to_scratchpad_address(b)
        // a = 8byte_add(a, 8byte_mul(b, scratchpad[scratchpad_address]))
        ulong hi, lo, cl, ch;
        cl = BitConverter.ToUInt64(ctx.long_state, (int)(idx0 & 0x1FFFF0));
        ch = BitConverter.ToUInt64(ctx.long_state, (int)(idx0 & 0x1FFFF0) + sizeof(ulong));

        Int128 mul = new Int128(0, idx0) * new Int128(0, cl);
        Int128 aInt = new Int128(BitConverter.ToUInt64(a, 8), BitConverter.ToUInt64(a, 0));

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
            a[aIndex] = mulHigh[aIndex - 8];
          }
          else
          {
            a[aIndex] = mulLow[aIndex];
          }
        }

        // temp = a;
        // a = a xor scratchpad[scratchpad_address]
        // scratchpad[scratchpad_address] = temp

        if(ctx.long_state[0] != 248)
        {
          Console.WriteLine();
        }

        byte[] temp = new byte[16];
        for (int aIndex = 0; aIndex < 16; aIndex++)
        {
          temp[aIndex] = ctx.long_state[(int)(idx0 & 0x1FFFF0) + aIndex];
        }

        for (int aIndex = 0; aIndex < 16; aIndex++)
        {
          ctx.long_state[(int)(idx0 & 0x1FFFF0) + aIndex] = a[aIndex];
          a[aIndex] ^= temp[aIndex];
        }

        // cache a for next loop
        idx0 = BitConverter.ToUInt64(a, 0);
      }



      Console.WriteLine();
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

    static ulong t64_to_diff(ulong t)
    {
      return 0xFFFFFFFFFFFFFFFF / t;
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