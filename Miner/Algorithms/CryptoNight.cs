using System;
using HD;
using System.Diagnostics;
using DotNetStratumMiner;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;

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
      Reg128
      System.Numerics.BigInteger aoeu = new System.Numerics.BigInteger();
      aoeu.

      // Fail
      //HD.AesEngine aes = new HD.AesEngine();
      //byte[] key = new byte[32];
      //for (int i = 0; i < key.Length; i++)
      //{
      //  key[i] = ctx.hash_state[i];
      //}
      //aes.Init(true, new KeyParameter(key));
      //// 64..191
      //byte[] eInfo = new byte[128];
      //aes.ProcessBlock(ctx.hash_state, 64, eInfo, 0);
      
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