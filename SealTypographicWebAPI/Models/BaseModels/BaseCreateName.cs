using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 新增更新資料時會填入的資料
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
