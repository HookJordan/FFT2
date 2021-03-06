﻿using System.Text;

namespace Common.Security.Cryptography
{
    public class RC4
    {
        public static byte[] PerformByCopy(byte[] bytes, string key)
        {
            byte[] dec = new byte[bytes.Length];
            bytes.CopyTo(dec, 0);
            Perform(ref dec, key);
            return dec;
        }
        public static void Perform(ref byte[] bytes, string key)
        {
            Perform(ref bytes, Encoding.ASCII.GetBytes(Hashing.SHA(key)));
        }

        public static void Perform(ref byte[] bytes, byte[] key)
        {
            byte[] s = new byte[256];
            byte[] k = new byte[256];
            byte temp;
            int i, j;

            for (i = 0; i < 256; i++)
            {
                s[i] = (byte)i;
                k[i] = key[i % key.GetLength(0)];
            }

            j = 0;
            for (i = 0; i < 256; i++)
            {
                j = (j + s[i] + k[i]) % 256;
                temp = s[i];
                s[i] = s[j];
                s[j] = temp;
            }

            i = j = 0;
            for (int x = 0; x < bytes.GetLength(0); x++)
            {
                i = (i + 1) % 256;
                j = (j + s[i]) % 256;
                temp = s[i];
                s[i] = s[j];
                s[j] = temp;
                int t = (s[i] + s[j]) % 256;
                bytes[x] ^= s[t];
            }
        }
    }
}
