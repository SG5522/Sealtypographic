using SealTypographicWebAPI.Models.PublicModel;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計資料
    /// </summary>
    public class AccountantForm : BaseCreateName
    {
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
        /// <example>1</example>
        [Required]
        public int AccountantGroupId { get; set; }
    }
}
