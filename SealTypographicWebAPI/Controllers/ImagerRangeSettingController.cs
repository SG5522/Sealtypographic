using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Models.ImageRangeSetting;

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 圖片範圍設定
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ImagerRangeSettingController : ControllerBase
    {
        private readonly IImageRangeSettingService imageRangeSettingService;

        /// <summary>
        /// 注入ImageRangeSettingService
        /// </summary>
        /// <param name="imageRangeSettingService"></param>
        public ImagerRangeSettingController(IImageRangeSettingService imageRangeSettingService)
        {
            this.imageRangeSettingService = imageRangeSettingService;            
        }

        /// <summary>
        /// 取得客戶印鑑圖片範圍設定
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public CustomerSealRangeSettingResponse CustomerSealRange() => imageRangeSettingService.GetCustomerSealRangeSetting();

        /// <summary>
        /// 取得會計師簽印範圍設定
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public AccountantSignRangeSettingResponse AccountantSignRange() => imageRangeSettingService.GetAccountantSignRangeSetting();

        /// <summary>
        /// 更新客戶印鑑範圍設定
        /// </summary>        
        /// <param name="customerSealRangeSetting"></param>
        /// <returns></returns>
        [HttpPut("[Action]")]
        public ResponseViewModel UpdateCustomerSealRange(CustomerSealRangeSetting customerSealRangeSetting)
            => imageRangeSettingService.UpdateCustomerSealRangeSetting(customerSealRangeSetting);

        /// <summary>
        /// 更新會計師簽印範圍設定
        /// </summary>        
        /// <param name="accountantSignRangeSetting"></param>
        /// <returns></returns>
        [HttpPut("[Action]")]
        public ResponseViewModel UpdateAccountantSignRange(AccountantSignRangeSetting accountantSignRangeSetting) 
            => imageRangeSettingService.UpdateAccountantSignRangeSetting(accountantSignRangeSetting);

    }
}
