using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計資料
    /// </summary>
    public class AccountantFormUpdate : BaseName
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
        /// <example>0</example>        
        [Required]
        public int AccountantGroupId { get; set; }
    }
}
