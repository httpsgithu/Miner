using System;
using System.Text;

namespace HD
{
  public static class ByteArrayExtensions
  {
    public static string ToHexString(
      this byte[] valueList)
    {
      StringBuilder hex = new StringBuilder(valueList.Length * 2);
      for (int i = 0; i < valueList.Length; i++)
      {
        hex.AppendFormat("{0:x2}", valueList[i]);
      }

      return hex.ToString();
    }
  }
}
