using System;
using System.Diagnostics;

namespace HD
{
  public static class StringExtensions
  {
    public static void ToByteArrayFromHex(
      this string hexString, 
      byte[] output)
    {
      Debug.Assert(hexString.Length % 2 == 0);
      Debug.Assert(output.Length == hexString.Length / 2);

      for (int index = 0; index < output.Length; index++)
      {
        string byteValue = hexString.Substring(index * 2, 2);
        output[index] = Convert.ToByte(byteValue, 16);
      }
    }
  }
}
