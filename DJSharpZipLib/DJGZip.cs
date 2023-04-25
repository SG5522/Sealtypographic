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
        public static byte[] CompressBytes(byte[] inputBytes)
        {            
            using (MemoryStream outputMemoryStream = new MemoryStream())
            {
                using (GZipOutputStream gzipOutputStream = new GZipOutputStream(outputMemoryStream ))
                {
                    gzipOutputStream.SetLevel(9);
                    gzipOutputStream.Write(inputBytes, 0, inputBytes.Length);
                }
                return outputMemoryStream .ToArray();
            }
        }
        
        /// <summary>
        /// 解壓縮
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static byte[] DecompressBytes(byte[] inputBytes)
        {
            using (var inputMemoryStream = new MemoryStream(inputBytes))
            {
                using (var gzipStream = new GZipInputStream(inputMemoryStream))
                {
                    using (var outputMemoryStream = new MemoryStream())
                    {
                        var buffer = new byte[4096];
                        int read;
                        while ((read = gzipStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            outputMemoryStream.Write(buffer, 0, read);
                        }
                        return outputMemoryStream.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// 透過路徑壓縮檔案並輸出
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destinationFile"></param>
        public static void CompressToFile(string sourceFile, string destinationFile)
        {
            using (var inputStream = new FileStream(sourceFile, FileMode.Open))
            {
                using (var outputStream = new FileStream(destinationFile, FileMode.Create))
                {
                    using (var gzipStream = new GZipOutputStream(outputStream))
                    {
                        var buffer = new byte[4096];
                        int read;
                        while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            gzipStream.Write(buffer, 0, read);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 透過路徑解縮檔案
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destinationFile"></param>
        public static void DecompressToFile(string sourceFile, string destinationFile)
        {
            using (var inputStream = new FileStream(sourceFile, FileMode.Open))
            {
                using (var gzipStream = new GZipInputStream(inputStream))
                {
                    using (var outputStream = new FileStream(destinationFile, FileMode.Create))
                    {
                        var buffer = new byte[4096];
                        int read;
                        while ((read = gzipStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            outputStream.Write(buffer, 0, read);
                        }
                    }
                }
            }
        }
    }
}
