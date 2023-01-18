using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 更新客戶印鑑、會計師簽印、信頭圖片的基本資料
    /// </summary>
    public class BaseUpdateSeal : BaseData
    {
        /// <summary>
        /// 圖檔字串(Base64)
        /// </summary>
        /// <example>image/...</example>        
        [Required]
        public string ImageBase64 { get; set; }
    }
}
