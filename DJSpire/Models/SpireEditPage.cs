using Spire.Pdf.Graphics;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DJSpire.Models
{
    public class SpireEditPage
    {        
        private string imageBase64;

        /// <summary>
        /// 頁次
        /// </summary>
        public int PageNumber { get; set; }        
             
        /// <summary>
        /// ImageBase64字串
        /// </summary>
        public string ImageBase64
        {
            get { return imageBase64; }
            set
            {
                imageBase64 = value;
                if (imageBase64 != string.Empty)
                {
                    ImageStream = new MemoryStream(Convert.FromBase64String(Regex.Replace(ImageBase64, @"^data:image\/[a-zA-Z]+;base64,", string.Empty)));
                }
            }
        }

        /// <summary>
        /// 最左邊位置
        /// </summary>
        public float Left { get; set; }

        /// <summary>
        /// 頂部位置
        /// </summary>
        public float Top { get; set; }

        /// <summary>
        /// 寬
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        public int Height { get; set; }

        public Stream ImageStream { get; set; }
    }
}
