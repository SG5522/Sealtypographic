using DBEntities.Consts;
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
        /// <param name="companyId">會計師事務所Id</param>
        /// <returns></returns>
        CustomerSealCaptureResponse GetCustomerSealCapture(int companyId = 1);

        /// <summary>
        /// 取得會計師簽印分離截取設定
        /// </summary>
        /// <param name="companyId">會計師事務所Id</param>
        /// <returns></returns>
        AccountantSignCaptureResponse GetAccountantSignCapture(int companyId = 1);

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T">CustomerSealCaptureSetting AccountantSignCaptureSetting</typeparam>
        /// <param name="captureSetting"></param>
        /// <param name="companyId">會計師事務所Id</param>
        /// <returns></returns>
        ResponseViewModel New<T>(T captureSetting, int companyId = 1);

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T">CustomerSealCaptureSetting AccountantSignCaptureSetting</typeparam>
        /// <param name="captureSetting"></param>
        /// <param name="sealType"></param>
        /// <param name="companyId">會計師事務所Id</param>
        /// <returns></returns>
        ResponseViewModel Update<T>(T captureSetting, SealType sealType, int companyId = 1);
    }
}
