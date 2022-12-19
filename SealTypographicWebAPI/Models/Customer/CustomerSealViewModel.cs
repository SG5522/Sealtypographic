using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 印鑑資料(含群組名稱)
    /// </summary>
    public class CustomerSealViewModel : BaseSeal
    {
        /// <summary>
        /// 印鑑編號(排序) 1為起始
        /// </summary>
        /// <example>1</example>
        public int Sequence { get; set; }

    }

    /// <summary>
    /// 印鑑組
    /// </summary>
    public class CustomerSealViewModels : ResponseViewModel
    {
        /// <summary>
        /// 客戶ID
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 印鑑季度
        /// </summary>
        public string Quarter { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        public string ReviewStatus { get; set; }

        /// <summary>
        /// 客戶印鑑組
        /// </summary>
        public List<CustomerSealViewModel> SealViewModels { get; set; }
    }
}
