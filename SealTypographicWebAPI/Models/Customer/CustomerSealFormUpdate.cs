using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 印鑑組資料(含ID)
    /// </summary>
    public class CustomerSealFormUpdate : BaseUpdateSeal
    {
        /// <summary>
        /// 印鑑編號(排序) 1為起始
        /// </summary>
        /// <example>1</example>        
        [Required]
        [Range(1, 99)]
        public int Sequence { get; set; }
    }
}
