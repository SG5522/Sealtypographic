using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Customer;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 顧客印鑑組Interface
    /// </summary>
    public interface IAccountantSignService
    {
        /// <summary>
        /// 取得會計取用時間列
        /// </summary>
        /// <param name="accountantId">會計師ID</param>        
        /// <returns></returns>
        AccountantSignGroupCreateDateViews GetAccountantWithGruopCreateDate(int accountantId);

        /// <summary>
        /// 取得會計師簽印組
        /// </summary>
        /// <param name="accountantSignStartDate"></param>
        /// <returns></returns>
        public AccountantSignViewModels GetAccountantSings(AccountantSignGroupCreateDateSearch accountantSignStartDate);

        /// <summary>
        /// 新增印鑑組
        /// </summary>
        /// <param name="accountantSignPosts">會計印鑑簽名資料</param>
        /// <returns></returns>
        ResponseViewModel CreateAccountantSigns(List<AccountantSignForm> accountantSignPosts);

        /// <summary>
        /// 異動會計師簽印的處理(審查狀態退回或是草稿才進行修改)
        /// </summary>
        /// <param name="accountantSignUpdate">刪除修改新增的list</param>
        /// <returns></returns>
        List<ResponseViewModel> UpdateAccountantSign(AccountantSignUpdate accountantSignUpdate);

        /// <summary>
        /// 修改印鑑組
        /// </summary>
        /// <param name="accountantSignGroupCreateDateSearch">會計師簽印id</param>
        /// <returns></returns>
        ResponseViewModel UpdateReviewStatusPendingAccountantSigns(AccountantSignGroupCreateDateSearch accountantSignGroupCreateDateSearch);

        /// <summary>
        /// 刪除會計師簽印 (作廢)
        /// </summary>
        /// <param name="accountantSignGroupCreateDateSearch">會計師簽印</param>
        /// <returns></returns>
        ResponseViewModel UpdateReviewStatusInvalidAccountantSigns(AccountantSignGroupCreateDateSearch accountantSignGroupCreateDateSearch);
    }
}
