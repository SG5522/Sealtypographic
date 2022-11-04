using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Service.Accountant
{
    /// <summary>
    /// 會計資料處理
    /// </summary>
    public class Accountant
    {
        /// <summary>
        /// 宣告會計師資料處理的interface
        /// </summary>
        public readonly IAccountant _accountant;

        /// <summary>
        /// 注入會計師interface
        /// </summary>
        /// <param name="accountant"></param>
        public Accountant(IAccountant accountant)
        {
            _accountant = accountant;
        }

        /// <summary>
        /// 取得會計印鑑簽名組
        /// </summary>
        /// <param name="accountantID"></param>        
        /// <returns></returns>
        public List<AccountantSign> GetAccountantSigns(int accountantID)
        {
            return _accountant.GetAccountantSigns(accountantID);
        }

        /// <summary>
        /// 取得會計師基本資料
        /// </summary>
        /// <param name="accountantID"></param>
        /// <returns></returns>
        public AccountantDataAddID GetAccountantData(int accountantID)
        {
            return _accountant.GetAccountantData(accountantID);
        }
    }
}
