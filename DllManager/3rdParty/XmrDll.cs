//using System;
//using System.Diagnostics;
//using System.Runtime.InteropServices;
//using System.Threading;

//namespace HD
//{
//  public class XmrDll
//  {
//    [DllImport("xmr-stak-cpu.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl,
//               BestFitMapping = false,
//               ThrowOnUnmappableChar = true)]
//    static extern bool Start(
//      string configurationJson);

//    [DllImport("xmr-stak-cpu.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl,
//               BestFitMapping = false,
//               ThrowOnUnmappableChar = true)]
//    static extern void Stop();

//    public void StartMining(
//      string wallet,
//      int numberOfThreads)
//    {
//      string json = Xmr.GenerateConfigJson(wallet, numberOfThreads);
//      // Requires whitespace at the start of the string.
//      Start(json);

//      //while (true)
//      //{
//      //  Console.WriteLine("Test");
//      //  Thread.Sleep(200);
//      //}
//      //Console.WriteLine("Test");
//      //Console.WriteLine("Test");
//      //Console.WriteLine("Test");
//      //Console.WriteLine("Test");
//      //Console.WriteLine("Test");
//      //Console.WriteLine("Test");
//      //Console.WriteLine("Test");
//      //Console.WriteLine("Test");
//      //Console.WriteLine("Test");
//      //Console.WriteLine("Test");
//      //Console.WriteLine("Test");
//      //Console.WriteLine("Test");

     
//    }

//    public void StopMining()
//    {
//      Stop();
//    }
//  }
//}
