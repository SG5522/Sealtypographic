using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 各項目搜尋或更新時需要的基本資料
    /// </summary>
    public abstract class BaseData
    {
        /// <summary>
        /// ID
        /// </summary>        
        [Required]
        public int Id { get; set; }

    }
}
