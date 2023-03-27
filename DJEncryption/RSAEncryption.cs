using System;
using System.Security.Cryptography;
using System.Text;

namespace DJEncryption
{
    public class RSAEncryption
    {
        private readonly RSACryptoServiceProvider rSA;

        public RSAEncryption()
        {
            rSA = new RSACryptoServiceProvider();
        }

        public string GetPublicKey()
        {
            return rSA.ToXmlString(false);
        }

        public string GetPrivateKey()
        {
            return rSA.ToXmlString(true);
        }

        public string Encrypt(string plainText, string publicKey)
        {
            rSA.FromXmlString(publicKey);
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var encryptedBytes = rSA.Encrypt(plainBytes, false);
            return Convert.ToBase64String(encryptedBytes);
        }

        public string Decrypt(string cipherText, string privateKey)
        {
            rSA.FromXmlString(privateKey);
            var cipherBytes = Convert.FromBase64String(cipherText);
            var decryptedBytes = rSA.Decrypt(cipherBytes, false);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
