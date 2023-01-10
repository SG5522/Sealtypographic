using System;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace DJLib
{
    public class SharpZipUtil
    {
        public static void MakeZipFile (byte[] byteArrayOut, string outPutName, string folderName, string password)
        {
            MemoryStream inputStream = new MemoryStream();            
            inputStream.Write(byteArrayOut, 0, byteArrayOut.Length);

            ZipOutputStream zipStream = new ZipOutputStream(inputStream);

            // 0-9, 9 being the highest level of compression
            zipStream.SetLevel(3);
            zipStream.Password = password;

            int folderOffset = folderName.Length + (folderName.EndsWith("\\") ? 0 : 1);

        }        
    }
}
