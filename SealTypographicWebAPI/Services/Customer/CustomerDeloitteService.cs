using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.DbModels;
using System.Linq;

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
        /// 取得單筆顧客資料
        /// </summary>
        /// <param name="customerId">顧客ID</param>
        /// <returns></returns>
        public CustomerResponse GetCustomer(string customerId)
        {
            var customerResponse = dbContext.Customers                                   
                                    .Where(customer => customer.Id == customerId)
                                    .First();
            
            if (customerResponse != null)
            {
                return new CustomerResponse()
                {
                    //回傳結果訊息用
                    Code = 200,
                    Message = "Success",

                    BaseData = new CustomerBaseData()
                    {
                        Id = customerResponse.Id,
                        BAN = customerResponse.BAN,
                        Name = customerResponse.Name,
                        StockCode = customerResponse.StockCode,
                        Address = customerResponse.Address,
                        Telephone = customerResponse.Telephone,
                        Fax = customerResponse.Fax
                    }                    
                };
            }
            else
            {
                return new CustomerResponse()
                {
                    Code = 404,
                    Message = "NoData"
                };
            }
            
        }

        /// <summary>
        /// 依搜尋條件獲得顧客資料列表
        /// </summary>
        /// <param name="customerIdOrName">顧客名字或ID</param>
        /// <param name="thisPage">現在頁次</param>        
        /// <returns></returns>
        public CustomerResponseViewModel GetCustomerViewModels(string customerIdOrName,int thisPage)
        {
            List<CustomerViewModel> customerViewModels = new();
            var customer = dbContext.Customers.Where
                                    (
                                        customer => 
                                        customer.Id.Contains(customerIdOrName) ||
                                        customer.Name.Contains(customerIdOrName)
                                    );
            int pageSize = 10;
            
            //取得該頁
            var thisPageCustomerBaseData = customer.Skip(thisPage * pageSize).Take(pageSize);
            foreach (var customerBase in thisPageCustomerBaseData)
            {
                customerViewModels.Add(new CustomerViewModel()
                {
                    CustomerID = customerBase.Id,
                    BAN = customerBase.BAN,
                    Name = customerBase.Name,
                    Status = customerBase.Status
                });
            }

            return new CustomerResponseViewModel()
            {
                ThisPage = thisPage,
                TotalCount = customer.Count(),
                TotalPage = customer.Count()/10,
                Customers = customerViewModels,
                //回傳結果訊息用
                Code = 200,
                Message = "Success"
            };
        }

        /// <summary>
        /// 新增顧客基本資料
        /// </summary>
        /// <param name="customer"></param>
        public void CreateCustomer(CustomerBaseData customer)
        {
            DbModels.Customer dbcustomer = new()
            {
                Id = customer.Id,
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