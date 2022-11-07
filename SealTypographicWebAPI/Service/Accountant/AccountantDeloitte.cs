using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Service.Accountant
{
    /// <summary>
    /// 勤業使用的取得會計師資料
    /// </summary>
    public class AccountantDeloitte : IAccountant
    {
        /// <summary>
        /// 取得會計師簽名印鑑組
        /// </summary>
        /// <param name="accountantID"></param>
        /// <returns></returns>
        public List<AccountantSign> GetAccountantSigns(string accountantID)
        {
            List<AccountantSign> signs = new();
            for (int i = 0; i < 4; i++)
            {
                //測試資料
                AccountantSign accountantSign = new()
                {
                    AccountantID = accountantID,
                    AccountantSignGroupID = i + 1,
                    ImagePath = "C://123.jpg",
                    AvailableDate = DateOnly.FromDateTime(DateTime.Now),
                    CreatedDate = DateOnly.FromDateTime(DateTime.Now),                    
                };
                signs.Add(accountantSign);
            }
            return signs;
        }

        /// <summary>
        /// 取得會計基本資料
        /// </summary>
        /// <param name="accountantID"></param>
        /// <returns></returns>
        public AccountantDataAddID GetAccountantData(string accountantID)
        {
            AccountantDataAddID accountantData = new()
            {
                ID = accountantID,
                AccountantGroupsID = "tap001",
                AccountantGroupName = "台北組"                
            };
            return accountantData;
        }
    }
}