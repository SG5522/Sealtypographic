using DBEntities.Consts;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 新增客戶印鑑、會計師簽印時所需要的base64圖片資料
    /// </summary>
    public class BaseCreateSeal
    {
        /// <summary>
        /// 圖檔字串(Base64)
        /// </summary>
        /// <example>image/...</example>
        [Required]
        public string ImageBase64 { get; set; }


    }
}
