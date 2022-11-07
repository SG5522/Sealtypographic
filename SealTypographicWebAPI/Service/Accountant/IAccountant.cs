using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Service.Accountant
{
    /// <summary>
    /// 會計師
    /// </summary>
    public interface IAccountant
    {
        /// <summary>
        /// 取得會計師簽名印鑑組
        /// </summary>
        /// <param name="accountantID"></param>
        /// <returns></returns>
        List<AccountantSign> GetAccountantSigns(string accountantID);

        /// <summary>
        /// 取得會計基本資料
        /// </summary>
        /// <param name="accountantID"></param>
        /// <returns></returns>
        AccountantDataAddID GetAccountantData(string accountantID);
    }
}
