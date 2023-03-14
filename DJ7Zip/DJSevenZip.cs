using System;
using System.IO;
using DJ7Zip.Consts;
using SevenZip;
using SevenZip.Compression.LZMA;

namespace DJ7Zip
{
    public class DjSevenZip
    {
        public static void CompressBytes(byte[] data)
        {
            string path = Path.Combine($"D:/","test.7z");
            using (MemoryStream inStream = new MemoryStream(data))
            {
                using (FileStream outStream = File.Create(path))
                {
                    var encoder = new Encoder();
                    encoder.WriteCoderProperties(outStream);
                    long streamSize = inStream.Length;
                    for (int i = 0; i < 8; i++)
                    {
                        outStream.WriteByte((byte)(streamSize >> (8 * i)));
                    }

                    encoder.Code(inStream, outStream, -1, -1, null);
                    //long filesize = inStream.Length - inStream.Position;
                }
            }
        }
    }
}
