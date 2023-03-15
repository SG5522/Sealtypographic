using System;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Encryption;
using ICSharpCode.SharpZipLib.Core;

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
            using (FileStream fs = File.Create(outfile))

            using (ZipOutputStream outStream = new ZipOutputStream(fs))
            {
                outStream.Password = password;                
                outStream.PutNextEntry(new ZipEntry("data.bin"));                
                outStream.Write(inputBytes, 0, inputBytes.Length);                
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
            using (FileStream fsInput = File.OpenRead(filePath))
            using (ZipFile zipInputStream = new ZipFile(fsInput))
            {
                
               
            }
            return base64;
        }
    }
}
