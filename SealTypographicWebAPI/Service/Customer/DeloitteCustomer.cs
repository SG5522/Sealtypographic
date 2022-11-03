using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Service.Customer
{
    /// <summary>
    /// 勤業用的顧客資料
    /// </summary>
    public class DeloitteCustomer : ICustomer
    {
        /// <summary>
        /// 取得顧客印鑑組
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="quarter"></param>
        /// <returns></returns>
        public List<CustomerSeal> GetcustomerSeals(int customerID , string quarter)
        {
            List<CustomerSeal> customerSeals = new();
            for (int i = 0; i < 4; i++)
            {
                //測試資料
                CustomerSeal customerSeal = new()
                {
                    CustomerID = customerID,
                    CustomerSealGroupsID = i + 1,
                    ImagePath = "C://123.jpg",
                    AvailableDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    Quarter = "110Q" + (i + 1).ToString(),
                };
                customerSeals.Add(customerSeal);
            }
            return customerSeals;
        }
        /// <summary>
        /// 取得顧客資料
        /// </summary>
        /// <param name="customerID">顧客ID</param>
        /// <returns></returns>
        public CustomerDataAddID GetCustomerData(int customerID)
        {
            //測試資料
            CustomerDataAddID customerDataAddID = new()
            {
                ID = customerID,
                IDnumber = "123456789",
                Name = "aaa公司",
                Account = "aaa001",
                StockCode = "9999",
                Address = "aaabbbcccddd",
                TelPhone = "28825252",
                Fax = "28825252"
            };
            return customerDataAddID;
        }
    }
}
