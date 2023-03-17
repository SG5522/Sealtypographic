using System;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Encryption;
using ICSharpCode.SharpZipLib.Core;
using System.Security.Cryptography;
using ICSharpCode.SharpZipLib.BZip2;

namespace DJSharpZipLib
{
    public class DJZip
    {
        /// <summary>
        /// 壓縮
        /// </summary>
        /// <param name="inputBytes"></param>
        /// <param name="outfile"></param>
        public static void CompressBytes(byte[] inputBytes, string password , string outfile)
        {
            byte[] compressedData = new byte[4096];
            using (FileStream fileStream = File.Create(outfile))
            using (MemoryStream compressedDataStream = new MemoryStream())
            using (GZipOutputStream gzipOutputStream = new GZipOutputStream(compressedDataStream))
            {
                gzipOutputStream.SetLevel(9);
                gzipOutputStream.Write(inputBytes, 0, inputBytes.Length);
                gzipOutputStream.Flush();
                compressedData = compressedDataStream.ToArray();
                if (!string.IsNullOrEmpty(password))
                {
                    EncrypteProcesss(compressedData, password);
                }

            }


            File.WriteAllBytes(outfile, compressedData);
        }

        public static void EncrypteProcesss(byte[] inputBytes, string password)
        {
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Derive key and IV from password using PBKDF2
                byte[] salt = new byte[16];
                new Random().NextBytes(salt);
                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
                aes.Key = pbkdf2.GetBytes(32);
                aes.IV = pbkdf2.GetBytes(16);

                // Encrypt the input data using AES
                using (MemoryStream inputStream = new MemoryStream(inputBytes))
                using (MemoryStream outputStream = new MemoryStream())
                using (CryptoStream cryptoStream = new CryptoStream(outputStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    do
                    {
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        cryptoStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead > 0);

                    inputBytes = outputStream.ToArray();
                }
            }
        }

        /// <summary>
        /// 解壓縮
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string UnCompressToBase64(string filePath, string password)
        {
            string base64 = string.Empty;
            using (FileStream fileStream = File.OpenRead(filePath))
            using (ZipFile zipFile = new ZipFile(fileStream))
            {
                if(!string.IsNullOrEmpty(password))
                {
                    zipFile.Password = password;
                }                               
            }
            return base64;
        }
    }
}
