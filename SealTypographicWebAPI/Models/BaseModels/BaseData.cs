using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 更新資料時會填入的資料
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
