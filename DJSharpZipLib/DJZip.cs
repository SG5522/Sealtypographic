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
    public class DJGZip
    {
        /// <summary>
        /// 壓縮
        /// </summary>
        /// <param name="inputBytes"></param>
        /// <param name="outfile"></param>
        public static byte[] CompressBytes(byte[] inputBytes, string outfile)
        {
            byte[] compressedData = new byte[4096];            
            using (MemoryStream compressedDataStream = new MemoryStream())
            using (FileStream fileStream = File.Create(outfile))
            using (GZipOutputStream gzipOutputStream = new GZipOutputStream(compressedDataStream))
            {
                gzipOutputStream.SetLevel(9);
                gzipOutputStream.Write(inputBytes, 0, inputBytes.Length);
                gzipOutputStream.Flush();
                compressedData = compressedDataStream.ToArray();
            }            
            File.WriteAllBytes(outfile, compressedData);
            return compressedData;
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
