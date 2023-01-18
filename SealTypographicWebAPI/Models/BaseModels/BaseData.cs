using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 各項目搜尋或更新時需要的基本資料
    /// </summary>
    public class BaseData
    {
        /// <summary>
        /// ID
        /// </summary>
        /// <example>0</example>        
        [Required]
        public int Id { get; set; }

    }
}
