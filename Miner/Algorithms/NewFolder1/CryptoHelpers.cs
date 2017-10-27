using System;

namespace HD
{
  public class CryptoHelpers
  {
    public static uint ROTL32(
      uint v,
      int n)
    {
      return ((((v) << (n)) | ((v) >> (32 - (n)))) & 0xffffffff);
    }

    public static ulong ROTL64(
      ulong x,
      int y)
    {
      return (((x) << (y)) | ((x) >> (64 - (y))));
    }
  }
}
