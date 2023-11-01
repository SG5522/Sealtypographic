using DBEntities.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.BaseModels;
using SealTypographicWebAPI.Models.ImageRangeSetting;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 印鑑截取範圍設定
    /// </summary>
    public interface IImageRangeSettingService
    {

        /// <summary>
        /// 取得客戶印鑑分離截取設定
        /// </summary>
        /// <param name="companyId">會計師事務所Id</param>
        /// <returns></returns>
        CustomerSealRangeSettingResponse GetCustomerSealRangeSetting(int companyId = 1);

        /// <summary>
        /// 取得會計師簽印分離截取設定
        /// </summary>
        /// <param name="companyId">會計師事務所Id</param>
        /// <returns></returns>
        AccountantSignRangeSettingResponse GetAccountantSignRangeSetting(int companyId = 1);

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T">CustomerSealRangeSetting AccountantSignRangeSetting</typeparam>        
        /// <param name="rangeSetting">範圍參數設定</param>
        /// <param name="userId">登入使用者Id</param>
        /// <returns></returns>
        ResponseViewModel New<T>(T rangeSetting, int userId = 1);

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T">CustomerSealRangeSetting AccountantSignRangeSetting</typeparam>
        /// <param name="id"></param>
        /// <param name="rangeSetting">範圍參數設定</param>
        /// <param name="sealType">印鑑類別</param>
        /// <param name="userId">登入使用者Id</param>
        /// <returns></returns>
        ResponseViewModel Update<T>(int id, T rangeSetting, SealType sealType, int userId = 1) where T : BaseLocation;
    }
}
