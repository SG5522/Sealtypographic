using System.Collections.Generic;

namespace DJSharpZipLib
{
    public abstract class FileModel
    {
        /// <summary>
        /// 檔案路徑列表
        /// </summary>
        public List<string> FullPaths { get; set; }

        /// <summary>
        /// 檔案bytes
        /// </summary>
        public List<byte[]> Bytes { get; set; }


        public string Password { get; set; }
    }
}
