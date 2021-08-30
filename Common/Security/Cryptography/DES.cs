using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Common.Security.Cryptography
{
    public class DES
    {
        private static byte[] SALT = Encoding.ASCII.GetBytes("FFT2 - FastFileTransfer2 - DES SALT");

        public static byte[] Encrypt(byte[] data, byte[] key)
        {
            key = key.Take(8).ToArray();
            using (DESCryptoServiceProvider provider = new DESCryptoServiceProvider())
            {
                ICryptoTransform transform = provider.CreateEncryptor(key, SALT);
                CryptoStreamMode mode = CryptoStreamMode.Write;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, transform, mode))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.FlushFinalBlock();
                    }

                    return ms.ToArray();
                }
            }
        }

        public static byte[] Decrypt(byte[] data, byte[] key)
        {
            key = key.Take(8).ToArray();
            using (DESCryptoServiceProvider provider = new DESCryptoServiceProvider())
            {
                ICryptoTransform transform = provider.CreateDecryptor(key, SALT);
                CryptoStreamMode mode = CryptoStreamMode.Write;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, transform, mode))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.FlushFinalBlock();
                    }

                    return ms.ToArray();
                }
            }
        }
    }
}
