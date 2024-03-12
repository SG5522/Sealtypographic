using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Models.ImageRangeSetting;
using System.Runtime.CompilerServices;

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 圖片範圍設定
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ImagerRangeSettingController : APIControllerBase
    {
        private readonly IImageRangeSettingService imageRangeSettingService;

        /// <summary>
        /// 注入ImageRangeSettingService
        /// </summary>
        /// <param name="imageRangeSettingService"></param>
        /// <param name="applicationUserService"></param>
        public ImagerRangeSettingController(IImageRangeSettingService imageRangeSettingService, IApplicationUserService applicationUserService) : base(applicationUserService)
        {
            this.imageRangeSettingService = imageRangeSettingService;            
        }

        /// <summary>
        /// 取得客戶印鑑圖片範圍設定
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<CustomerSealRangeSettingResponse> CustomerSealRange() => await imageRangeSettingService.GetCustomerSealRangeSetting();

        /// <summary>
        /// 取得會計師簽印範圍設定
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public async Task<AccountantSignRangeSettingResponse> AccountantSignRange() => await imageRangeSettingService.GetAccountantSignRangeSetting();

        /// <summary>
        /// 更新客戶印鑑範圍設定
        /// </summary>        
        /// <param name="customerSealRangeSetting"></param>
        /// <returns></returns>
        [HttpPut("[Action]")]
        public async Task<ResponseViewModel> UpdateCustomerSealRange(CustomerSealRangeSetting customerSealRangeSetting)
            => await imageRangeSettingService.UpdateCustomerSealRangeSetting(customerSealRangeSetting, await GetUserId());

        /// <summary>
        /// 更新會計師簽印範圍設定
        /// </summary>        
        /// <param name="accountantSignRangeSetting"></param>
        /// <returns></returns>
        [HttpPut("[Action]")]
        public async Task<ResponseViewModel> UpdateAccountantSignRange(AccountantSignRangeSetting accountantSignRangeSetting) 
            => await imageRangeSettingService.UpdateAccountantSignRangeSetting(accountantSignRangeSetting, await GetUserId());

    }
}
