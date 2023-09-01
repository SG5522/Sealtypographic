using System;
using System.Security.Cryptography;
using System.Text;

namespace DJEncryption
{
    public class RSA
    {
        /// <summary>
        /// 預設Key Size
        /// </summary>
        public static int DEFAULT_KEY_SIZE = 2048;
        /// <summary>
        /// 預設的OAEP false
        /// OAEP = Optimal Asymmetric Encryption Padding (最優非對稱加密填充)
        /// </summary>
        public static bool DEFAULT_OAEP = false;

        /// <summary>
        /// 設定加密的KeySize 範圍(1024~15360)
        /// </summary>
        public int KeySize { get; set; }

        /// <summary>
        /// 是否啟用 Optimal Asymmetric Encryption Padding (最優非對稱加密填充)
        /// </summary>
        public bool OAEP { get; set; }

        /// <summary>
        /// 私鑰字串(XML格式)
        /// </summary>
        public string PrivateXML { get; set; }

        /// <summary>
        /// 公鑰字串(XML格式)
        /// </summary>
        public string PublicXML { get; set; }

        /// <summary>
        /// 原始字串
        /// </summary>
        public string SourceString { get; set; }

        /// <summary>
        /// 加密後字串
        /// </summary>
        public string EncryptString { get; private set; }

        /// <summary>
        /// 建立RSA 公鑰 私鑰    
        /// </summary>
        /// <returns></returns>
        public RSA Generate()
        {
            return Generate(DEFAULT_KEY_SIZE, DEFAULT_OAEP);
        }

        /// <summary>
        /// 建立RSA基本參數
        /// </summary>
        /// <param name="keySize"></param>
        /// <param name="oaep">是否啟用 Optimal Asymmetric Encryption Padding (最優非對稱加密填充)</param>
        /// <returns></returns>
        public RSA Generate(int keySize, bool oaep)
        {
            RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(keySize);
            RSA result = new RSA
            {
                KeySize = keySize,
                OAEP = oaep,
                //產生私鑰
                PrivateXML = rsaCryptoServiceProvider.ToXmlString(true),
                //產生公鑰
                PublicXML = rsaCryptoServiceProvider.ToXmlString(false)
            };
            return result;
        }

        /// <summary>
        /// 加密處理(使用自己CLASS設定的參數)
        /// </summary>
        public void Encrypt()
        {
            EncryptString = Encrypt(SourceString, PublicXML, OAEP);
        }

        /// <summary>
        /// 加密處理(字串輸入)
        /// </summary>
        /// <param name="sourceString">原始字串或是base64</param>
        /// <param name="publicXML">公鑰(XML字串)</param>
        /// <param name="oaep">是否啟用oaep</param>
        /// <returns></returns>
        public static string Encrypt(string sourceString, string publicXML, bool oaep)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(sourceString), publicXML, oaep));
        }

        /// <summary>
        /// 加密處理(bytes輸入)
        /// </summary>
        /// <param name="sourceBytes">來源bytes</param>
        /// <param name="publicXML">公鑰(XML字串)</param>
        /// <param name="oaep">是否啟用oaep</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the input string is null.</exception>
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

        /// <summary>
        /// 解密處理(使用自己CLASS設定的參數)
        /// </summary>
        public void Decrypt()
        {
            SourceString = Decrypt(EncryptString, PrivateXML, OAEP);
        }

        /// <summary>
        /// 解密處理(字串輸入)
        /// </summary>
        /// <param name="encryptString">加密字串</param>
        /// <param name="privateXML">私鑰</param>
        /// <param name="oaep">是否啟用oaep</param>
        /// <returns></returns>
        public static string Decrypt(string encryptString, string privateXML, bool oaep)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(encryptString), privateXML, oaep));
        }

        /// <summary>
        /// 解密處理(bytes輸入)
        /// </summary>
        /// <param name="encryptBytes">加密(Bytes)</param>
        /// <param name="privateXML">私鑰</param>
        /// <param name="oaep">是否啟用oaep</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the input string is null.</exception>
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
