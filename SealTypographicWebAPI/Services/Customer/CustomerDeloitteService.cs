using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Services.Customer
{
    /// <summary>
    /// 勤業用的顧客資料
    /// </summary>
    public class CustomerDeloitteService : ICustomerService
    {
        /// <summary>
        /// 取得顧客印鑑組
        /// </summary>
        /// <param name="customerID">顧客ID</param>
        /// <param name="quarter">季度</param>
        /// <returns></returns>
        public CustomerSeals GetcustomerSeals(string customerID , string quarter)
        {
            List<CustomerSeal> customerSeals = new();
            for (int i = 0; i < 4; i++)
            {
                //測試資料
                CustomerSeal customerSeal = new()
                {
                    CustomerID = customerID,
                    CustomerSealGroupsID = i + 1,
                    No = 1,
                    ImagePath = "C://123.jpg",
                    AvailableDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    Quarter = quarter,
                };
                customerSeals.Add(customerSeal);
            }
            return new CustomerSeals()
            {
                ResponseStatus = 200,
                ResponseMessage = "Success",

                Seals = customerSeals
            };
        }

        /// <summary>
        /// 取得單筆顧客資料
        /// </summary>
        /// <param name="customerID">顧客ID</param>
        /// <returns></returns>
        public CustomerWithId GetCustomer(string customerID)
        {
            //測試資料
            CustomerWithId customerDataAddID = new()
            {
                //回傳結果訊息用
                ResponseStatus = 200,
                ResponseMessage = "Success",

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

        public CustomerViewModels GetCustomerViewModels(string customerIDOrName)
        {
            //測試資料
            List<CustomerViewModel> customerViewModels = new();
            CustomerViewModel customerListData1 = new()
            {
                CustomerID = "aaa000",
                IDnumber = "12345678",
                Name = "aaa公司",
            };
            CustomerViewModel customerListData2 = new()
            {
                CustomerID = "aaa001",
                IDnumber = "23456789",
                Name = "bbb公司",
            };
            CustomerViewModel customerListData3 = new()
            {
                CustomerID = "aaa002",
                IDnumber = "23456789",
                Name = "ccc公司",
            };
            CustomerViewModel customerListData4 = new()
            {
                CustomerID = "aaa003",
                IDnumber = "12345678",
                Name = "ddd公司",
            };
            customerViewModels.Add(customerListData1);
            customerViewModels.Add(customerListData2);
            customerViewModels.Add(customerListData3);
            customerViewModels.Add(customerListData4);

            customerViewModels = customerViewModels.Where(customerListData => 
                                                        customerListData.CustomerID.Contains(customerIDOrName) ||
                                                        customerListData.Name.Contains(customerIDOrName))                                                
                                                       .ToList();
            
            return new CustomerViewModels()
            {
                //回傳結果訊息用
                ResponseStatus = 200,
                ResponseMessage = "Success",

                ViewModels = customerViewModels,
            };
        }
    }
}