using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.ReviewStatusList
{
    /// <summary>
    /// 審核狀態顯示
    /// </summary>
    public class ReviewStatusViewModel : BaseData
    {
        /// <summary>
        /// 審核狀態名稱
        /// </summary>
        /// <example></example>
        public string? Name { get; set; }
    }
}
