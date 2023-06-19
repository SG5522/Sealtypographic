using DBEntities;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 排版資訊
    /// </summary>
    public class TypographicPageForm
    {
        /// <summary>
        /// new 
        /// </summary>
        public TypographicPageForm() 
        {
            CustomerSealLocations = new();
            AccountantSignLocations = new();
            LetterheadImageLocations = new();
            TemporarySealLocations = new();
        }

        /// <summary>
        /// 頁數
        /// </summary>        
        /// <example>1</example>
        public int PageNumber { get; set; }

        /// <summary>
        /// 確認是否需要插入空白頁
        /// </summary>
        /// <example>false</example>
        public bool BlankCheck { get; set; }

        /// <summary>
        /// 確認是否為刪除頁
        /// </summary>
        /// <example>false</example>
        public bool DeleteCheck { get; set; }

        /// <summary>
        /// 會計師證明書的ID
        /// </summary>
        /// <example>0</example>
        public int AccountantCertificateId { get; set; }
        
        /// <summary>
        /// 客戶印鑑位置
        /// </summary>        
        public List<CustomerSealLocationForm> CustomerSealLocations { get; set; }

        /// <summary>
        /// 會計師簽名印鑑位置
        /// </summary>
        public List<AccountantSignLocationForm> AccountantSignLocations { get; set; }

        /// <summary>
        /// 信頭圖片位置
        /// </summary>
        public List<LetterheadImageLocationForm> LetterheadImageLocations { get; set; }

        /// <summary>
        /// 臨時章
        /// </summary>
        public List<TemporarySealLocationForm> TemporarySealLocations { get; set;}
    }
}
