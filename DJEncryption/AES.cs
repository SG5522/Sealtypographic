using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DJEncryption
{
    /// <summary>
    /// AES 加解密
    /// </summary>
    public class AES
    {
        /// <summary>
        /// 預設Key Size
        /// </summary>
        public static int DEFAULT_KEYSIZE = 256;
        /// <summary>
        /// 預設Cipher Mode
        /// </summary>
        public static CipherMode DEFAULT_CIPHER_MODE = CipherMode.CBC;
        /// <summary>
        /// 預設Padding Mode
        /// </summary>
        public static PaddingMode DEFAULT_PADDING_MODE = PaddingMode.PKCS7;

        public int KeySize { get; set; }

        public string Key { get; set; }
        public string IV { get; set; }

        public CipherMode CipherMode { get; set; }
        public PaddingMode PaddingMode { get; set; }

        /// <summary>
        /// 原始字串
        /// </summary>
        public string SourceString { get; set; }
        /// <summary>
        /// 加密後字串
        /// </summary>
        public string EncryptString { get; private set; }

        public AES() : this(DEFAULT_KEYSIZE, DEFAULT_CIPHER_MODE, DEFAULT_PADDING_MODE) 
        { 
        }

        public AES(int keySize, CipherMode cipherMode, PaddingMode paddingMode)
        {
            KeySize = keySize;
            CipherMode = cipherMode;
            PaddingMode = paddingMode;
        }

        public AES(int keySize, string key, string iv, CipherMode cipherMode, PaddingMode paddingMode) : this(keySize, cipherMode, paddingMode)
        {
            Key = key;
            IV = iv;
        }

        /// <summary>
        /// 產生預設的AES
        /// </summary>
        /// <returns></returns>
        public AES Generate()
        {
            return Generate(DEFAULT_KEYSIZE, DEFAULT_CIPHER_MODE, DEFAULT_PADDING_MODE);
        }

        /// <summary>
        /// 按照設定產生AES
        /// </summary>
        /// <param name="keySize"></param>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <returns></returns>
        public AES Generate(int keySize, CipherMode cipherMode, PaddingMode paddingMode)
        {
            AES result = new AES(keySize, cipherMode, paddingMode);
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = keySize; 
                aes.Mode = cipherMode;
                aes.Padding = paddingMode;
                aes.GenerateKey();
                aes.GenerateIV();

                result.Key = Convert.ToBase64String(aes.Key);
                result.IV = Convert.ToBase64String(aes.IV);
            }
            return result;
        }

        /// <summary>
        /// 預設加密方式
        /// </summary>
        public void Encrypt()
        {
            EncryptString = Encrypt(SourceString, Key, IV, CipherMode, PaddingMode);
        }

        /// <summary>
        /// 按照設定提供Key iv 進行加密
        /// </summary>
        /// <param name="sourceString">原始字串(base64)</param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string Encrypt(string sourceString, string key, string iv)
        {
            return Encrypt(sourceString, key, iv, DEFAULT_CIPHER_MODE, DEFAULT_PADDING_MODE);
        }
        /// <summary>
        /// 按照設定提供Key iv 
        /// 與定義 cipherMode 與 paddingMode
        /// 進行加密
        /// </summary>
        /// <param name="sourceString">原始字串(base64)</param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <returns></returns>
        public static string Encrypt(string sourceString, string key, string iv, CipherMode cipherMode, PaddingMode paddingMode)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(sourceString), Convert.FromBase64String(key), Convert.FromBase64String(iv), cipherMode, paddingMode));
        }

        /// <summary>
        /// 使用byte[]的方式提供來源進行加密
        /// </summary>
        /// <param name="sourceBytes">原始資料Bytes格式</param>
        /// <param name="key">key byte格式</param>
        /// <param name="iv">iv byte格式</param>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] Encrypt(byte[] sourceBytes, byte[] key, byte[] iv, CipherMode cipherMode, PaddingMode paddingMode)
        {
            byte[] result = null;

            if (sourceBytes == null || sourceBytes.Length <= 0)
            {
                throw new ArgumentNullException("sourceBytes");
            }
            else
            {
                if (key == null || key.Length <= 0)
                {
                    throw new ArgumentNullException("key");
                }
                else
                {
                    if (iv == null || iv.Length <= 0)
                    {
                        throw new ArgumentNullException("iv");
                    }
                    else
                    {
                        // Create an Aes object
                        // with the specified key and IV.
                        using (Aes aes = Aes.Create())
                        {
                            aes.Key = key;
                            aes.IV = iv;
                            aes.Mode = cipherMode;
                            aes.Padding = paddingMode;

                            // Create the streams used for encryption.
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                                {
                                    cryptoStream.Write(sourceBytes, 0, sourceBytes.Length);
                                    cryptoStream.FlushFinalBlock();
                                    result = memoryStream.ToArray();
                                }
                            }
                        }
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return result;
        }

        /// <summary>
        /// 預設解密方式
        /// </summary>
        public void Decrypt()
        {
            SourceString = Decrypt(EncryptString, Key, IV, CipherMode, PaddingMode);
        }
        /// <summary>
        /// 按照設定提供Key iv 進行解密
        /// </summary>
        /// <param name="encryptString">加密字串</param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string Decrypt(string encryptString, string key, string iv)
        {
            return Decrypt(encryptString, key, iv, DEFAULT_CIPHER_MODE, DEFAULT_PADDING_MODE);
        }

        /// <summary>
        /// 按照設定提供Key iv 
        /// 與定義cipherMode、paddingMode 進行解密
        /// </summary>
        /// <param name="encryptString">加密字串</param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <returns></returns>
        public static string Decrypt(string encryptString, string key, string iv, CipherMode cipherMode, PaddingMode paddingMode)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(encryptString), Convert.FromBase64String(key), Convert.FromBase64String(iv), cipherMode, paddingMode));
        }
        /// <summary>
        /// 使用byte[]的方式提供來源進行解密
        /// </summary>
        /// <param name="encryptBytes"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] Decrypt(byte[] encryptBytes, byte[] key, byte[] iv, CipherMode cipherMode, PaddingMode paddingMode)
        {
            byte[] result = null;

            // Check arguments.
            if (encryptBytes == null || encryptBytes.Length <= 0)
            {
                throw new ArgumentNullException("encryptBytes");
            }
            else
            {
                if (key == null || key.Length <= 0)
                {
                    throw new ArgumentNullException("key");
                }
                else
                {
                    if (iv == null || iv.Length <= 0)
                    {
                        throw new ArgumentNullException("iv");
                    }
                    else
                    {
                        // Create an Aes object
                        // with the specified key and IV.
                        using (Aes aes = Aes.Create())
                        {
                            aes.Key = key;
                            aes.IV = iv;
                            aes.Mode = cipherMode;
                            aes.Padding = paddingMode;

                            // Create the streams used for decryption.
                            using (MemoryStream memoryStream = new MemoryStream(encryptBytes))
                            {
                                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                                {
                                    cryptoStream.Write(encryptBytes, 0, encryptBytes.Length);
                                    cryptoStream.FlushFinalBlock();
                                    result = memoryStream.ToArray();
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
