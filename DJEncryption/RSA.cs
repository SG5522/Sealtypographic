using System;
using System.Security.Cryptography;
using System.Text;

namespace DJEncryption
{
    public class RSA
    {
        public static int DEFAULT_KEY_SIZE = 2048;
        public static bool DEFAULT_OAEP = false;

        public int KeySize { get; set; }

        public bool OAEP { get; set; }

        public string PrivateXML { get; set; }
        public string PublicXML { get; set; }

        /// <summary>
        /// 原始字串
        /// </summary>
        public string SourceString { get; set; }
        /// <summary>
        /// 加密後字串
        /// </summary>
        public string EncryptString { get; private set; }

        public RSA Generate()
        {
            return Generate(DEFAULT_KEY_SIZE, DEFAULT_OAEP);
        }

        public RSA Generate(int keySize, bool oaep)
        {
            RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(keySize);
            RSA result = new RSA
            {
                KeySize = keySize,
                OAEP = oaep,
                PrivateXML = rsaCryptoServiceProvider.ToXmlString(true),
                PublicXML = rsaCryptoServiceProvider.ToXmlString(false)
            };
            return result;
        }

        public void Encrypt()
        {
            EncryptString = Encrypt(SourceString, PublicXML, OAEP);
        }

        public static string Encrypt(string sourceString, string publicXML, bool oaep)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(sourceString), publicXML, oaep));
        }

        public static byte[] Encrypt(byte[] sourceBytes, string publicXML, bool oaep)
        {
            byte[] result = null;

            if (sourceBytes == null || sourceBytes.Length <= 0)
            {
                throw new ArgumentNullException("sourceBytes");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(publicXML))
                {
                    throw new ArgumentNullException("publicXML");
                }
                else
                {
                    using (RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider())
                    {
                        rsaCryptoServiceProvider.FromXmlString(publicXML);

                        result = rsaCryptoServiceProvider.Encrypt(sourceBytes, oaep);
                    }
                }
            }
            return result;
        }

        public void Decrypt()
        {
            SourceString = Decrypt(EncryptString, PrivateXML, OAEP);
        }

        public static string Decrypt(string encryptString, string privateXML, bool oaep)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(encryptString), privateXML, oaep));
        }

        public static byte[] Decrypt(byte[] encryptBytes, string privateXML, bool oaep)
        {
            byte[] result = null;

            if (encryptBytes == null || encryptBytes.Length <= 0)
            {
                throw new ArgumentNullException("encryptBytes");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(privateXML))
                {
                    throw new ArgumentNullException("privateXML");
                }
                else
                {
                    using (RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider())
                    {
                        rsaCryptoServiceProvider.FromXmlString(privateXML);

                        result = rsaCryptoServiceProvider.Decrypt(encryptBytes, oaep);
                    }
                }
            }
            return result;
        }
    }
}
