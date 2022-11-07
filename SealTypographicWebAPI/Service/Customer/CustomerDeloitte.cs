using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Service.Customer
{
    /// <summary>
    /// 勤業用的顧客資料
    /// </summary>
    public class CustomerDeloitte : ICustomer
    {
        /// <summary>
        /// 取得顧客印鑑組
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="quarter"></param>
        /// <returns></returns>
        public List<CustomerSeal> GetcustomerSeals(string customerID , string quarter)
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
        /// 取得單筆顧客資料
        /// </summary>
        /// <param name="customerID">顧客ID</param>
        /// <returns></returns>
        public CustomerDataAddID GetCustomerData(string customerID)
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

        /// <summary>
        /// 依搜尋條件獲得顧客資料列表
        /// </summary>
        /// <param name="customerIDOrName">顧客名字或ID</param>
        /// <returns></returns>

        public List<CustomerListData> GetCustomerList(string customerIDOrName)
        {
            //測試資料
            List<CustomerListData> customerListDatas = new();
            CustomerListData customerListData1 = new()
            {
                CustomerID = "aaa000",
                IDnumber = "12345678",
                Name = "aaa公司",
            };
            CustomerListData customerListData2 = new()
            {
                CustomerID = "aaa001",
                IDnumber = "23456789",
                Name = "bbb公司",
            };
            CustomerListData customerListData3 = new()
            {
                CustomerID = "aaa002",
                IDnumber = "23456789",
                Name = "ccc公司",
            };
            CustomerListData customerListData4 = new()
            {
                CustomerID = "aaa003",
                IDnumber = "12345678",
                Name = "ddd公司",
            };
            customerListDatas.Add(customerListData1);
            customerListDatas.Add(customerListData2);
            customerListDatas.Add(customerListData3);
            customerListDatas.Add(customerListData4);

            customerListDatas = customerListDatas.Where(customerListData => 
                                                        customerListData.CustomerID.Contains(customerIDOrName) ||
                                                        customerListData.Name.Contains(customerIDOrName))                                                
                                                       .ToList();
            return customerListDatas;
        }
    }
}