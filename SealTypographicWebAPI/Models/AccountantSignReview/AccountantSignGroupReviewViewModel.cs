using DBEntities.Consts;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.AccountantSignReview
{
    /// <summary>
    /// 會計師簽印審核檢視
    /// </summary>
    public class AccountantSignGroupReviewViewModel
    {
        /// <summary>
        /// New SealImageInfos
        /// </summary>
        public AccountantSignGroupReviewViewModel()
        {
            SignImageInfos = new();
        }

        /// <summary>
        /// 會計師簽印群組Id
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        /// <example>王XX</example>
        public string Name { get; set; }

        /// <summary>
        /// 群組名稱
        /// </summary>
        /// <example>台北群組</example>
        public string GroupName { get; set; }

        /// <summary>
        /// 編號
        /// </summary>
        /// <example>ACC001</example>
        public string Code { get; set; }

        /// <summary>
        /// 審核狀態
        /// 請參考 /api/ReviewStatus 的內容
        /// </summary>
        /// <example>0</example>
        public ReviewStatus ReviewStatus { get; set; }

        //--------------log Save Data-------------//
        /// <summary>
        /// 會計師ID
        /// </summary>
        [JsonIgnore]
        public int AccountantId { get; set; }

        /// <summary>
        /// 會計簽印群組建立日期
        /// </summary>
        [JsonIgnore]
        public DateTime GroupCreateDate { get; set; }
        //--------------log Save Data-------------//

        /// <summary>
        /// 簽印組
        /// </summary>
        public List<SignImageInfo> SignImageInfos { get; set; }
    }

    /// <summary>
    /// 簽印資訊
    /// </summary>
    public class SignImageInfo
    {
        /// <summary>
        /// 簽印類別
        /// 請參考 /api/SealMappingConfig?sealType=2 的內容
        /// </summary>
        /// <example>1</example>
        public AccountantSignType SealMappingConfigId { get; set; }

        /// <summary>
        /// 縮圖字串(Base64)
        /// </summary>
        /// <example>image/...</example>
        public string ThumbnailBase64 { get; set; }
    }
}
