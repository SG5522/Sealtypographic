using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.CustomerSeal;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.CustomerSealReview
{
    /// <summary>
    /// 客戶印鑑審核詳細資料
    /// </summary>
    public class CustomerSealGroupDetailReviewViewModel
    {
        /// <summary>
        /// new Seals
        /// </summary>
        public CustomerSealGroupDetailReviewViewModel() 
        {
            Seals = new();
        }

        /// <summary>
        /// 印鑑季度Id
        /// </summary>         
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// 客戶編號(更新或搜尋使用)
        /// </summary>
        /// <example>AAA001</example>        
        public string Code { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        /// <example>映像公司</example>        
        public string Name { get; set; }

        /// <summary>
        /// 顯示此筆季度
        /// </summary>         
        /// <example>1</example>
        public int QuarterYearId { get; set; }

        /// <summary>
        /// 客戶印鑑組
        /// </summary>
        public List<CustomerSealViewModel> Seals { get; set; }
    }

}
