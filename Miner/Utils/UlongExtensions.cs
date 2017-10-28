using System;
using System.Diagnostics;

namespace HD
{
  public static class UlongExtensions
  {
    public unsafe static void GetBytes(
      this ulong value,
      byte[] output,
      int outputStartingIndex = 0)
    {
      Debug.Assert(output.Length - outputStartingIndex >= 8);
      fixed (byte* b = output)
      {
        *((ulong*)(b + outputStartingIndex)) = value;
      }
    }
  }
}
