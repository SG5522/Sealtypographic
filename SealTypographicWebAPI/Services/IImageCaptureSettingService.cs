using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.SealCaptureRange;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 印鑑截取範圍設定
    /// </summary>
    public interface IImageCaptureSettingService
    {

        /// <summary>
        /// 取得客戶印鑑分離截取設定
        /// </summary>
        /// <param name="userId">使用者Id</param>
        /// <returns></returns>
        CustomerSealCaptureResponse GetCustomerSealCapture(int userId = 1);

        /// <summary>
        /// 取得會計師簽印分離截取設定
        /// </summary>
        /// <param name="userId">使用者Id</param>
        /// <returns></returns>
        AccountantSignCaptureResponse GetAccountantSignCapture(int userId = 1);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="customerSealSetting"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResponseViewModel New<T>(T customerSealSetting, int userId = 1); 

    }
}
