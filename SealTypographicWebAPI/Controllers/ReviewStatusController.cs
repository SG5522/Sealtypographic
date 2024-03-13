using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models.ReviewStatusList;
using SealTypographicWebAPI.Services.Implements;
using Serilog;


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
        public ReviewStatusResponse StatusList() => reviewStatusService.GetStatusList();    
    }
}
