using DBEntities.Base;
using DBEntities.Consts;
using SealTypographicWebAPI.Models.Customer;

namespace SealTypographicWebAPI.Models.CustomerSeal
{
    /// <summary>
    /// 印鑑序號是否重複確認用
    /// </summary>
    public class CustomerSealSummry : CustomerSummary
    {
        /// <summary>
        /// 顯示最新季度Id
        /// </summary>        
        public int CustomerSealQuarterId { get; set; }

        /// <summary>
        /// 有無草稿狀態
        /// </summary>
        public bool IsDraff { get; set; }

        /// <summary>
        /// 有無待審狀態
        /// </summary>
        public bool IsPending { get; set; }

        /// <summary>
        /// 有無退件狀態
        /// </summary>
        public bool IsReject { get; set; }
    }
}
