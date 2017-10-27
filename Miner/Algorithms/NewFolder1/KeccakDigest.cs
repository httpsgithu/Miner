using System;
using System.Diagnostics;

namespace HD
{
  public static class KeccakDigest
  {
    const int HASH_DATA_AREA = 136;
    public const int KECCAK_ROUNDS = 24;
    static readonly ulong[] keccakf_rndc = new ulong[]
    {
      0x0000000000000001, 0x0000000000008082, 0x800000000000808a,
      0x8000000080008000, 0x000000000000808b, 0x0000000080000001,
      0x8000000080008081, 0x8000000000008009, 0x000000000000008a,
      0x0000000000000088, 0x0000000080008009, 0x000000008000000a,
      0x000000008000808b, 0x800000000000008b, 0x8000000000008089,
      0x8000000000008003, 0x8000000000008002, 0x8000000000000080,
      0x000000000000800a, 0x800000008000000a, 0x8000000080008081,
      0x8000000000008080, 0x0000000080000001, 0x8000000080008008
    };

    public static void keccak(
      byte[] input,
      int inlen,
      byte[] md,
      int mdlen)
    {
      ulong[] st = new ulong[25];
      byte[] temp = new byte[144];
      int i, rsiz, rsizw;

      rsiz = sizeof(ulong) * st.Length == mdlen ? HASH_DATA_AREA : 200 - 2 * mdlen;
      rsizw = rsiz / 8;

      for (int inputOffset = 0; inlen >= rsiz; inlen -= rsiz, inputOffset += rsiz / sizeof(long))
      {
        for (i = 0; i < rsizw; i++)
        {
          st[i] ^= BitConverter.ToUInt64(input, i * sizeof(long));
        }

        keccakf(st, KECCAK_ROUNDS);
      }

      // last block and padding
      for (int inputIndex = 0; inputIndex < inlen / sizeof(byte); inputIndex++)
      {
        temp[inputIndex] = input[inputIndex];
      }

      temp[inlen++] = 1;


      temp[rsiz - 1] |= 0x80;

      for (i = 0; i < rsizw; i++)
      {
        st[i] ^= BitConverter.ToUInt64(temp, i * sizeof(long) / sizeof(byte));
      }

      keccakf(st, KECCAK_ROUNDS);

      for (int stIndex = 0; stIndex < mdlen / sizeof(ulong); stIndex++)
      {
        byte[] data = BitConverter.GetBytes(st[stIndex]);
        md[stIndex * (sizeof(ulong) / sizeof(byte))] = data[0];
        md[stIndex * (sizeof(ulong) / sizeof(byte)) + 1] = data[1];
        md[stIndex * (sizeof(ulong) / sizeof(byte)) + 2] = data[2];
        md[stIndex * (sizeof(ulong) / sizeof(byte)) + 3] = data[3];
        md[stIndex * (sizeof(ulong) / sizeof(byte)) + 4] = data[4];
        md[stIndex * (sizeof(ulong) / sizeof(byte)) + 5] = data[5];
        md[stIndex * (sizeof(ulong) / sizeof(byte)) + 6] = data[6];
        md[stIndex * (sizeof(ulong) / sizeof(byte)) + 7] = data[7];
      }
    }

