using DJImageLib.Extensions;
using DJImageLib.Utils;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 回傳排版後PDFBase64
    /// </summary>
    public class TypographicPDFEditViewResponse : ResponseViewModel
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
                    ThumbnailPDFBase64  = ImageUtil.ReSize(DataUrlUtil.GetBase64(pDFBase64).ToBytes(), 0.1, 0.1); ;
                }
            }
        }

        /// <summary>
        /// PDF縮圖(base64)
        /// </summary>
        [JsonIgnore]
        public string ThumbnailPDFBase64 { get; set; }
    }
}
