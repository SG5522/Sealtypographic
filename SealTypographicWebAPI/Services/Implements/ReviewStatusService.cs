using Microsoft.Extensions.Localization;
using SealTypographicWebAPI.Models.ReviewStatusList;
using DBEntities.Consts;
using CommonLib.Extensions;
using Serilog;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 審核狀態列表
    /// </summary>
    public class ReviewStatusService
    {
        private readonly IStringLocalizer<ReviewStatusService> localizer;
        private readonly ILogger<ReviewStatusService> logger;

        /// <summary>
        /// IStringLocalizer
        /// </summary>
        /// <param name="localizer"></param>
        /// <param name="logger"></param>      
        public ReviewStatusService(IStringLocalizer<ReviewStatusService> localizer, ILogger<ReviewStatusService> logger)
        {
            this.localizer = localizer;
            this.logger = logger;
        }

        /// <summary>
        /// 取得審核狀態列表
        /// </summary>
        /// <returns></returns>
        public ReviewStatusResponse GetStatusList()
        {
            ReviewStatusResponse reviewStatusResponse = new();

            try
            {
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
                logger.LogInformation("GetStatusList output {@Output}", reviewStatusResponse);
            }
            catch (Exception ex)
            {
                logger.LogError("GetStatusList error {@Error}", ex.Message);
                reviewStatusResponse.DbError();
            }
            return reviewStatusResponse;
        }
    }
}