    public static void keccakf(
      ulong[] st,
      int rounds)
    {
      Debug.Assert(st.Length == 25);

      int i, j, round;
      ulong t;
      ulong[] bc = new ulong[5];

      for (round = 0; round < rounds; ++round)
      {
        // Theta
        bc[0] = st[0] ^ st[5] ^ st[10] ^ st[15] ^ st[20];
        bc[1] = st[1] ^ st[6] ^ st[11] ^ st[16] ^ st[21];
        bc[2] = st[2] ^ st[7] ^ st[12] ^ st[17] ^ st[22];
        bc[3] = st[3] ^ st[8] ^ st[13] ^ st[18] ^ st[23];
        bc[4] = st[4] ^ st[9] ^ st[14] ^ st[19] ^ st[24];

        for (i = 0; i < 5; ++i)
        {
          t = bc[(i + 4) % 5] ^ CryptoHelpers.ROTL64(bc[(i + 1) % 5], 1);
          st[i] ^= t;
          st[i + 5] ^= t;
          st[i + 10] ^= t;
          st[i + 15] ^= t;
          st[i + 20] ^= t;
        }

        // Rho Pi
        t = st[1];
        st[1] = CryptoHelpers.ROTL64(st[6], 44);
        st[6] = CryptoHelpers.ROTL64(st[9], 20);
        st[9] = CryptoHelpers.ROTL64(st[22], 61);
        st[22] = CryptoHelpers.ROTL64(st[14], 39);
        st[14] = CryptoHelpers.ROTL64(st[20], 18);
        st[20] = CryptoHelpers.ROTL64(st[2], 62);
        st[2] = CryptoHelpers.ROTL64(st[12], 43);
        st[12] = CryptoHelpers.ROTL64(st[13], 25);
        st[13] = CryptoHelpers.ROTL64(st[19], 8);
        st[19] = CryptoHelpers.ROTL64(st[23], 56);
        st[23] = CryptoHelpers.ROTL64(st[15], 41);
        st[15] = CryptoHelpers.ROTL64(st[4], 27);
        st[4] = CryptoHelpers.ROTL64(st[24], 14);
        st[24] = CryptoHelpers.ROTL64(st[21], 2);
        st[21] = CryptoHelpers.ROTL64(st[8], 55);
        st[8] = CryptoHelpers.ROTL64(st[16], 45);
        st[16] = CryptoHelpers.ROTL64(st[5], 36);
        st[5] = CryptoHelpers.ROTL64(st[3], 28);
        st[3] = CryptoHelpers.ROTL64(st[18], 21);
        st[18] = CryptoHelpers.ROTL64(st[17], 15);
        st[17] = CryptoHelpers.ROTL64(st[11], 10);
        st[11] = CryptoHelpers.ROTL64(st[7], 6);
        st[7] = CryptoHelpers.ROTL64(st[10], 3);
        st[10] = CryptoHelpers.ROTL64(t, 1);

        //  Chi
        // unrolled loop, where only last iteration is different
        j = 0;
        bc[0] = st[j];
        bc[1] = st[j + 1];

        st[j] ^= (~st[j + 1]) & st[j + 2];
        st[j + 1] ^= (~st[j + 2]) & st[j + 3];
        st[j + 2] ^= (~st[j + 3]) & st[j + 4];
        st[j + 3] ^= (~st[j + 4]) & bc[0];
        st[j + 4] ^= (~bc[0]) & bc[1];

        j = 5;
        bc[0] = st[j];
        bc[1] = st[j + 1];

        st[j] ^= (~st[j + 1]) & st[j + 2];
        st[j + 1] ^= (~st[j + 2]) & st[j + 3];
        st[j + 2] ^= (~st[j + 3]) & st[j + 4];
        st[j + 3] ^= (~st[j + 4]) & bc[0];
        st[j + 4] ^= (~bc[0]) & bc[1];

        j = 10;
        bc[0] = st[j];
        bc[1] = st[j + 1];

        st[j] ^= (~st[j + 1]) & st[j + 2];
        st[j + 1] ^= (~st[j + 2]) & st[j + 3];
        st[j + 2] ^= (~st[j + 3]) & st[j + 4];
        st[j + 3] ^= (~st[j + 4]) & bc[0];
        st[j + 4] ^= (~bc[0]) & bc[1];

        j = 15;
        bc[0] = st[j];
        bc[1] = st[j + 1];

        st[j] ^= (~st[j + 1]) & st[j + 2];
        st[j + 1] ^= (~st[j + 2]) & st[j + 3];
        st[j + 2] ^= (~st[j + 3]) & st[j + 4];
        st[j + 3] ^= (~st[j + 4]) & bc[0];
        st[j + 4] ^= (~bc[0]) & bc[1];

        j = 20;
        bc[0] = st[j];
        bc[1] = st[j + 1];
        bc[2] = st[j + 2];
        bc[3] = st[j + 3];
        bc[4] = st[j + 4];

        st[j] ^= (~bc[1]) & bc[2];
        st[j + 1] ^= (~bc[2]) & bc[3];
        st[j + 2] ^= (~bc[3]) & bc[4];
        st[j + 3] ^= (~bc[4]) & bc[0];
        st[j + 4] ^= (~bc[0]) & bc[1];

        //  Iota
        st[0] ^= keccakf_rndc[round];
      }
    }
  }
}
