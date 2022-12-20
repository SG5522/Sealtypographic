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
        /// 取得會計印鑑
        /// </summary>
        /// <param name="accountantId">會計師ID</param>        
        /// <returns></returns>
        AccountantSignViewModels GetAccountantSings(int accountantId);


        /// <summary>
        /// 新增印鑑組
        /// </summary>
        /// <param name="accountantSignPosts">會計印鑑簽名資料</param>
        /// <returns></returns>
        ResponseViewModel CreateAccountantSigns(List<AccountantSignForm> accountantSignPosts);

        /// <summary>
        /// 修改印鑑組
        /// </summary>
        /// <param name="AccountantSignUpdate">會計印鑑刪除修改新增資料</param>
        /// <returns></returns>
        ResponseViewModel UpdateAccountantSigns(AccountantSignUpdate AccountantSignUpdate);

        /// <summary>
        /// 刪除會計師簽印 (隱藏)
        /// </summary>
        /// <param name="accountantSignId"></param>
        /// <returns></returns>
        ResponseViewModel DeleteAccountantSign(int accountantSignId);
    }
}
