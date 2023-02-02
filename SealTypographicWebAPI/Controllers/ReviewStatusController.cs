using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models.ReviewStatusList;
using SealTypographicWebAPI.Services.Implements;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 取得審核狀態
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewStatusController : ControllerBase
    {
        /// <summary>
        /// 取得印鑑類型列表的service
        /// </summary>
        private readonly ReviewStatusService reviewStatusService;

        /// <summary>
        /// 建構:注入Service
        /// </summary>
        /// <param name="reviewStatusService">取得印鑑類型列表的service</param>        
        public ReviewStatusController(ReviewStatusService reviewStatusService)
        {
            this.reviewStatusService = reviewStatusService;
        }

        /// <summary>
        /// 取得審核狀態列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ReviewStatusResponse StatusList()
        {
            ReviewStatusResponse reviewStatusResponse = new();
            try
            {                
                reviewStatusResponse = reviewStatusService.GetStatusList();
                Log.Information("ReviewStatusResponse StatusList output {@Output}", reviewStatusResponse);
            }
            catch (Exception ex)
            {
                Log.Error("ReviewStatusResponse StatusList error {@Error}", ex);
                reviewStatusResponse.DbError();
            }
            return reviewStatusResponse;
        }        
    }
}
