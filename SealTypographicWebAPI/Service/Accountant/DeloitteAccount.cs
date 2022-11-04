using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Service.Customer;

namespace SealTypographicWebAPI.Service.Accountant
{
    public class DeloitteAccount : IAccountant
    {
        /// <summary>
        /// 取得會計師簽名印鑑組
        /// </summary>
        /// <param name="accountantID"></param>
        /// <returns></returns>
        public List<AccountantSign> GetAccountantSigns(int accountantID)
        {
            List<AccountantSign> signs = new List<AccountantSign>();
            for (int i = 0; i < 4; i++)
            {
                //測試資料
                AccountantSign accountantSign = new()
                {
                    AccountantID = accountantID,
                    AccountantSignGroupID = i + 1,
                    ImagePath = "C://123.jpg",
                    AvailableDate = DateTime.Now,
                    CreatedDate = DateTime.Now,                    
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
        public AccountantDataAddID GetAccountantData(int accountantID)
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
