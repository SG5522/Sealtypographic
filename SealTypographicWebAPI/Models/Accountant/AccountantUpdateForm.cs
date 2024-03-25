using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師資料
    /// </summary>
    public class AccountantUpdateForm : BaseData
    {
        /// <summary>
        /// 姓名
        /// </summary>
        /// <example>王XX</example>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 會計師編號
        /// </summary>   
        /// <example>ACC001</example>
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]        
        public string AccountantNumber { get; set; }

        /// <summary>
        /// 會計師群組ID
        /// 0 無群組
        /// </summary> 
        [Required]
        public IList<int> AccountantGroupIds { get; set; }
    }
}
