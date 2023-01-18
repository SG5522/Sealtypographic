using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 新增客戶、會計師、會計師群組時填入的名稱
    /// </summary>
    public class BaseCreateName
    {
        /// <summary>
        /// 名稱
        /// </summary>
        /// <example>公司名稱 Or 名稱</example>
        [Required]
        public string Name { get; set; }
    }
}
