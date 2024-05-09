using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using System.IO;

namespace DJSharpZipLib
{
    public class DJZip : FileModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDataPaths">key檔名 value bytes資料</param>
        /// <returns></returns>
        public static byte[] Compress(Dictionary<string, string> sourceDataPaths)
        {
            Dictionary<string, byte[]> sourceDatas = new();

            foreach (KeyValuePair<string, string> sourceData in sourceDataPaths)
            {
                sourceDatas.Add(sourceData.Key, File.ReadAllBytes(sourceData.Value));
            }
            return Compress(sourceDatas);
        }



        /// <summary>
        /// 壓縮
        /// </summary>
        /// <param name="sourceDatas">key檔名 value bytes資料</param>
        /// <returns></returns>
        public static byte[] Compress(Dictionary<string, byte[]> sourceDatas, string pwaosrsd = null)
        {
            using MemoryStream memoryStream = new();
            using ZipOutputStream zipOutputStream = new(memoryStream);
            zipOutputStream.SetLevel(9); // 設定壓縮等級，1~9，9為最高等級
            if(!string.IsNullOrWhiteSpace(pwaosrsd)) zipOutputStream.Password = pwaosrsd;
            foreach (KeyValuePair<string, byte[]> sourceData in sourceDatas)
            {
                ZipEntry entry = new(sourceData.Key);
                zipOutputStream.PutNextEntry(entry);
                zipOutputStream.Write(sourceData.Value, 0, sourceData.Value.Length);
                zipOutputStream.CloseEntry();
            }            
            return memoryStream.ToArray();
        }

        /// <summary>
        /// 解壓縮
        /// </summary>
        /// <param name="srcBytes">資料源</param>
        /// <returns></returns>
        public static Dictionary<string, byte[]> Decompress(byte[] srcBytes, string pwaosrsd = null)
        {
            Dictionary<string, byte[]> result = new();
            using (MemoryStream compressedStream = new(srcBytes))
            {
                using ZipInputStream zipInputStream = new(compressedStream);
                ZipEntry entry;
                while ((entry = zipInputStream.GetNextEntry()) != null)
                {
                    byte[] buffer = new byte[entry.Size];
                    int offset = 0;
                    while (offset < buffer.Length)
                    {
                        var bytesRead = zipInputStream.Read(buffer, offset, buffer.Length - offset);
                        if (bytesRead == 0)
                        {
                            break;
                        }
                        offset += bytesRead;
                    }
                    // 將解壓縮後的資料存儲到字典中，使用 entry 的名稱作為 key
                    result[entry.Name] = buffer;
                }
            }
            return result;
        }
    }
}
