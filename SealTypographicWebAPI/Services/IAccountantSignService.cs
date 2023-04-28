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
        /// <returns></returns>
        AccountantSignGroupResponse GetCreateDates(int accountantId);

        /// <summary>
        /// 取得會計師簽印組
        /// </summary>
        /// <param name="accountantSignGroupId">會計師簽印群組Id</param>
        /// <returns></returns>
        public AccountantSignViewModels GetSignViewModels(int accountantSignGroupId);

        /// <summary>
        /// 新增會計師簽印組
        /// </summary>
        /// <param name="accountantSignForms">會計師簽印組</param>
        /// <returns></returns>
        Task<ResponseViewModel> New(AccountantSignForms accountantSignForms);

        /// <summary>
        /// 異動會計師簽印
        /// </summary>
        /// <param name="accountantSignUpdate">需要異動會計師簽印資料</param>
        /// <returns></returns>
        Task<List<ResponseViewModel>> Update(AccountantSignUpdate accountantSignUpdate);

        /// <summary>
        /// 將草稿的簽印組狀態變更為待審
        /// </summary>
        /// <param name="accountantSignGroupId">會計師簽印群組Id</param>
        /// <returns></returns>
        ResponseViewModel Pending(int accountantSignGroupId);

        /// <summary>
        /// 將草稿的簽印組狀態變更為作廢
        /// </summary>
        /// <param name="accountantSignGroupId">會計師簽印群組Id</param>
        /// <returns></returns>
        ResponseViewModel Invalid(int accountantSignGroupId);

        /// <summary>
        /// 將待審的簽印組狀態變更為草稿
        /// </summary>
        /// <param name="accountantSignGroupId">會計師簽印群組Id</param>        
        ResponseViewModel CancelReview(int accountantSignGroupId);
    }
}
