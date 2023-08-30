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
            string test = Convert.ToBase64String(rSA.ExportCspBlob(false));
            return rSA.ToXmlString(false);
        }

        public string GetPrivateKey()
        {
            string test = Convert.ToBase64String(rSA.ExportCspBlob(true));
            return rSA.ToXmlString(true);
        }

        public string Encrypt(string plainText, string publicKey)
        {
            //rSA.FromXmlString(publicKey);
            rSA.ImportCspBlob(Encoding.UTF8.GetBytes(publicKey));
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = rSA.Encrypt(plainBytes, false);
            return Convert.ToBase64String(encryptedBytes);
        }

        public string Decrypt(string cipherText, string privateKey)
        {
            //rSA.FromXmlString(privateKey);
            rSA.ImportCspBlob(Encoding.UTF8.GetBytes(privateKey));
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] decryptedBytes = rSA.Decrypt(cipherBytes, false);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
