using CommonLib.Extensions;
using DJImageLib.Extensions;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.EditPdf
{
    /// <summary>
    /// PDF排版圖像與位置
    /// </summary>
    public class EditImage : BaseSeal
    {
        private string imageBase64;
        /// <summary>
        /// ImageBase64字串
        /// </summary>
        public override string ImageBase64
        {
            get { return imageBase64; }
            set
            {                
                if (value != string.Empty)
                {
                    imageBase64 = value;
                    byte[]? bytes = imageBase64.FromDataUrlToBytes();
                    if (bytes != null)
                    {
                        ImageStream = new MemoryStream(bytes);
                    }                        
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

        ///// <summary>
        ///// 圖片縮放大小
        ///// 1 inch = 72pt, and when dpi = 300, 1 inch = 300px. So when dpi = 300, 1px = 0.24pt    
        ///// </summary>
        //public float ImageScale { get; set; }

        /// <summary>
        /// 圖片流
        /// </summary>
        public Stream ImageStream { get; set; }
    }
}
