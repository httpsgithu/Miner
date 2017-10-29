using Microsoft.VisualStudio.TestTools.UnitTesting;
using HD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using SecurityDriven.Inferno.Cipher;
using System.Runtime.InteropServices;

namespace HD.Tests
{

  // Goal: find a faster way to replicate these two use cases:

  // 1) Process one Aes Round using the 16-byte key provided:
  // AesEngine.Encrypt(inputAndOutput, key);
  // both params are 16 bytes.  Goal is to produce the same inputAndOutput after.

  // 2) Process 10 Aes Rounds using 10 16-byte round keys generated from the 32-byte key provided:
  // AesEngine engine = new AesEngine();
  // engine.Init(key); where key is 32 bytes.  This produces 10 round keys.
  // engine.ProcessBlock(inputAndOutput);
  // Goal is to produce the same inputAndOutput after.



  [TestClass()]
  public class AesEngineTests
  {




    [DllImport("Bcrypt.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern uint BCryptOpenAlgorithmProvider(
            [In] [Out] ref IntPtr phAlgorithm,
            [In] String pszAlgId,
            [In] String pszImplementation,
            [In] int dwFlags
        );

    [DllImport("Bcrypt.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern uint BCryptGetProperty(
                [In] IntPtr hObject,
                [In] String pszProperty,
                [Out] byte[] pbOutput,
                [In] int cbOutput,
                [In] [Out] ref int pcbResult,
                [In] int dwFlags);

    [DllImport("Bcrypt.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern uint BCryptSetProperty(
                [In][Out] IntPtr hObject,
                [In] String pszProperty,
                [In] byte[] pbInput,
                [In] int cbInput,
                [In] int dwFlags);

    [DllImport("Bcrypt.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern uint BCryptGenerateSymmetricKey(
               [In]  IntPtr hAlgorithm,
               [In] [Out] ref IntPtr phKey,
               [Out] byte[] pbKeyObject,
               [In] int cbKeyObject,
               [In] byte[] pbSecret,
               [In] int cbSecret,
               [In] int dwFlags
           );

    [DllImport("Bcrypt.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern uint BCryptEncrypt(
                [In][Out] IntPtr hKey,
                [In] byte[] pbInput,
                [In] int cbInput,
                [In] IntPtr pPaddingInfo,
                [In] byte[] pbIV,
                [In] int cbIV,
                [Out] byte[] pbOutput,
                [In] int cbOutput,
                [In] [Out] ref int pcbResult,
                [In] int dwFlags
            );

    [DllImport("Bcrypt.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern uint BCryptDecrypt(
                [In][Out] IntPtr hKey,
                [In] byte[] pbInput,
                [In] int cbInput,
                [In] IntPtr pPaddingInfo,
                [In] byte[] pbIV,
                [In] int cbIV,
                [Out] byte[] pbOutput,
                [In] int cbOutput,
                [In] [Out] ref int pcbResult,
                [In] int dwFlags
            );

    [DllImport("Bcrypt.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern uint BCryptCloseAlgorithmProvider(
                [In] IntPtr phAlgorithm,
                [In] int dwFlags
            );


    [TestMethod]
    public unsafe void TestBcrypt()
    {
      byte[] pbKey = new byte[]
      {
        0,0,0,0,
        0,0,0,0,
        0,0,0,0,
        0,0,0,0,
        0,0,0,0,
        0,0,0,0,
        0,0,0,0,
        0,0,0,0,
      };




      //Initialize Status
      uint status = 0;

      //Initialize AlgHandle
      IntPtr pAlgHandle = IntPtr.Zero;

      //Open Algorithm Provider
      status = BCryptOpenAlgorithmProvider(
             ref pAlgHandle,
             "AES",
             "Microsoft Primitive Provider",
             0);

      //Allocate DWORD for ObjectLength
      byte[] pbObjectLength = new byte[4];

      //Initialize ObjectLength Byte Count
      int pcbObjectLength = 0;



      //Allocate DWORD for ObjectLength
      byte[] pbBlockLength = new byte[4];
      pbBlockLength[0] = 16;

      var paramData = Encoding.Unicode.GetBytes("ChainingModeN/A");
      status = BCryptSetProperty(
        pAlgHandle,
        "ChainingMode",
        paramData,
        paramData.Length,
        0);


      status = BCryptGetProperty(
             pAlgHandle,
             "ObjectLength",
             pbObjectLength,
             pbObjectLength.Length,
             ref pcbObjectLength,
             0);

      //Initialize KeyHandle
      IntPtr pKeyHandle = IntPtr.Zero;

      //Initialize Key Object Size with ObjectLength
      int keyObjectSize = pbObjectLength[3] << 24 | pbObjectLength[2] << 16 | pbObjectLength[1] << 8 | pbObjectLength[0];

      //Allocate KeyObject With Key Object Size
      byte[] pbKeyObject = new byte[keyObjectSize];

      //Generate Symmetric Key Object
      status = BCryptGenerateSymmetricKey(
             pAlgHandle,
             ref pKeyHandle,
             pbKeyObject,
             keyObjectSize,
             pbKey,
             pbKey.Length,
             0);



      //Initialize Data To Encrypt
      byte[] pbData = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

      //Initialize Initialization Vector
      byte[] pbIV = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
      //Initialize PaddingInfo
      IntPtr pPaddingInfo = IntPtr.Zero;

      //Initialize Cipher Text Byte Count
      int pcbCipherText = 0;

      //Get Cipher Text Byte Count
      status = BCryptEncrypt(
             pKeyHandle,
             pbData,
             pbData.Length,
             IntPtr.Zero,
             null,
             0,
             null,
             0,
             ref pcbCipherText,
             0);

      //Allocate Cipher Text Buffer
      byte[] pbCipherText = new byte[pcbCipherText];

      //Encrypt The Data
      status = BCryptEncrypt(
             pKeyHandle,
             pbData,
             pbData.Length,
             pPaddingInfo,
             pbIV,
             pbIV.Length,
             pbCipherText,
             pcbCipherText,
             ref pcbCipherText,
             0);






      fixed (byte* keyAsByteData = pbKey)
      {
        uint* keyAsUint = (uint*)keyAsByteData;
        ulong* keyAsUlong = (ulong*)keyAsByteData;
        byte[] tempBlock = new byte[CryptoNight.sizeOfBlock];
        byte[] tempBlock2 = new byte[CryptoNight.sizeOfBlock];
        fixed (byte* tempBlockBytes = tempBlock)
        {
          uint* tempBlockAsUint = (uint*)tempBlockBytes;
          ulong* tempBlockAsUlong = (ulong*)tempBlockBytes;

          AesEngine.Encrypt(tempBlockAsUint, keyAsUint);
        }
      }

      AesEngine engine = new AesEngine();
      engine.Init(pbKey);
      fixed(byte* data = new byte[16])
      {
        uint* uintData = (uint*)data;

        engine.ProcessBlock(uintData);
      }
      

    }




































































































    [TestMethod()]
    public unsafe void ProcessBlockTest10rounds()
    {
      AesEngine aes = new AesEngine();
      byte[] keyAsBytes = new byte[CryptoNight.sizeOfKey];
      aes.Init(keyAsBytes);

      byte* blockData = stackalloc byte[CryptoNight.numberOfBlocks * CryptoNight.sizeOfBlock];
      ulong* blockDataUlong = (ulong*)blockData;
      uint* blockDataUint = (uint*)blockData;


      for (int blockIndex = 0; blockIndex < CryptoNight.numberOfBlocks; blockIndex++)
      {
        aes.ProcessBlock(blockDataUint + blockIndex * CryptoNight.sizeOfBlock / sizeof(uint));
      }

      // Given 0s for key and input blocks, blockdata is the expected out

      for (int mode = 0; mode < Enum.GetValues(typeof(CipherMode)).Length; mode++)
      {
        for (int paddingMode = 0; paddingMode < Enum.GetValues(typeof(PaddingMode)).Length; paddingMode++)
        {
          using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
          {
            //aesProvider.IV = new byte[16];
            try
            {
              aesProvider.Padding = (PaddingMode)paddingMode;
            }
            catch
            {
              continue;
            }

            try
            {
              aesProvider.Mode = (CipherMode)mode;
            }
            catch
            {
              continue;
            }

            aesProvider.Key = keyAsBytes;
            try
            {
              var encryptor = aesProvider.CreateEncryptor();
              byte[] results = encryptor.TransformFinalBlock(new byte[CryptoNight.sizeOfBlock], 0, CryptoNight.sizeOfBlock);
              Assert.IsFalse(results[0] == blockData[0]);
            }
            catch
            {
              continue;
            }

          }
        }
      }
      return;




      fixed (byte* keyAsByteData = keyAsBytes)
      {
        uint* keyAsUint = (uint*)keyAsByteData;
        ulong* keyAsUlong = (ulong*)keyAsByteData;
        byte[] tempBlock = new byte[CryptoNight.sizeOfBlock];
        byte[] tempBlock2 = new byte[CryptoNight.sizeOfBlock];
        fixed (byte* tempBlockBytes = tempBlock)
        {
          uint* tempBlockAsUint = (uint*)tempBlockBytes;
          ulong* tempBlockAsUlong = (ulong*)tempBlockBytes;

          AesEngine.Encrypt(tempBlockAsUint, keyAsUint);

          for (int i = 0; i < Enum.GetValues(typeof(CipherMode)).Length; i++)
          {
            for (int paddingMode = 0; paddingMode < Enum.GetValues(typeof(PaddingMode)).Length; paddingMode++)
            {

              using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
              {

                //aesProvider.FeedbackSize = 0;
                aesProvider.IV = new byte[16];
                try
                {
                  aesProvider.Padding = (PaddingMode)paddingMode;
                }
                catch
                {
                  continue;
                }


                //aesProvider.BlockSize = CryptoNight.sizeOfBlock * 8;
                aesProvider.Key = keyAsBytes;
                try
                {
                  aesProvider.Mode = (CipherMode)i;
                }
                catch
                {
                  continue;
                }
                try
                {
                  ICryptoTransform aesEncrypter = aesProvider.CreateEncryptor();
                  byte[] tempBlock3 = new byte[CryptoNight.sizeOfBlock];

                  //aesEncrypter.TransformBlock();





                  AesCtrCryptoTransform aesCtrCryptoTransform = new AesCtrCryptoTransform(keyAsBytes, new ArraySegment<byte>(new byte[16]));
                  aesCtrCryptoTransform.TransformBlock(tempBlock2, 0, CryptoNight.sizeOfBlock, tempBlock3, 0);





                  bool passed = true;
                  for (int blockIndex = 0; blockIndex < CryptoNight.sizeOfBlock; blockIndex++)
                  {
                    if (tempBlock[blockIndex] != tempBlock3[blockIndex])
                    {
                      passed = false;
                    }
                  }

                  Assert.IsTrue(passed == false);
                }
                catch
                {
                  continue;
                }
              }
            }
          }
        }
      }
    }


    [TestMethod()]
    public unsafe void ProcessBlockTest()
    {
      AesEngine aes = new AesEngine();
      byte[] keyAsBytes = new byte[AesEngine.numberOfUintsPerKey * sizeof(uint)];
      fixed (byte* keyAsByteData = keyAsBytes)
      {
        uint* keyAsUint = (uint*)keyAsByteData;
        ulong* keyAsUlong = (ulong*)keyAsByteData;
        byte[] tempBlock = new byte[CryptoNight.sizeOfBlock];
        byte[] tempBlock2 = new byte[CryptoNight.sizeOfBlock];
        fixed (byte* tempBlockBytes = tempBlock)
        {
          uint* tempBlockAsUint = (uint*)tempBlockBytes;
          ulong* tempBlockAsUlong = (ulong*)tempBlockBytes;

          AesEngine.Encrypt(tempBlockAsUint, keyAsUint);

          for (int i = 0; i < Enum.GetValues(typeof(CipherMode)).Length; i++)
          {
            for (int paddingMode = 0; paddingMode < Enum.GetValues(typeof(PaddingMode)).Length; paddingMode++)
            {

              using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
              {

                //aesProvider.FeedbackSize = 0;
                aesProvider.IV = new byte[16];
                try
                {
                  aesProvider.Padding = (PaddingMode)paddingMode;
                }
                catch
                {
                  continue;
                }


                //aesProvider.BlockSize = CryptoNight.sizeOfBlock * 8;
                aesProvider.Key = keyAsBytes;
                try
                {
                  aesProvider.Mode = (CipherMode)i;
                }
                catch
                {
                  continue;
                }
                try
                {
                  ICryptoTransform aesEncrypter = aesProvider.CreateEncryptor();
                  byte[] tempBlock3 = new byte[CryptoNight.sizeOfBlock];

                  //aesEncrypter.TransformBlock();





                  AesCtrCryptoTransform aesCtrCryptoTransform = new AesCtrCryptoTransform(keyAsBytes, new ArraySegment<byte>(new byte[16]));
                  aesCtrCryptoTransform.TransformBlock(tempBlock2, 0, CryptoNight.sizeOfBlock, tempBlock3, 0);





                  bool passed = true;
                  for (int blockIndex = 0; blockIndex < CryptoNight.sizeOfBlock; blockIndex++)
                  {
                    if (tempBlock[blockIndex] != tempBlock3[blockIndex])
                    {
                      passed = false;
                    }
                  }

                  Assert.IsTrue(passed == false);
                }
                catch
                {
                  continue;
                }
              }
            }
          }
        }
      }
    }
  }
}