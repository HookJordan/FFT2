using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Common.Security.Cryptography
{
    public class TripleDES
    {
        private static readonly byte[] SALT = Encoding.ASCII.GetBytes("FFT2 - FastFileTransfer2 - DES SALT");

        public static byte[] Encrypt(byte[] data, byte[] key)
        {
            try
            {
                key = key.Take(16).ToArray();
                using (TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider())
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
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);

                return null;
            }
        }

        public static byte[] Decrypt(byte[] data, byte[] key)
        {
            key = key.Take(16).ToArray();
            using (TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider())
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
