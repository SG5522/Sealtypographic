using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶印鑑
    /// </summary>
    public class CustomerSealForm : BaseCreateSeal
    {
        /// <summary>
        /// 客戶印鑑群組ID 
        /// 1.公司章
        /// 2.負責人
        /// 3.經理
        /// 4.會計主管    
        /// 5.其他(客戶)
        /// </summary>
        /// <example>1</example>
        [Required]        
        public CustomerSealConfigType SealMappingConfigId { get; set; }

        /// <summary>
        /// 印鑑編號(排序) 1為起始
        /// </summary>
        /// <example>1</example>
        [Required]
        [Range(1, 99)]
        public int Sequence { get; set; }
    }

    /// <summary>
    /// 客戶印鑑組
    /// </summary>
    public class CustomerSealForms
    {
        /// <summary>
        /// 客戶ID
        /// </summary>
        /// <example>1</example>
        [Required]
        public int CustomerId { get; set; }

        /// <summary>
        /// 印鑑季度
        /// </summary>
        /// <example>111Q1</example>
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        public string Quarter { get; set; }

        /// <summary>
        /// 客戶印鑑
        /// </summary>
        public List<CustomerSealForm> SealForms { get; set; }
    }
}
