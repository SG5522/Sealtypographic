using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Services.Accountant
{
    /// <summary>
    /// 會計師
    /// </summary>
    public interface IAccountantService
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
        AccountantDataWithId GetAccountantData(string accountantID);
    }
}
