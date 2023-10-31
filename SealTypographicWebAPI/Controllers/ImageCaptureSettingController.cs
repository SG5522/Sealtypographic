using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Upload;
using Serilog;
using DBEntities.Consts;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Models.SealCaptureRange;

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 上傳
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ImageCaptureSettingController : ControllerBase
    {
        private readonly IImageCaptureSettingService imageCaptureSettingService;

        /// <summary>
        /// 注入ImageCaptureSettingService
        /// </summary>
        /// <param name="imageCaptureSettingService"></param>
        public ImageCaptureSettingController(IImageCaptureSettingService imageCaptureSettingService)
        {
            this.imageCaptureSettingService = imageCaptureSettingService;            
        }

        /// <summary>
        /// 取得客戶印鑑分離截取設定
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public CustomerSealCaptureResponse CustomerSealCapture() => imageCaptureSettingService.GetCustomerSealCapture();

        /// <summary>
        /// 取得客戶印鑑分離截取設定
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public AccountantSignCaptureResponse AccountantSignCapture() => imageCaptureSettingService.GetAccountantSignCapture();

        /// <summary>
        /// 新增客戶印鑑分離截取設定
        /// </summary>        
        /// <param name="customerSealSetting">客戶印鑑截取設定</param>
        /// <returns></returns>
        [HttpPost("[Action]")]
        public ResponseViewModel NewCustomerSealCapture(CustomerSealCaptureSetting customerSealSetting) => imageCaptureSettingService.New(customerSealSetting);

        /// <summary>
        /// 新增客戶印鑑分離截取設定
        /// </summary>        
        /// <param name="accountantSignCaptureSetting">客戶印鑑截取設定</param>
        /// <returns></returns>
        [HttpPost("[Action]")]
        public ResponseViewModel NewAccountantSignCapture(AccountantSignCaptureSetting accountantSignCaptureSetting) => imageCaptureSettingService.New(accountantSignCaptureSetting);

        /// <summary>
        /// 更新客戶印鑑截取設定
        /// </summary>
        /// <param name="customerSealCaptureSetting"></param>
        /// <returns></returns>
        [HttpPut("[Action]")]
        public ResponseViewModel UpdateCustomerSealCapture(CustomerSealCaptureSetting customerSealCaptureSetting) => imageCaptureSettingService.Update(customerSealCaptureSetting, SealType.Customer);

        /// <summary>
        /// 更新會計師簽印分離截取設定
        /// </summary>
        /// <param name="accountantSignCaptureSetting"></param>
        /// <returns></returns>
        [HttpPut("[Action]")]
        public ResponseViewModel UpdateAccountantSignCapture(AccountantSignCaptureSetting accountantSignCaptureSetting) => imageCaptureSettingService.Update(accountantSignCaptureSetting, SealType.Accountant);

    }
}
