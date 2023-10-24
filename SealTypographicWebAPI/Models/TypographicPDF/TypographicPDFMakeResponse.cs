using DJLib.Models;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 排版PDF + 回應訊息
    /// </summary>
    public class TypographicPDFMakeResponse : ResponseViewModel
    {
        private string pDFBase64;

        /// <summary>
        /// PDF圖檔(base64)
        /// </summary>
        public string PDFBase64
        {
            get { return pDFBase64; }
            set
            {
                pDFBase64 = value;
                if (pDFBase64 != string.Empty)
                {
                    ImageInfo imageInfo = ImageInfo.FromImageBase64(pDFBase64);
                    imageInfo.ReSize(imageInfo, 0.1);
                    ThumbnailPDFBase6 = imageInfo.ToBase64();
                }
            }
        }

        /// <summary>
        /// PDF縮圖(base64)
        /// </summary>
        [JsonIgnore]
        public string ThumbnailPDFBase6 { get; set; }
    }
}
