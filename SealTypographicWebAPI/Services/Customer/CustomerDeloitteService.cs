using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.DbModels;

namespace SealTypographicWebAPI.Services.Customer
{
    /// <summary>
    /// 勤業用的顧客資料
    /// </summary>
    public class CustomerDeloitteService : ICustomerService
    {
        private readonly SealTypographicDbContext dbContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public CustomerDeloitteService(SealTypographicDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// 取得顧客印鑑組
        /// </summary>
        /// <param name="customerID">顧客ID</param>
        /// <param name="quarter">季度</param>
        /// <returns></returns>
        public CustomerSeals GetCustomerSeals(string customerID , string quarter)
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
                Code = 200,
                Message = "Success",

                Seals = customerSeals
            };
        }

        /// <summary>
        /// 取得單筆顧客資料
        /// </summary>
        /// <param name="customerID">顧客ID</param>
        /// <returns></returns>
        public Models.Customer GetCustomer(string customerID)
        {
            //測試資料
            Models.Customer customerDataAddID = new()
            {
                //回傳結果訊息用
                Code = 200,
                Message = "Success",

                ID = customerID,
                BAN = "123456789",
                Name = "aaa公司",                
                StockCode = "9999",
                Address = "aaabbbcccddd",
                Telephone = "28825252",
                Fax = "28825252"
            };
            return customerDataAddID;
        }

        /// <summary>
        /// 依搜尋條件獲得顧客資料列表
        /// </summary>
        /// <param name="customerIDOrName">顧客名字或ID</param>
        /// <returns></returns>

        public CustomerResponseViewModel GetCustomerViewModels(string customerIDOrName)
        {
            //測試資料
            List<CustomerViewModel> customerViewModels = new();
            CustomerViewModel customerListData1 = new()
            {
                CustomerID = "aaa000",
                BAN = "12345678",
                Name = "aaa公司",
            };
            CustomerViewModel customerListData2 = new()
            {
                CustomerID = "aaa001",
                BAN = "23456789",
                Name = "bbb公司",
            };
            CustomerViewModel customerListData3 = new()
            {
                CustomerID = "aaa002",
                BAN = "23456789",
                Name = "ccc公司",
            };
            CustomerViewModel customerListData4 = new()
            {
                CustomerID = "aaa003",
                BAN = "12345678",
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
            
            return new CustomerResponseViewModel()
            {
                //回傳結果訊息用
                Code = 200,
                Message = "Success",

                Customers = customerViewModels,
            };
        }

        /// <summary>
        /// 新增顧客基本資料
        /// </summary>
        /// <param name="customer"></param>
        public void CreateCustomer(Models.Customer customer)
        {
            DbModels.Customer dbcustomer = new()
            {
                ID = customer.ID,
                Name = customer.Name,
                BAN = customer.BAN,                
                Address = customer.Address,
                StockCode = customer.StockCode,
                Telephone = customer.Telephone,
                Fax = customer.Fax,
                Status = customer.Status                
            };
            dbContext.Customers.Add(dbcustomer);
            dbContext.SaveChanges();
        }
    }
}