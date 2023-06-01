using System;
using System.Collections.Generic;
using System.IO;
using SevenZip;
using SevenZip.Compression.LZMA;

namespace DJSharpZipLib
{
    /// <summary>
    /// UNDONE 7ZIP目前無法正常壓縮
    /// </summary>
    public class DJ7Zip
    {
        // 設定壓縮器的壓縮等級
        private const int PosStateBits = 2; // default: 2
        private const int LitContextBits = 3; // 3 for normal files, 0; for 32-bit data
        private const int LitPosBits = 0; // 0 for 64-bit data, 2 for 32-bit.        
        private const int NumFastBytes = 128;
        private const string MatchFinder = "BT4"; // default: BT4
        private const bool EndMarker = false;

        /// <summary>
        /// 輸出壓縮後的檔案
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static byte[] Compress(Dictionary<string, byte[]> files)
        {
            using (MemoryStream outputStream = new MemoryStream())
            {
                Encoder encoder = new Encoder();
                encoder.WriteCoderProperties(outputStream);
                outputStream.Write(BitConverter.GetBytes(files.Count), 0, 4);
                //encoder.SetCoderProperties(GetProIds(), GetProperties());

                foreach (KeyValuePair<string, byte[]> file in files)
                {
                    // 寫入檔案名稱
                    byte[] fileNameBytes = System.Text.Encoding.UTF8.GetBytes(file.Key);
                    outputStream.Write(BitConverter.GetBytes(fileNameBytes.Length), 0, 4);
                    outputStream.Write(fileNameBytes, 0, fileNameBytes.Length);
                    // 寫入檔案內容
                    outputStream.Write(BitConverter.GetBytes((Int64)file.Value.Length), 0, 8);
                    encoder.Code(new MemoryStream(file.Value), outputStream, file.Value.Length, -1, null);
                    //壓縮檔案
                    //using (MemoryStream inputFileStream = new MemoryStream(file.Value))
                    //{
                    //    encoder.Code(inputFileStream, outputStream, inputFileStream.Length, -1, null);
                    //}
                }
                return outputStream.ToArray();
            }
        }

        private static CoderPropID[] GetProIds()
        {
            CoderPropID[] propIDs =
            {
                CoderPropID.DictionarySize,
                CoderPropID.PosStateBits, // (0 <= x <= 4).
                CoderPropID.LitContextBits, // (0 <= x <= 8).                
                CoderPropID.LitPosBits, // (0 <= x <= 4).
                CoderPropID.NumFastBytes,
                CoderPropID.MatchFinder, // "BT2", "BT4".
                CoderPropID.EndMarker
            };
            return propIDs;
        }

        private static object[] GetProperties()
        {
            object[] properties =
            {
                (int)22, // 設定字典大小為 2^22
                PosStateBits,
                LitContextBits,
                LitPosBits,
                NumFastBytes,
                MatchFinder,
                EndMarker
            };
            return properties;
        }
    }
}
