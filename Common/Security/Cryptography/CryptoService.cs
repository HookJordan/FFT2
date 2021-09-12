using System.Text;

namespace Common.Security.Cryptography
{
    public enum CryptoServiceAlgorithm : byte
    {
        Disabled = 0,
        AES,
        RC4,
        XOR,
        TripleDES
    }

    public class CryptoService
    {
        public CryptoServiceAlgorithm Algorithm { get; private set; }
        private readonly byte[] key;

        public CryptoService(CryptoServiceAlgorithm algorithm, string password)
        {
            Algorithm = algorithm;
            key = Encoding.ASCII.GetBytes(password);
        }


        public byte[] Encrypt(byte[] input)
        {
            switch (Algorithm)
            {
                case CryptoServiceAlgorithm.AES:
                    input = AES.Encrypt(input, key);
                    break;
                case CryptoServiceAlgorithm.RC4:
                    RC4.Perform(ref input, key);
                    break;
                case CryptoServiceAlgorithm.XOR:
                    XOR.Perform(ref input, key);
                    break;
                case CryptoServiceAlgorithm.TripleDES:
                    input = TripleDES.Encrypt(input, key);
                    break;
                default: // No encryption
                    break;
            }

            return input;
        }

        public byte[] Decrypt(byte[] input)
        {
            switch (Algorithm)
            {
                case CryptoServiceAlgorithm.AES:
                    input = AES.Decrypt(input, key);
                    break;
                case CryptoServiceAlgorithm.RC4:
                    RC4.Perform(ref input, key);
                    break;
                case CryptoServiceAlgorithm.XOR:
                    XOR.Perform(ref input, key);
                    break;
                case CryptoServiceAlgorithm.TripleDES:
                    input = TripleDES.Decrypt(input, key);
                    break;
                default: // No encryption
                    break;
            }

            return input;
        }
    }
}
