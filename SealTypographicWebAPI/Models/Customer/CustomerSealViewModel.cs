using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 印鑑資料(含群組名稱)
    /// </summary>
    public class CustomerSealViewModel
    {
        /// <summary>
        /// 客戶印鑑組ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 客戶ID
        /// </summary>
        /// <example>aaa001</example>
        [Required]
        public string CustomerId { get; set; } = null!;

        /// <summary>
        /// 客戶印鑑群組名稱
        /// 1.公司章
        /// 2.負責人
        /// 3.經理
        /// 4.會計主管   
        /// </summary>        
        public string SealMappingConfigName { get; set; }

        /// <summary>
        /// 印鑑編號(排序) 1為起始
        /// </summary>
        /// <example>1</example>
        public int Sequence { get; set; }

        /// <summary>
        /// 圖檔字串(Base64)
        /// </summary>
        /// <example>image/...</example>
        public string ImageBase64 { get; set; }

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
