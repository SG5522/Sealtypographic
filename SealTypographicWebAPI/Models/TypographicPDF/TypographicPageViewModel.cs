using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 單頁排板資訊
    /// </summary>
    public class TypographicPageViewModel : ResponseViewModel
    {
        /// <summary>
        /// NEW
        /// </summary>
        public TypographicPageViewModel()
        {
            CustomerSealLocationViewModels = new();
            AccountantSignLocationViewModels = new();
            LetterheadImageLocationViewModels = new();
            TemporarySealLocationViewModels = new();
        }

        /// <summary>
        /// 頁次Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 頁數
        /// </summary>        
        public int PageNumber { get; set; }

        /// <summary>
        /// PDF圖片寬度
        /// </summary>
        public int PDFImageWidth { get; set; }

        /// <summary>
        /// PDF圖片高度
        /// </summary>
        public int PDFImageHeight { get; set; }

        /// <summary>
        /// PDF單頁圖片
        /// </summary>
        public string PDFImageBase64 { get; set; }

        /// <summary>
        /// PDF檔案路徑
        /// </summary>
        [JsonIgnore]
        public string PDFFullPath { get; set; }

        /// <summary>
        /// PDF檔案Key
        /// </summary>
        [JsonIgnore]
        public string EncryptKey { get; set; }

        /// <summary>
        /// RSAKEY
        /// </summary>
        [JsonIgnore]
        public RSAKey RSAKey { get; set; }

        /// <summary>
        /// 客戶印鑑位置
        /// </summary>        
        public List<CustomerSealLocationViewModel> CustomerSealLocationViewModels { get; set; }

        /// <summary>
        /// 會計師簽名印鑑位置
        /// </summary>
        public List<AccountantSignLocationViewModel> AccountantSignLocationViewModels { get; set; }

        /// <summary>
        /// 信頭圖片位置
        /// </summary>
        public List<LetterheadImageLocationViewModel> LetterheadImageLocationViewModels { get; set; }

        /// <summary>
        /// 臨時章位置
        /// </summary>
        public List<TemporarySealLocationViewModel> TemporarySealLocationViewModels { get; set;}
    }    
}
