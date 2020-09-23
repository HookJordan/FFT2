using System.Security.Cryptography;
using System.Text;

namespace Common.Security
{
    public class Hashing
    {
        public static string SHA(string data)
        {
            return SHA(Encoding.ASCII.GetBytes(data));
        }

        public static string SHA(byte[] data)
        {
            using (SHA256 sha = SHA256.Create())
            {
                data = sha.ComputeHash(data);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
