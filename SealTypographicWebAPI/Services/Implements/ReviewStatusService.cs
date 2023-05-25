using Microsoft.Extensions.Localization;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Models.ReviewStatusList;
using DBEntities.Consts;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 審核狀態列表
    /// </summary>
    public class ReviewStatusService
    {
        private readonly IStringLocalizer<ReviewStatusService> localizer;

        /// <summary>
        /// IStringLocalizer
        /// </summary>
        /// <param name="localizer"></param>      
        public ReviewStatusService(IStringLocalizer<ReviewStatusService> localizer)
        {
            this.localizer = localizer;
        }

        /// <summary>
        /// 取得審核狀態列表
        /// </summary>
        /// <returns></returns>
        public ReviewStatusResponse GetStatusList()
        {
            ReviewStatusResponse reviewStatusResponse = new();
            foreach (ReviewStatus reviewStatus in (ReviewStatus[])Enum.GetValues(typeof(ReviewStatus)))
            {
                ReviewStatusViewModel reviewStatusViewModel = new()
                {
                    Id = (int)reviewStatus,
                    Name = localizer[reviewStatus.GetDescription()]
                };
                reviewStatusResponse.ViewModels.Add(reviewStatusViewModel);
            }
            reviewStatusResponse.Success();
            return reviewStatusResponse;
        }
    }
}
