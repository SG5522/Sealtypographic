using System.Text.RegularExpressions;

namespace DJSpireNet6.Models
{
    /// <summary>
    /// PDF排版圖像與位置
    /// </summary>
    public class EditImage
    {
        private string imageBase64;
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

        /// <summary>
        /// 圖片縮放大小
        /// 1 inch = 72pt, and when dpi = 300, 1 inch = 300px. So when dpi = 300, 1px = 0.24pt    
        /// </summary>
        public float ImageScale { get; set; }

        /// <summary>
        /// 圖片流
        /// </summary>
        public Stream ImageStream { get; set; }
    }
}
