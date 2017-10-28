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
    public void E2E5Fake3Steps()
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
    public void E2E5Fake()
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
    public void E2E1Fake()
    {
      EndToEnd(@"

{""jsonrpc"":""2.0"",""id"":1,""error"":null,""result"":{""id"":""770893074783710"",""status"":""OK"",""job"":{""job_id"":""000000228b2212a9"",""blob"":""0606a78ac3cf05a9fca0d1560eea0d51bf8338af0f988282522c05eb993d7ebd721856228374a20000007b3eee48e6e2b5744e9884d61631d7f9b043630c1c343d10066d563265264f039407"",""target"":""b7d10000""}}}

        ", @"

{""method"":""submit"",""params"":{""id"":""421970933313569"",""job_id"":""000000228b2212a9"",""nonce"":""07110c7b"",""result"":""e5d18403a03ff4fdc27152202482bed2dfc5fdfd09e77a6ea7d471f2fa0c0000""},""id"":1}

        ", "07110c7a");
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
      night.Process(result, nonce);
      
      while (true) // TODO move the loop into code not test
      {
        night.ProcessStep2();
        night.ProcessStep3();
        night.ProcessStep4();
        night.ProcessStep5();
        night.ProcessStep6();
        night.ProcessStep7();
        night.ProcessStep8();
        night.ProcessStep9();
        night.ProcessStep10();
        night.ProcessStep11();
        night.ProcessStep12();
        night.ProcessStep13();
        night.ProcessStep14();
        night.ProcessStep15();

        if (night.piHashVal < night.iTarget)
        {
          break;
        }
      }


      string resultJson = night.GetResultJson();

      Assert.IsTrue(resultJson == jsonOut);
    }

    [TestMethod()]
    public void AdHoc()
    {

      NewJob newJob = JsonConvert.DeserializeObject<NewJob>(@"

{""jsonrpc"":""2.0"",""id"":1,""error"":null,""result"":{""id"":""958279617719034"",""status"":""OK"",""job"":{""job_id"":""00000022914b1fba"",""blob"":""06069595cacf05621a11850207b551cb5324b43ac2bb9c6d12710ad60fc8f39be8ae67342913b2000000c1acdfded1636bc63a54387009741881078b0aeb1bad0fabaf23e0f9387a47f7b608"",""target"":""b7d10000""}}}
");
      CryptoNight night = new CryptoNight();
      night.Process(newJob.Result.Job);
      night.ProcessStep2();
      night.ProcessStep3();
      night.ProcessStep4();
      night.ProcessStep5();
      night.ProcessStep6();
      night.ProcessStep7();
      night.ProcessStep8();
      night.ProcessStep9();
      night.ProcessStep10();

      Assert.IsTrue(night.ctx.scratchpad[0] == 251);
      Assert.IsTrue(night.ctx.scratchpad[1231392] == 152);
    }



    [TestMethod()]
    public void Step15()
    {

      NewJob newJob = JsonConvert.DeserializeObject<NewJob>(@"

{""jsonrpc"":""2.0"",""id"":1,""error"":null,""result"":{""id"":""770893074783710"",""status"":""OK"",""job"":{""job_id"":""000000229377aa06"",""blob"":""0606889bcdcf056eefdfd76b7bf3387b5477f5d6e88f4daf315ee8623f887b90bd257e3ce584c4000000b1271ca0670f14b0586c35a92785256980598d1913acd27e901f4638e2edcf553406"",""target"":""b7d10000""}}}

");
      CryptoNight night = new CryptoNight();
      night.Process(newJob.Result.Job);
      night.ProcessStep2();
      night.ProcessStep3();
      night.ProcessStep4();
      night.ProcessStep5();
      night.ProcessStep6();
      night.ProcessStep7();
      night.ProcessStep8();
      night.ProcessStep9();
      night.ProcessStep10();
      night.ProcessStep11();
      night.ProcessStep12();
      night.ProcessStep13();
      night.ProcessStep14();
      night.ProcessStep15();

      Assert.IsTrue(night.ctx.bResult[0] == 118);
      Assert.IsTrue(night.ctx.bResult[1] == 187);
      Assert.IsTrue(night.ctx.bResult[31] == 81);

    }





    [TestMethod()]
    public void Step14()
    {

      NewJob newJob = JsonConvert.DeserializeObject<NewJob>(@"

{""jsonrpc"":""2.0"",""id"":1,""error"":null,""result"":{""id"":""770893074783710"",""status"":""OK"",""job"":{""job_id"":""000000229377aa06"",""blob"":""0606889bcdcf056eefdfd76b7bf3387b5477f5d6e88f4daf315ee8623f887b90bd257e3ce584c4000000b1271ca0670f14b0586c35a92785256980598d1913acd27e901f4638e2edcf553406"",""target"":""b7d10000""}}}

");
      CryptoNight night = new CryptoNight();
      night.Process(newJob.Result.Job);
      night.ProcessStep2();
      night.ProcessStep3();
      night.ProcessStep4();
      night.ProcessStep5();
      night.ProcessStep6();
      night.ProcessStep7();
      night.ProcessStep8();
      night.ProcessStep9();
      night.ProcessStep10();
      night.ProcessStep11();
      night.ProcessStep12();
      night.ProcessStep13();
      night.ProcessStep14();

      Assert.IsTrue(night.hashID == 0);
    }





    [TestMethod()]
    public void Step13()
    {

      NewJob newJob = JsonConvert.DeserializeObject<NewJob>(@"

{""jsonrpc"":""2.0"",""id"":1,""error"":null,""result"":{""id"":""770893074783710"",""status"":""OK"",""job"":{""job_id"":""000000229377aa06"",""blob"":""0606889bcdcf056eefdfd76b7bf3387b5477f5d6e88f4daf315ee8623f887b90bd257e3ce584c4000000b1271ca0670f14b0586c35a92785256980598d1913acd27e901f4638e2edcf553406"",""target"":""b7d10000""}}}

");
      CryptoNight night = new CryptoNight();
      night.Process(newJob.Result.Job);
      night.ProcessStep2();
      night.ProcessStep3();
      night.ProcessStep4();
      night.ProcessStep5();
      night.ProcessStep6();
      night.ProcessStep7();
      night.ProcessStep8();
      night.ProcessStep9();
      night.ProcessStep10();
      night.ProcessStep11();
      night.ProcessStep12();
      night.ProcessStep13();

      Assert.IsTrue(night.ctx.keccakHash[0] == 100);
      Assert.IsTrue(night.ctx.keccakHash[1] == 111);
      Assert.IsTrue(night.ctx.keccakHash[169] == 140);
      Assert.IsTrue(night.ctx.keccakHash[170] == 6);
      Assert.IsTrue(night.ctx.keccakHash[199] == 133);
    }






    [TestMethod()]
    public void Step12()
    {

      NewJob newJob = JsonConvert.DeserializeObject<NewJob>(@"

{""jsonrpc"":""2.0"",""id"":1,""error"":null,""result"":{""id"":""282481223474240"",""status"":""OK"",""job"":{""job_id"":""00000022936482b0"",""blob"":""0202f18acdcf05e7c56332c76be812ae86f7ed748b112fc4e5901195a8722208373a9e03ecaa37000000940c3e45aff1f308ecdec122c0735f0552a670bd91f00aa27ed54955e77f58ab3504"",""target"":""b7d10000""}}}

");
      CryptoNight night = new CryptoNight();
      night.Process(newJob.Result.Job);
      night.ProcessStep2();
      night.ProcessStep3();
      night.ProcessStep4();
      night.ProcessStep5();
      night.ProcessStep6();
      night.ProcessStep7();
      night.ProcessStep8();
      night.ProcessStep9();
      night.ProcessStep10();
      night.ProcessStep11();
      night.ProcessStep12();


      Assert.IsTrue(night.ctx.keccakHash[0] == 123);
      Assert.IsTrue(night.ctx.keccakHash[1] == 246);
      Assert.IsTrue(night.ctx.keccakHash[169] == 152);
      Assert.IsTrue(night.ctx.keccakHash[170] == 248);
      Assert.IsTrue(night.ctx.keccakHash[199] == 166);
    }


    [TestMethod()]
    public void Step11()
    {

      NewJob newJob = JsonConvert.DeserializeObject<NewJob>(@"

{""jsonrpc"":""2.0"",""id"":1,""error"":null,""result"":{""id"":""958279617719034"",""status"":""OK"",""job"":{""job_id"":""00000022914b1fba"",""blob"":""06069595cacf05621a11850207b551cb5324b43ac2bb9c6d12710ad60fc8f39be8ae67342913b2000000c1acdfded1636bc63a54387009741881078b0aeb1bad0fabaf23e0f9387a47f7b608"",""target"":""b7d10000""}}}

");
      CryptoNight night = new CryptoNight();
      night.Process(newJob.Result.Job);
      night.ProcessStep2();
      night.ProcessStep3();
      night.ProcessStep4();
      night.ProcessStep5();
      night.ProcessStep6();
      night.ProcessStep7();
      night.ProcessStep8();
      night.ProcessStep9();
      night.ProcessStep10();
      night.ProcessStep11();

      Assert.IsTrue(night.aes.WorkingKey[0][0] == 448915449);
      Assert.IsTrue(night.aes.WorkingKey[0][3] == 1450329266);
      Assert.IsTrue(night.aes.WorkingKey[9][3] == 4003620890);
    }






    [TestMethod()]
    public void Step10_HardLoop()
    {
      NewJob newJob = JsonConvert.DeserializeObject<NewJob>(@"

{""jsonrpc"":""2.0"",""id"":1,""error"":null,""result"":{""id"":""958279617719034"",""status"":""OK"",""job"":{""job_id"":""00000022914b1fba"",""blob"":""06069595cacf05621a11850207b551cb5324b43ac2bb9c6d12710ad60fc8f39be8ae67342913b2000000c1acdfded1636bc63a54387009741881078b0aeb1bad0fabaf23e0f9387a47f7b608"",""target"":""b7d10000""}}}
");
      CryptoNight night = new CryptoNight();
      night.Process(newJob.Result.Job);
      night.ProcessStep2();
      night.ProcessStep3();
      night.ProcessStep4();
      night.ProcessStep5();
      night.ProcessStep6();
      night.ProcessStep7();
      night.ProcessStep8();
      night.ProcessStep9();
      night.ProcessStep10();

      Assert.IsTrue(night.ctx.scratchpad[0] == 251);
      Assert.IsTrue(night.ctx.scratchpad[1231392] == 152);
    }





    [TestMethod()]
    public void Step9_AB()
    {
      NewJob newJob = JsonConvert.DeserializeObject<NewJob>(@"

{""jsonrpc"":""2.0"",""id"":1,""error"":null,""result"":{""id"":""284736284218945"",""status"":""OK"",""job"":{""job_id"":""00000022903015a0"",""blob"":""0606c2e7c8cf0593ae736ed60c197b674b1f8db1d83e0ea17349a0f062ea80d684e52e0325f5de000000758e40ef9ecefbac1cd699cbbc997986a678af8e5f6a8c64eb77a9de2ad24f54d008"",""target"":""b7d10000""}}}


");
      CryptoNight night = new CryptoNight();
      night.Process(newJob.Result.Job);
      night.ProcessStep2();
      night.ProcessStep3();
      night.ProcessStep4();
      night.ProcessStep5();
      night.ProcessStep6();
      night.ProcessStep7();
      night.ProcessStep8();
      night.ProcessStep9();

      Assert.IsTrue(night.a[0] == 132);
      Assert.IsTrue(night.a[1] == 94);
      Assert.IsTrue(night.a[7] == 235);
      Assert.IsTrue(night.a[8] == 80);

      Assert.IsTrue(night.b[0] == 207);
      Assert.IsTrue(night.b[6] == 143);
      Assert.IsTrue(night.b[15] == 157);
    }



    [TestMethod()]
    public void Step8_CompleteScratch()
    {
      NewJob newJob = JsonConvert.DeserializeObject<NewJob>(@"

{""jsonrpc"":""2.0"",""id"":1,""error"":null,""result"":{""id"":""236837541229689"",""status"":""OK"",""job"":{""job_id"":""000000228ffb9d4b"",""blob"":""0606cacac8cf05536e5b97528e2f1dc190fc7ebb494494303fc0b9d2e71df6b6926d7db934a2e5000000275ad8b6a20aa530fba2a492780ac934dfd8a359df3dd02c88d38be3c1dd15edb507"",""target"":""b7d10000""}}}

");
      CryptoNight night = new CryptoNight();
      night.Process(newJob.Result.Job);
      night.ProcessStep2();
      night.ProcessStep3();
      night.ProcessStep4();
      night.ProcessStep5();
      night.ProcessStep6();
      night.ProcessStep7();
      night.ProcessStep8();


      Assert.IsTrue(night.ctx.scratchpad[0] == 205);
      Assert.IsTrue(night.ctx.scratchpad[3] == 28);
      Assert.IsTrue(night.ctx.scratchpad[99] == 87);
      Assert.IsTrue(night.ctx.scratchpad[127] == 241);
      Assert.IsTrue(night.ctx.scratchpad[128] == 29);
      Assert.IsTrue(night.ctx.scratchpad[209711] == 52);
      Assert.IsTrue(night.ctx.scratchpad[2097151] == 191);
    }



    [TestMethod()]
    public void Step7_StoreInScratch()
    {
      NewJob newJob = JsonConvert.DeserializeObject<NewJob>(@"

{""jsonrpc"":""2.0"",""id"":1,""error"":null,""result"":{""id"":""236837541229689"",""status"":""OK"",""job"":{""job_id"":""000000228ffb9d4b"",""blob"":""0606cacac8cf05536e5b97528e2f1dc190fc7ebb494494303fc0b9d2e71df6b6926d7db934a2e5000000275ad8b6a20aa530fba2a492780ac934dfd8a359df3dd02c88d38be3c1dd15edb507"",""target"":""b7d10000""}}}

");
      CryptoNight night = new CryptoNight();
      night.Process(newJob.Result.Job);
      night.ProcessStep2();
      night.ProcessStep3();
      night.ProcessStep4();
      night.ProcessStep5();
      night.ProcessStep6();
      night.ProcessStep7();

      Assert.IsTrue(night.ctx.scratchpad[0] == 205);
      Assert.IsTrue(night.ctx.scratchpad[3] == 28);
      Assert.IsTrue(night.ctx.scratchpad[99] == 87);
      Assert.IsTrue(night.ctx.scratchpad[127] == 241);
    }

    [TestMethod()]
    public void Step6_OneRoundPerBlock()
    {
      NewJob newJob = JsonConvert.DeserializeObject<NewJob>(@"

{""jsonrpc"":""2.0"",""id"":1,""error"":null,""result"":{""id"":""236837541229689"",""status"":""OK"",""job"":{""job_id"":""000000228ffb9d4b"",""blob"":""0606cacac8cf05536e5b97528e2f1dc190fc7ebb494494303fc0b9d2e71df6b6926d7db934a2e5000000275ad8b6a20aa530fba2a492780ac934dfd8a359df3dd02c88d38be3c1dd15edb507"",""target"":""b7d10000""}}}

");
      CryptoNight night = new CryptoNight();
      night.Process(newJob.Result.Job);
      night.ProcessStep2();
      night.ProcessStep3();
      night.ProcessStep4();
      night.ProcessStep5();
      night.ProcessStep6();

      Assert.IsTrue(night.blocks[0][0] == 205);
      Assert.IsTrue(night.blocks[0][1] == 115);
      Assert.IsTrue(night.blocks[0][15] == 205);
      Assert.IsTrue(night.blocks[7][0] == 123);
      Assert.IsTrue(night.blocks[7][6] == 115);
      Assert.IsTrue(night.blocks[7][15] == 241);
    }

    [TestMethod()]
    public void Step4_ConfirmAESKey()
    {
      NewJob newJob = JsonConvert.DeserializeObject<NewJob>("{\"jsonrpc\":\"2.0\",\"id\":1,\"error\":null,\"result\":{\"id\":\"935427267220117\",\"status\":\"OK\",\"job\":{\"job_id\":\"000000228f4f7dce\",\"blob\":\"0606aae6c7cf05be009d308985e25cfeaadc4f3198458dba946bce18810a8ce747259738f932980000000d504b50169cbf70974edce6b18a47b094c0604047762a9d998b58e54cf8b0d71707\",\"target\":\"b7d10000\"}}}");
      CryptoNight night = new CryptoNight();
      night.Process(newJob.Result.Job);
      night.ProcessStep2();
      night.ProcessStep3();
      night.ProcessStep4();

      Assert.IsTrue(night.aes.WorkingKey[0][0] == 620964217);
      Assert.IsTrue(night.aes.WorkingKey[1][0] == 46193916);
      Assert.IsTrue(night.aes.WorkingKey[7][1] == 3590950729);
      Assert.IsTrue(night.aes.WorkingKey[9][3] == 407388248);
    }
    [TestMethod()]
    public void Step5_ConfirmBlocks()
    {
      NewJob newJob = JsonConvert.DeserializeObject<NewJob>("{\"jsonrpc\":\"2.0\",\"id\":1,\"error\":null,\"result\":{\"id\":\"935427267220117\",\"status\":\"OK\",\"job\":{\"job_id\":\"000000228f4f7dce\",\"blob\":\"0606aae6c7cf05be009d308985e25cfeaadc4f3198458dba946bce18810a8ce747259738f932980000000d504b50169cbf70974edce6b18a47b094c0604047762a9d998b58e54cf8b0d71707\",\"target\":\"b7d10000\"}}}");
      CryptoNight night = new CryptoNight();
      night.Process(newJob.Result.Job);
      night.ProcessStep2();
      night.ProcessStep3();
      night.ProcessStep4();
      night.ProcessStep5();

      Assert.IsTrue(night.blocks[0][0] == 73);
      Assert.IsTrue(night.blocks[0][1] == 255);
      Assert.IsTrue(night.blocks[0][2] == 189);
      Assert.IsTrue(night.blocks[5][2] == 209);
      Assert.IsTrue(night.blocks[5][9] == 80);
      Assert.IsTrue(night.blocks[7][15] == 76);
      Assert.IsTrue(night.blocks.Length == 8);
    }

    /// <summary>
    /// Up to while(iGlobalJobNo.load(std::memory_order_relaxed) == iJobNo)
    /// </summary>
    [TestMethod()]
    public void ProcessTest()
    {
      NewJob newJob = JsonConvert.DeserializeObject<NewJob>("{\"jsonrpc\":\"2.0\",\"id\":1,\"error\":null,\"result\":{\"id\":\"831431040790814\",\"status\":\"OK\",\"job\":{\"job_id\":\"000000228b507492\",\"blob\":\"0606aea6c3cf055b878e5f92902e6691cb6be34012b2d20f6cd1fe74ebc7306b35e3cb55e4782000000092444031f7a6023a17838d25035743300cf2b8db6dcb539e1fc6e303475c5d763501\",\"target\":\"b7d10000\"}}}");
      CryptoNight night = new CryptoNight();
      night.Process(newJob.Result.Job);

      Assert.IsTrue(night.bWorkBlob[0] == 6);
      Assert.IsTrue(night.bWorkBlob[5] == 207);
      Assert.IsTrue(night.bWorkBlob[73] == 118);
      Assert.IsTrue(night.bWorkBlob[75] == 1);
      Assert.IsTrue(night.iTarget == (ulong)230584300921369);
      Assert.IsTrue(night.iJobDiff == (ulong)80000);
      Assert.IsTrue(night.iCount == 0);
      Assert.IsTrue(night.piNonce == 2449473536);
    }



    [TestMethod()]
    public void ProcessTest2()
    {
      NewJob newJob = JsonConvert.DeserializeObject<NewJob>("{\"jsonrpc\":\"2.0\",\"id\":1,\"error\":null,\"result\":{\"id\":\"905120989173307\",\"status\":\"OK\",\"job\":{\"job_id\":\"000000228b601b20\",\"blob\":\"0405eeafc3cf0584f60a594f4850e6315aae83ae95cf980d8407346037895c65a2e324e858e18a0000006a3c7ebcfcf867030b6e03b52c736cfecdf3f1a9ccc06bb69382669b3b49c6905f02\",\"target\":\"b7d10000\"}}}");
      CryptoNight night = new CryptoNight();
      night.Process(newJob.Result.Job);

      // Passing
      Assert.AreEqual(night.bWorkBlob[0], 4);
      Assert.AreEqual(night.bWorkBlob[5], 207);
      Assert.AreEqual(night.bWorkBlob[73], 144);
      Assert.AreEqual(night.bWorkBlob[75], 2);
      Assert.AreEqual(night.piNonce, (uint)1778384896);

      night.ProcessStep2();

      Assert.IsTrue(night.piNonce == 1778384897);
      Assert.IsTrue(night.bWorkBlob[38] == 138);
      Assert.IsTrue(night.bWorkBlob[39] == 1);
      Assert.IsTrue(night.bWorkBlob[42] == 106);

      night.ProcessStep3();

      Assert.IsTrue(night.ctx.keccakHash[0] == 79);
      Assert.IsTrue(night.ctx.keccakHash[10] == 11);
      Assert.IsTrue(night.ctx.keccakHash[129] == 59);
      Assert.IsTrue(night.ctx.keccakHash[199] == 76);
    }
  }
}