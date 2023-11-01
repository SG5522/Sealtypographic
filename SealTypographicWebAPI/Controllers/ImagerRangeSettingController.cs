using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using DBEntities.Consts;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Models.ImageRangeSetting;

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 上傳
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
        /// 取得客戶印鑑分離截取設定
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public CustomerSealRangeSettingResponse CustomerSealRange() => imageRangeSettingService.GetCustomerSealRangeSetting();

        /// <summary>
        /// 取得客戶印鑑分離截取設定
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public AccountantSignRangeSettingResponse AccountantSignRange() => imageRangeSettingService.GetAccountantSignRangeSetting();

        /// <summary>
        /// 新增客戶印鑑分離截取設定
        /// </summary>        
        /// <param name="customerSealRangeSetting">客戶印鑑截取設定</param>
        /// <returns></returns>
        [HttpPost("[Action]")]
        public ResponseViewModel NewCustomerSealRange(CustomerSealRangeSetting customerSealRangeSetting) => imageRangeSettingService.New(customerSealRangeSetting);

        /// <summary>
        /// 新增客戶印鑑分離截取設定
        /// </summary>        
        /// <param name="accountantSignRangeSetting">客戶印鑑截取設定</param>
        /// <returns></returns>
        [HttpPost("[Action]")]
        public ResponseViewModel NewAccountantSignRange(AccountantSignRangeSetting accountantSignRangeSetting) => imageRangeSettingService.New(accountantSignRangeSetting);

        /// <summary>
        /// 更新客戶印鑑截取設定
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customerSealRangeSetting"></param>
        /// <returns></returns>
        [HttpPut("[Action]")]
        public ResponseViewModel UpdateCustomerSealRange(int id, CustomerSealRangeSetting customerSealRangeSetting)
            => imageRangeSettingService.Update(id, customerSealRangeSetting, SealType.Customer);

        /// <summary>
        /// 更新會計師簽印分離截取設定
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accountantSignRangeSetting"></param>
        /// <returns></returns>
        [HttpPut("[Action]")]
        public ResponseViewModel UpdateAccountantSignRange(int id, AccountantSignRangeSetting accountantSignRangeSetting) 
            => imageRangeSettingService.Update(id, accountantSignRangeSetting, SealType.Accountant);

    }
}
