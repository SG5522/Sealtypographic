using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.PublicModel
{
    /// <summary>
    /// 印鑑
    /// </summary>
    public class BaseUpdateSeal : BaseUpdateData
    {
        /// <summary>
        /// 圖檔字串(Base64)
        /// </summary>
        /// <example>image/...</example>        
        [Required]
        public string ImageBase64 { get; set; }
    }
}
