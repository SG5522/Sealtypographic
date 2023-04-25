using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace DJEncryption
{
    public class AESEncryption
    {
        public byte[] EncryptionData { get; set; }
        public string SaltString { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// 使用純密碼的方式加密資料。
        /// </summary>
        /// <param name="inputBytes">輸入的Byte[]</param>
        /// <param name="password">密碼</param>
        public static AESEncryption Encrypte(byte[] inputBytes, int passwordLength)
        {
            AESEncryption encryptionWithAES = new AESEncryption();
            RijndaelManaged rijndaelManaged = new RijndaelManaged();

            byte[] salt = GenerateRandomSalt();
            //convert password string to byte arrray
            string password = GenerateRandomPassword(passwordLength);

            Setting(rijndaelManaged, salt, password);
            
            // Encrypt the input data using AES
            using (MemoryStream inputStream = new MemoryStream(inputBytes))
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(outputStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] buffer = new byte[1048576];
                    int bytesRead;
                    do
                    {
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        cryptoStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead > 0);

                    //inputBytes = outputStream.ToArray();                
                }
                encryptionWithAES.EncryptionData = outputStream.ToArray();
                encryptionWithAES.SaltString = Convert.ToBase64String(salt);
                encryptionWithAES.Password = password;
            }
            return encryptionWithAES;
        }
        /// <summary>
        /// Decrypts an encrypted file with the FileEncrypt method through its path and the plain password.
        /// </summary>
        /// <param name="inputFile"></param>       
        /// <param name="password"></param>
        public static byte[] Decrypt(AESEncryption encryptionWithAES)
        {            
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            byte[] salt = Convert.FromBase64String(encryptionWithAES.SaltString);

            Setting(rijndaelManaged, salt, encryptionWithAES.Password);

            // Encrypt the input data using AES
            using (MemoryStream inputStream = new MemoryStream(encryptionWithAES.EncryptionData))
            using (MemoryStream outputStream = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream(inputStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Read))
            {
                byte[] buffer = new byte[1048576];
                int bytesRead;
                do
                {
                    bytesRead = cryptoStream.Read(buffer, 0, buffer.Length);
                    outputStream.Write(buffer, 0, bytesRead);
                } while (bytesRead > 0);
                return outputStream.ToArray();
            }            
        }

        private static byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[32];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    // Fille the buffer with the generated data
                    rng.GetBytes(data);
                }
            }
            return data;
        }

        private static string GenerateRandomPassword(int length) 
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"; // 可以使用的字符
            byte[] randomBytes = new byte[length];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }
            StringBuilder passwordBuilder = new StringBuilder(length);
            foreach (byte b in randomBytes)
            {
                passwordBuilder.Append(validChars[b % validChars.Length]);
            }
            return passwordBuilder.ToString();
        }

        /// <summary>
        /// 設定加密方式
        /// </summary>
        /// <param name="aes"></param>
        /// <param name="salt"></param>
        /// <param name="password"></param>
        private static void Setting(RijndaelManaged aes, byte[] salt ,string password)
        {
            aes.KeySize = 256;
            aes.BlockSize = 128;                        
            // Derive key and IV from password using PBKDF2
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, 50000);
            aes.Key = pbkdf2.GetBytes(aes.KeySize / 8);
            aes.IV = pbkdf2.GetBytes(aes.BlockSize / 8);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;            
        }
    }
}
