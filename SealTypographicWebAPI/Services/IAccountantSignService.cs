using DBEntities.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Customer;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 會計師簽印管理
    /// </summary>
    public interface IAccountantSignService
    {
        /// <summary>
        /// 取得會計師簽印建立日期列表
        /// </summary>
        /// <param name="accountantId">會計師ID</param>
        /// <param name="userId">登入的使用者ID</param>  
        /// <returns></returns>
        Task<AccountantSignGroupResponse> GetCreateDates(int accountantId, int userId = 1);

        /// <summary>
        /// 取得會計師簽印組
        /// </summary>
        /// <param name="accountantSignGroupId">會計師簽印群組Id</param>
        /// <param name="isTransparent">是否白底透明化</param>
        /// <param name="userInfo">登入使用者基本資訊</param>
        /// <returns></returns>        
        Task<AccountantSignViewModels> GetSignViewModels(int accountantSignGroupId, bool isTransparent, UserInfo userInfo);

        /// <summary>
        /// 新增會計師簽印組
        /// </summary>
        /// <param name="accountantSignForms">會計師簽印組</param>
        /// <param name="userId">登入的使用者ID</param>
        /// <returns></returns>
        Task<ResponseViewModel> New(AccountantSignForms accountantSignForms, int userId = 1);

        /// <summary>
        /// 異動會計師簽印
        /// </summary>
        /// <param name="accountantSignUpdate">需要異動會計師簽印資料</param>
        /// <param name="userId">登入的使用者ID</param>
        /// <returns></returns>
        Task<List<ResponseViewModel>> Update(AccountantSignUpdate accountantSignUpdate, int userId = 1);

        /// <summary>
        /// 會計師印鑑待審狀態變更。
        /// </summary>
        /// <param name="accountantSignGroupId">會計師簽印群組Id</param>
        /// <param name="reviewStatus">審查狀態</param>
        /// <param name="userId">登入的使用者ID</param>        
        Task<ResponseViewModel> ChangeReviewStatus(int accountantSignGroupId, ReviewStatus reviewStatus, int userId = 1);
    }
}
