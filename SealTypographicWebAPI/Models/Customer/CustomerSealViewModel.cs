using SealTypographicWebAPI.Models.PublicModel;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 印鑑資料(含群組名稱)
    /// </summary>
    public class CustomerSealViewModel : BaseSeal
    {
        /// <summary>
        /// 客戶ID
        /// </summary>
        /// <example>aaa001</example>
        [Required]
        public int CustomerId { get; set; } 

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
        /// 客戶印鑑組
        /// </summary>
        public List<CustomerSealViewModel> SealViewModels { get; set; }
    }
}
