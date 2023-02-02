using SealTypographicWebAPI.Models.BaseModels;
using SealTypographicWebAPI.Models.SealMappingConfig;

namespace SealTypographicWebAPI.Models.ReviewStatusList
{
    /// <summary>
    /// 審核狀態列表
    /// </summary>
    public class ReviewStatusResponse : ResponseViewModel
    {
        /// <summary>
        /// new SealMappingConfigViewModel
        /// </summary>
        public ReviewStatusResponse()
        {
            ViewModels = new();
        }

        /// <summary>
        /// 審核狀態顯示
        /// </summary>
        public List<ReviewStatusViewModel> ViewModels { get; set; }
    }
}
