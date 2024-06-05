using DBEntities.Consts;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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

        /// <summary>
        /// 簽印(會計師)以外類型的資料需使用順序
        /// </summary>
        [JsonIgnore]
        public int CommonSequence { get; set; }

        /// <summary>
        /// 印鑑、簽印類型 
        /// </summary>
        [JsonIgnore]
        public SealType SealType { get; set; }

        /// <summary>
        /// 印鑑、簽印子類別
        /// </summary>
        [JsonIgnore]
        public SubSealType SubSealType { get; set; }
    }
}
