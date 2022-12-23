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
        AccountantSignGroupCreateDates GetAccountantWithGruopCreateDate(int accountantId);

        /// <summary>
        /// 取得會計師簽印組
        /// </summary>
        /// <param name="accountantSignStartDate"></param>
        /// <returns></returns>
        public AccountantSignViewModels GetAccountantSings(AccountantSignGroupCreateDate accountantSignStartDate);

        /// <summary>
        /// 新增印鑑組
        /// </summary>
        /// <param name="accountantSignPosts">會計印鑑簽名資料</param>
        /// <returns></returns>
        ResponseViewModel CreateAccountantSigns(List<AccountantSignForm> accountantSignPosts);

        /// <summary>
        /// 修改印鑑組
        /// </summary>
        /// <param name="accountantSignIds">會計師簽印id</param>
        /// <returns></returns>
        List<ResponseViewModel> UpdateReviewStatusPendingAccountantSigns(List<int> accountantSignIds);

        /// <summary>
        /// 刪除會計師簽印 (作廢)
        /// </summary>
        /// <param name="accountantSignIds">會計師簽印</param>
        /// <returns></returns>
        List<ResponseViewModel> DeleteAccountantSign(List<int> accountantSignIds);
    }
}
