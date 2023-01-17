using SealTypographicWebAPI.Consts;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 印鑑
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
