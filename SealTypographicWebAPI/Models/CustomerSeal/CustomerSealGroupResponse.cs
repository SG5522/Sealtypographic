using DBEntities.Consts;

namespace SealTypographicWebAPI.Models.CustomerSeal
{
    /// <summary>
    /// 印鑑群組簡易資訊
    /// </summary>
    public class CustomerSealGroupResponse : ResponseViewModel
    {
        /// <summary>
        /// 印鑑群組Id
        /// </summary>
        /// <example>1</example>
        public int CustomerSealGroupId { get; set; }

        /// <summary>
        /// 印鑑季度(年度)
        /// </summary>
        /// <example>111Q1</example>
        public string Quarter { get; set; }
    }
}
