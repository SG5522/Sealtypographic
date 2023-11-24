using System;
using System.Collections.Generic;
using System.Text;

namespace DJSpireNet6.Models
{
    /// <summary>
    /// PDF圖片資訊
    /// </summary>
    public class PDFImageInfo
    {
        /// <summary>
        /// 圖片寬度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 圖片高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Imagebase64
        /// </summary>
        public string ImageBase64 { get; set; }
    }
}
