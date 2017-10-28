using Microsoft.VisualStudio.TestTools.UnitTesting;
using HD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HD.Tests
{
  [TestClass()]
  public class CryptoNightTests
  {

    static uint Shift(
      uint r,
      int shift)
    {
      return (r >> shift) | (r << (32 - shift));
    }

    [TestMethod()]
    public void Hack()
    {

      uint[] T0 =
      {
            0xa56363c6, 0x847c7cf8, 0x997777ee, 0x8d7b7bf6, 0x0df2f2ff,
            0xbd6b6bd6, 0xb16f6fde, 0x54c5c591, 0x50303060, 0x03010102,
            0xa96767ce, 0x7d2b2b56, 0x19fefee7, 0x62d7d7b5, 0xe6abab4d,
            0x9a7676ec, 0x45caca8f, 0x9d82821f, 0x40c9c989, 0x877d7dfa,
            0x15fafaef, 0xeb5959b2, 0xc947478e, 0x0bf0f0fb, 0xecadad41,
            0x67d4d4b3, 0xfda2a25f, 0xeaafaf45, 0xbf9c9c23, 0xf7a4a453,
            0x967272e4, 0x5bc0c09b, 0xc2b7b775, 0x1cfdfde1, 0xae93933d,
            0x6a26264c, 0x5a36366c, 0x413f3f7e, 0x02f7f7f5, 0x4fcccc83,
            0x5c343468, 0xf4a5a551, 0x34e5e5d1, 0x08f1f1f9, 0x937171e2,
            0x73d8d8ab, 0x53313162, 0x3f15152a, 0x0c040408, 0x52c7c795,
            0x65232346, 0x5ec3c39d, 0x28181830, 0xa1969637, 0x0f05050a,
            0xb59a9a2f, 0x0907070e, 0x36121224, 0x9b80801b, 0x3de2e2df,
            0x26ebebcd, 0x6927274e, 0xcdb2b27f, 0x9f7575ea, 0x1b090912,
            0x9e83831d, 0x742c2c58, 0x2e1a1a34, 0x2d1b1b36, 0xb26e6edc,
            0xee5a5ab4, 0xfba0a05b, 0xf65252a4, 0x4d3b3b76, 0x61d6d6b7,
            0xceb3b37d, 0x7b292952, 0x3ee3e3dd, 0x712f2f5e, 0x97848413,
            0xf55353a6, 0x68d1d1b9, 0x00000000, 0x2cededc1, 0x60202040,
            0x1ffcfce3, 0xc8b1b179, 0xed5b5bb6, 0xbe6a6ad4, 0x46cbcb8d,
            0xd9bebe67, 0x4b393972, 0xde4a4a94, 0xd44c4c98, 0xe85858b0,
            0x4acfcf85, 0x6bd0d0bb, 0x2aefefc5, 0xe5aaaa4f, 0x16fbfbed,
            0xc5434386, 0xd74d4d9a, 0x55333366, 0x94858511, 0xcf45458a,
            0x10f9f9e9, 0x06020204, 0x817f7ffe, 0xf05050a0, 0x443c3c78,
            0xba9f9f25, 0xe3a8a84b, 0xf35151a2, 0xfea3a35d, 0xc0404080,
            0x8a8f8f05, 0xad92923f, 0xbc9d9d21, 0x48383870, 0x04f5f5f1,
            0xdfbcbc63, 0xc1b6b677, 0x75dadaaf, 0x63212142, 0x30101020,
            0x1affffe5, 0x0ef3f3fd, 0x6dd2d2bf, 0x4ccdcd81, 0x140c0c18,
            0x35131326, 0x2fececc3, 0xe15f5fbe, 0xa2979735, 0xcc444488,
            0x3917172e, 0x57c4c493, 0xf2a7a755, 0x827e7efc, 0x473d3d7a,
            0xac6464c8, 0xe75d5dba, 0x2b191932, 0x957373e6, 0xa06060c0,
            0x98818119, 0xd14f4f9e, 0x7fdcdca3, 0x66222244, 0x7e2a2a54,
            0xab90903b, 0x8388880b, 0xca46468c, 0x29eeeec7, 0xd3b8b86b,
            0x3c141428, 0x79dedea7, 0xe25e5ebc, 0x1d0b0b16, 0x76dbdbad,
            0x3be0e0db, 0x56323264, 0x4e3a3a74, 0x1e0a0a14, 0xdb494992,
            0x0a06060c, 0x6c242448, 0xe45c5cb8, 0x5dc2c29f, 0x6ed3d3bd,
            0xefacac43, 0xa66262c4, 0xa8919139, 0xa4959531, 0x37e4e4d3,
            0x8b7979f2, 0x32e7e7d5, 0x43c8c88b, 0x5937376e, 0xb76d6dda,
            0x8c8d8d01, 0x64d5d5b1, 0xd24e4e9c, 0xe0a9a949, 0xb46c6cd8,
            0xfa5656ac, 0x07f4f4f3, 0x25eaeacf, 0xaf6565ca, 0x8e7a7af4,
            0xe9aeae47, 0x18080810, 0xd5baba6f, 0x887878f0, 0x6f25254a,
            0x722e2e5c, 0x241c1c38, 0xf1a6a657, 0xc7b4b473, 0x51c6c697,
            0x23e8e8cb, 0x7cdddda1, 0x9c7474e8, 0x211f1f3e, 0xdd4b4b96,
            0xdcbdbd61, 0x868b8b0d, 0x858a8a0f, 0x907070e0, 0x423e3e7c,
            0xc4b5b571, 0xaa6666cc, 0xd8484890, 0x05030306, 0x01f6f6f7,
            0x120e0e1c, 0xa36161c2, 0x5f35356a, 0xf95757ae, 0xd0b9b969,
            0x91868617, 0x58c1c199, 0x271d1d3a, 0xb99e9e27, 0x38e1e1d9,
            0x13f8f8eb, 0xb398982b, 0x33111122, 0xbb6969d2, 0x70d9d9a9,
            0x898e8e07, 0xa7949433, 0xb69b9b2d, 0x221e1e3c, 0x92878715,
            0x20e9e9c9, 0x49cece87, 0xff5555aa, 0x78282850, 0x7adfdfa5,
            0x8f8c8c03, 0xf8a1a159, 0x80898909, 0x170d0d1a, 0xdabfbf65,
            0x31e6e6d7, 0xc6424284, 0xb86868d0, 0xc3414182, 0xb0999929,
            0x772d2d5a, 0x110f0f1e, 0xcbb0b07b, 0xfc5454a8, 0xd6bbbb6d,
            0x3a16162c
        };


      string t8, t16, t24;
      t8 = t16 = t24 = "";

      for (int i = 0; i < T0.Length; i++)
      {
        if (i > 0)
        {
          t8 += ", ";
          t16 += ", ";
          t24 += ", ";
        }

        t8 += "0x" + Shift(T0[i], 8).ToString("x");
        t16 += "0x" + Shift(T0[i], 16).ToString("x");
        t24 += "0x" + Shift(T0[i], 24).ToString("x");
      }
      Console.WriteLine();
    }


    /// <summary>
    /// piNonce == 3C0C01E3
    /// nonce in result == e3010c3c
    /// 
    /// 
    /// {"params":{"job_id":"0000002297b75770","blob":"0606fc97d2cf0546124d70ebc3a23268f0680de3b5e1fb4e9b3f51cb96f5dabfaf7e8ee025e5bd0000003ca5256c6cd1e24032196c96b92f756853714756778d9d84a6e0510340122bc85f06","target":"b7d10000"},"jsonrpc":"2.0","method":"job"}
    /// {"method":"submit","params":{"id":"658828527673356","job_id":"0000002297b75770","nonce":"e3010c3c","result":"c731f8e552f380a8063afe8cee7ff64ef9b18220ffca901de333571a20c50000"},"id":1}
    /// </summary>



    /// <summary>
    /// Job 0x0000004bcf7ff730 "0000002297ad5378"
    /// bResult 240,244 .. 0,0
    /// iTarget = 230584300921369
    /// hashval = 9560061706770
    /// 
    /// {"params":{"job_id":"0000002297ad5378","blob":"0606c090d2cf05dcc43f21fca8b65ef2741e7e0d6a9930ef4ef3d387d9a19b9b969ab8bfab1b87000000a85a6a72160e7768788421aeaffa1aa095e4d13c9b78786e1de866e4f6f135392609","target":"b7d10000"},"jsonrpc":"2.0","method":"job"}
    /// {"method":"submit","params":{"id":"083450145620356","job_id":"0000002297ad5378","nonce":"89010ca8","result":"f0f4feb27fd1365524753ad7aa7d754f673c647f10da5ff1120215e0b1080000"},"id":1}
    /// </summary>



    // 
    // 
    // {"id":1,"jsonrpc":"2.0","error":null,"result":{"status":"OK"}}


    // {"params":{"job_id":"000000228b2ccd9f","blob":"0606f78ec3cf0577b744fab48ba56e724c38276897d36f8f1908362d85d65318f6d3b33cd76a3c0000007ba0ed3fee4f999e53f17114931d55b95553df90f7c55472f8adbbb0eac3ba47f403","target":"b7d10000"},"jsonrpc":"2.0","method":"job"}
    // {"method":"submit","params":{"id":"421970933313569","job_id":"000000228b2ccd9f","nonce":"9207147b","result":"525adce6a66339b8bce448b3121d415fa6777c1ca986413bf109acf3f6b20000"},"id":1}
    // {"id":1,"jsonrpc":"2.0","error":null,"result":{"status":"OK"}}
    // {"method":"submit","params":{"id":"421970933313569","job_id":"000000228b2ccd9f","nonce":"2232087b","result":"eabc0546b73de5909f9941c67cdf4095e39f339802229468ea741ff3f9770000"},"id":1}
    // {"id":1,"jsonrpc":"2.0","error":null,"result":{"status":"OK"}}
    // {"method":"submit","params":{"id":"421970933313569","job_id":"000000228b2ccd9f","nonce":"fb33147b","result":"79a0cc26cdbf1958c6370bd48de024dd1c5cf9c789cc98bdf1a8cda5e8190000"},"id":1}
    // {"id":1,"jsonrpc":"2.0","error":null,"result":{"status":"OK"}}



    // 
    // 

    //      [TestMethod()]
    //      public void Hash1Test()
    //    {
    //      byte[] inputData = new byte[]
    //      {
    //        185,
    //46 ,
    //80 ,
    //127,
    //66 ,
    //30 ,
    //213,
    //234,
    //62 ,
    //254,
    //40 ,
    //120,
    //50 ,
    //0 ,
    //221,
    //151,
    //111,
    //214,
    //104,
    //93 ,
    //164,
    //5 ,
    //209,
    //124,
    //28 ,
    //177,
    //10 ,
    //8 ,
    //204,
    //241,
    //11 ,
    //11 ,
    //130,
    //102,
    //137,
    //80 ,
    //123,
    //17 ,
    //94 ,
    //127,
    //157,
    //45 ,
    //220,
    //244,
    //60 ,
    //96 ,
    //219,
    //231,
    //25 ,
    //218,
    //1 ,
    //171,
    //220,
    //138,
    //154,
    //15 ,
    //83 ,
    //75 ,
    //206,
    //230,
    //148,
    //68 ,
    //159,
    //88 ,
    //36 ,
    //28 ,
    //237,
    //210,
    //203,
    //216,
    //234,
    //98 ,
    //187,
    //111,
    //97 ,
    //186,
    //196,
    //103,
    //172,
    //232,
    //135,
    //205,
    //83 ,
    //253,
    //13 ,
    //110,
    //7 ,
    //82 ,
    //140,
    //168,
    //63 ,
    //172,
    //116,
    //131,
    //160,
    //240,
    //243,
    //17 ,
    //230,
    //202,
    //91 ,
    //168,
    //216,
    //146,
    //146,
    //113,
    //204,
    //87 ,
    //45 ,
    //198,
    //133,
    //244,
    //186,
    //111,
    //14 ,
    //183,
    //110,
    //34 ,
    //0 ,
    //85 ,
    //154,
    //105,
    //79 ,
    //204,
    //210,
    //3 ,
    //236,
    //33 ,
    //234,
    //137,
    //255,
    //241,
    //97 ,
    //87 ,
    //77 ,
    //214,
    //68 ,
    //25 ,
    //138,
    //150,
    //210,
    //14 ,
    //149,
    //152,
    //50 ,
    //41 ,
    //227,
    //191,
    //93 ,
    //65 ,
    //130,
    //121,
    //183,
    //95 ,
    //127,
    //48 ,
    //74 ,
    //243,
    //29 ,
    //184,
    //74 ,
    //251,
    //236,
    //191,
    //27 ,
    //240,
    //146,
    //164,
    //75 ,
    //179,
    //124,
    //165,
    //222,
    //84 ,
    //27 ,
    //87 ,
    //63 ,
    //160,
    //236,
    //167,
    //170,
    //244,
    //94 ,
    //58 ,
    //174,
    //4 ,
    //42 ,
    //200,
    //114,
    //245,
    //163,
    //74 ,
    //233,
    //18 ,
    //249,
    //248,
    //42 ,
    //220,
    //157,
    //204,
    //      };
    //      byte[] resultData = new byte[]
    //      {
    //        26  ,
    //174 ,
    //20  ,
    //169 ,
    //111 ,
    //153 ,
    //32  ,
    //22  ,
    //179 ,
    //199 ,
    //177 ,
    //192 ,
    //13  ,
    //99  ,
    //66  ,
    //73  ,
    //95  ,
    //187 ,
    //0   ,
    //59  ,
    //45  ,
    //91  ,
    //97  ,
    //149 ,
    //56  ,
    //196 ,
    //176 ,
    //239 ,
    //109 ,
    //85  ,
    //113 ,
    //247 ,
    //      };
    //      byte[] calculatedResult = CryptoNight.DoHash1(inputData);
    //      Assert.IsTrue(resultData.Length == calculatedResult.Length);

    //      for (int i = 0; i < resultData.Length; i++)
    //      {
    //        Assert.IsTrue(resultData[i] == calculatedResult[i]);
    //      }
    //    }

    [TestMethod()]
    public void E2E5Fast3Steps()
    {
      // piNonce == job_result.nonce == 537657678 == 200C014E -- is this wrong?
      // in json nonce is 4e010c20 == 1308691488
      // piHashVal == 215769439008401 -- is this wrong?
      EndToEnd(@"

{""jsonrpc"":""2.0"",""id"":1,""error"":null,""result"":{""id"":""421970933313569"",""status"":""OK"",""job"":{""job_id"":""00000022979abe5b"",""blob"":""0606c286d2cf054e62c484c4297e4afdce51d5326091796b8b5411f49d20b347a164f224d57b4000000020141c3792f28256e7558cc7b2a520da2c66ed518b8541a91637238e2cbce16e670f"",""target"":""b7d10000""}}}

        ", @"

{""method"":""submit"",""params"":{""id"":""421970933313569"",""job_id"":""00000022979abe5b"",""nonce"":""4e010c20"",""result"":""3fbc7b9a8b0df08b1067d87daa1cd8aa23ad7baffed4312691f6c3bc3dc40000""},""id"":1}

        ", "200C014A");
    }

    [TestMethod()]
    public void E2E5Fast()
    {
      // piNonce == job_result.nonce == 537657678 == 200C014E -- is this wrong?
      // in json nonce is 4e010c20 == 1308691488
      // piHashVal == 215769439008401 -- is this wrong?
      EndToEnd(@"

{""jsonrpc"":""2.0"",""id"":1,""error"":null,""result"":{""id"":""421970933313569"",""status"":""OK"",""job"":{""job_id"":""00000022979abe5b"",""blob"":""0606c286d2cf054e62c484c4297e4afdce51d5326091796b8b5411f49d20b347a164f224d57b4000000020141c3792f28256e7558cc7b2a520da2c66ed518b8541a91637238e2cbce16e670f"",""target"":""b7d10000""}}}

        ", @"

{""method"":""submit"",""params"":{""id"":""421970933313569"",""job_id"":""00000022979abe5b"",""nonce"":""4e010c20"",""result"":""3fbc7b9a8b0df08b1067d87daa1cd8aa23ad7baffed4312691f6c3bc3dc40000""},""id"":1}

        ", "200C014D");
    }

    [TestMethod()]
    public void E2E5()
    {
      // piNonce == job_result.nonce == 537657678 == 200C014E -- is this wrong?
      // in json nonce is 4e010c20 == 1308691488
      // piHashVal == 215769439008401 -- is this wrong?
      EndToEnd(@"

{""jsonrpc"":""2.0"",""id"":1,""error"":null,""result"":{""id"":""672296271781987"",""status"":""OK"",""job"":{""job_id"":""00000022979abe5b"",""blob"":""0606c286d2cf054e62c484c4297e4afdce51d5326091796b8b5411f49d20b347a164f224d57b4000000020141c3792f28256e7558cc7b2a520da2c66ed518b8541a91637238e2cbce16e670f"",""target"":""b7d10000""}}}

        ", @"

{""method"":""submit"",""params"":{""id"":""672296271781987"",""job_id"":""00000022979abe5b"",""nonce"":""4e010c20"",""result"":""3fbc7b9a8b0df08b1067d87daa1cd8aa23ad7baffed4312691f6c3bc3dc40000""},""id"":1}

        ");
    }

    [TestMethod()]
    public void E2E1Fast()
    {
      EndToEnd(@"

{""jsonrpc"":""2.0"",""id"":1,""error"":null,""result"":{""id"":""770893074783710"",""status"":""OK"",""job"":{""job_id"":""000000228b2212a9"",""blob"":""0606a78ac3cf05a9fca0d1560eea0d51bf8338af0f988282522c05eb993d7ebd721856228374a20000007b3eee48e6e2b5744e9884d61631d7f9b043630c1c343d10066d563265264f039407"",""target"":""b7d10000""}}}

        ", @"

{""method"":""submit"",""params"":{""id"":""421970933313569"",""job_id"":""000000228b2212a9"",""nonce"":""07110c7b"",""result"":""e5d18403a03ff4fdc27152202482bed2dfc5fdfd09e77a6ea7d471f2fa0c0000""},""id"":1}

        ", "7b0c1106");
    }

    [TestMethod()]
    public void E2E1()
    {
      EndToEnd(@"

{""jsonrpc"":""2.0"",""id"":1,""error"":null,""result"":{""id"":""770893074783710"",""status"":""OK"",""job"":{""job_id"":""000000228b2212a9"",""blob"":""0606a78ac3cf05a9fca0d1560eea0d51bf8338af0f988282522c05eb993d7ebd721856228374a20000007b3eee48e6e2b5744e9884d61631d7f9b043630c1c343d10066d563265264f039407"",""target"":""b7d10000""}}}

        ", @"

{""method"":""submit"",""params"":{""id"":""421970933313569"",""job_id"":""000000228b2212a9"",""nonce"":""07110c7b"",""result"":""e5d18403a03ff4fdc27152202482bed2dfc5fdfd09e77a6ea7d471f2fa0c0000""},""id"":1}

        ");
    }

    [TestMethod()]
    public void E2E2Fast()
    {
      EndToEnd(@"

{""jsonrpc"":""2.0"",""id"":1,""error"":null,""result"":{""id"":""421970933313569"",""status"":""OK"",""job"":{""job_id"":""0000002294244353"",""blob"":""0606f4eacdcf0510093efcf3c6ac3137a543059b364a9921a4f6dfadebe7989386e54d63c2145e000000ab3b19b1c82b839ccb543e5462f495f77ad1a9ccfa7954c97bed372c7f1350f4f807"",""target"":""b7d10000""}}}

        ", @"

{""method"":""submit"",""params"":{""id"":""421970933313569"",""job_id"":""0000002294244353"",""nonce"":""64020cab"",""result"":""afdd1376741ad538b44272b827ae1d799b5e28b663a83b7e7e8bc2b727a60000""},""id"":1}

        ", "ab0c0263");
    }

    [TestMethod()]
    public void E2E2()
    {
      EndToEnd(@"

{""jsonrpc"":""2.0"",""id"":1,""error"":null,""result"":{""id"":""905391288186024"",""status"":""OK"",""job"":{""job_id"":""0000002294244353"",""blob"":""0606f4eacdcf0510093efcf3c6ac3137a543059b364a9921a4f6dfadebe7989386e54d63c2145e000000ab3b19b1c82b839ccb543e5462f495f77ad1a9ccfa7954c97bed372c7f1350f4f807"",""target"":""b7d10000""}}}

        ", @"

{""method"":""submit"",""params"":{""id"":""905391288186024"",""job_id"":""0000002294244353"",""nonce"":""64020cab"",""result"":""afdd1376741ad538b44272b827ae1d799b5e28b663a83b7e7e8bc2b727a60000""},""id"":1}

        ");
    }

    [TestMethod()]
    public void E2E3()
    {
      EndToEnd(@"

{""params"":{""job_id"":""000000228b11006d"",""blob"":""0606f382c3cf053219fe60635f528da9f6732fc95f1e27585c552a2e62df8405243e0b731a59350000007bed6320ecf337b1818d9e9d5ce81964b9bc4a2af3535af3d22fd0ed9747d6490704"",""target"":""b7d10000""},""jsonrpc"":""2.0"",""method"":""job""}

        ", @"

{""method"":""submit"",""params"":{""id"":""421970933313569"",""job_id"":""000000228b11006d"",""nonce"":""0508047b"",""result"":""edeabc3dd5ee2bb84fe36f107f302982fa3da606ece6db32ca9bf77820350000""},""id"":1}

        ");
    }

    [TestMethod()]
    public void E2E3Fast()
    {
      EndToEnd(@"

{""params"":{""job_id"":""000000228b11006d"",""blob"":""0606f382c3cf053219fe60635f528da9f6732fc95f1e27585c552a2e62df8405243e0b731a59350000007bed6320ecf337b1818d9e9d5ce81964b9bc4a2af3535af3d22fd0ed9747d6490704"",""target"":""b7d10000""},""jsonrpc"":""2.0"",""method"":""job""}

        ", @"

{""method"":""submit"",""params"":{""id"":""421970933313569"",""job_id"":""000000228b11006d"",""nonce"":""0508047b"",""result"":""edeabc3dd5ee2bb84fe36f107f302982fa3da606ece6db32ca9bf77820350000""},""id"":1}

        ", "7b040804");
    }

    static void EndToEnd(
      string jsonIn,
      string jsonOut,
      string nonce = null)
    {
      jsonOut = jsonOut.Trim();

      Job result;
      try
      {

        NewJob newJob = JsonConvert.DeserializeObject<NewJob>(jsonIn);
        result = newJob.Result.Job;
      }
      catch
      {
        result = JsonConvert.DeserializeObject<NewBlock>(jsonIn).@params;
      }
      CryptoNight night = new CryptoNight();
      night.Process(result, (json) => Assert.IsTrue(json == jsonOut), nonce);
    }
  }

  public static class TestExtensions
  {
    public static void Process(
      this CryptoNight night,
      Job job,
      Action<string> onComplete = null,
      string nonceOverride = null)
    {
      night.Process(onComplete, 1, job.Job_Id, job.Blob, job.Target, nonceOverride);
    }
  }
}