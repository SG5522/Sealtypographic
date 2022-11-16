using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.DbModels;
using SealTypographicWebAPI.Consts;


namespace SealTypographicWebAPI.Services.Customer
{
    /// <summary>
    /// 勤業用的顧客資料
    /// </summary>
    public class CustomerDeloitteService : ICustomerService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ResponseService responseService;

        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="responseService"></param>
        public CustomerDeloitteService(SealTypographicDbContext dbContext,ResponseService responseService)
        {
            this.dbContext = dbContext;
            this.responseService = responseService;
        }

        /// <summary>
        /// 取得單筆顧客資料
        /// </summary>
        /// <param name="customerId">顧客ID</param>
        /// <returns></returns>
        public CustomerResponse GetCustomer(string customerId)
        {
            CustomerBaseData customer = new();
            Response response = new();
            var customerQuery = dbContext.Customers                                   
                                    .Where(customer => customer.Id == customerId);
                                    
            if (customerQuery.Any())
            {
                var customerResponse = customerQuery.First();

                customer.Id = customerResponse.Id;
                customer.BAN = customerResponse.BAN;
                customer.Name = customerResponse.Name;
                customer.StockCode = customerResponse.StockCode;
                customer.Address = customerResponse.Address;
                customer.Telephone = customerResponse.Telephone;
                customer.Fax = customerResponse.Fax;

                response = responseService.Get(ResponseCode.Success);
            }
            else
            {                
                response = responseService.Get(ResponseCode.NoData);
            }
            return new CustomerResponse()
            {
                //回傳結果訊息用
                Code = response.Code,
                Message = response.Message,

                BaseData = customer
            };
        }

        /// <summary>
        /// 依搜尋條件獲得顧客資料列表
        /// </summary>
        /// <param name="customerQueryPage">搜尋條件</param>  
        /// <returns></returns>
        public CustomerResponsePage GetCustomerViewModels(CustomerQueryPage customerQueryPage)
        {            
            List<CustomerViewModel> customerViewModels = new();
            Response response = new();
            int totalPage = 0;
            int totalCount = 0;
            var customerQuery = dbContext.Customers.AsQueryable();
            if (customerQueryPage.CustomerIdOrName != null) 
            {
                customerQuery = customerQuery.Where
                    (
                        customer =>
                        customer.Id.Contains(customerQueryPage.CustomerIdOrName)
                        || customer.Name.Contains(customerQueryPage.CustomerIdOrName)
                    );
            }

            customerQuery = customerQuery.OrderBy(customer => customer.Id);

            if (customerQuery.Any())
            {
                //取得該頁            
                var pageNumberCustomers = customerQuery
                                          .Skip((customerQueryPage.PageNumber - 1) * customerQueryPage.PageSize)
                                          .Take(customerQueryPage.PageSize)
                                          .ToList();
                //計算總頁數
                totalPage = (customerQuery.Count() / customerQueryPage.PageSize) + (customerQuery.Count() % customerQueryPage.PageSize == 0 ? 0 : 1) ;
                totalCount = customerQuery.Count();
                foreach (var customerBase in pageNumberCustomers)
                {
                    customerViewModels.Add(new CustomerViewModel()
                    {
                        CustomerID = customerBase.Id,
                        BAN = customerBase.BAN,
                        Name = customerBase.Name,
                        Status = customerBase.Status
                    });
                }
                //取得成功訊息
                response = responseService.Get(ResponseCode.Success);
            }
            else
            {
                response = responseService.Get(ResponseCode.NoData);
            }

            return new CustomerResponsePage()
            {
                PageNumber = customerQueryPage.PageNumber,
                TotalCount = totalCount,
                TotalPage = totalPage,
                Customers = customerViewModels,
                //回傳結果訊息用
                Code = response.Code,
                Message = response.Message
            };
        }

        /// <summary>
        /// 新增顧客基本資料
        /// </summary>
        /// <param name="customerBaseData">基本資料</param>
        public Response CreateCustomer(CustomerBaseData customerBaseData)
        {
            Response response = new();
            var customerQuery = dbContext.Customers
                                .Where(customer => customer.Id == customerBaseData.Id);

            if (!customerQuery.Any())
            {
                DbModels.Customer dbcustomer = new()
                {
                    Id = customerBaseData.Id,
                    Name = customerBaseData.Name,
                    BAN = customerBaseData.BAN,
                    Address = customerBaseData.Address,
                    StockCode = customerBaseData.StockCode,
                    Telephone = customerBaseData.Telephone,
                    Fax = customerBaseData.Fax,
                    Status = customerBaseData.Status
                };
                dbContext.Customers.Add(dbcustomer);
                dbContext.SaveChanges();
                response = responseService.Get(ResponseCode.Success);
            }
            else
            {
                response = responseService.Get(ResponseCode.UniqueConstraintFailed);
            }

            return response;
        }

        /// <summary>
        /// 更新客戶基本資料
        /// </summary>
        /// <param name="customerBaseData">客戶基本資料 customerBaseData.id 為搜尋條件</param>        
        public Response UpdateCustomer(CustomerBaseData customerBaseData)
        {
            Response response = new();
            var customerQuery = dbContext.Customers
                                .Where(customer => customer.Id == customerBaseData.Id);

            if(customerQuery.Any())
            {
                var customer = customerQuery.First();
                customer.BAN = customerBaseData.BAN;
                customer.Name = customerBaseData.Name;
                customer.Address = customerBaseData.Address;
                customer.StockCode = customerBaseData.StockCode;
                customer.Telephone = customerBaseData.Telephone;
                customer.Fax = customerBaseData.Fax;
                customer.Status = customerBaseData.Status;
                dbContext.SaveChanges();
                response = responseService.Get(ResponseCode.Success);
            }
            else
            {
                response = responseService.Get(ResponseCode.NoData);
            }

            return response;
        }

        /// <summary>
        /// 變更此客戶狀態為刪除。
        /// </summary>
        /// <param name="customerId">客戶ID</param>        
        public Response DeleteCustomer(string customerId)
        {
            Response response = new();
            var customerQuery = dbContext.Customers
                                .Where(customer => customer.Id == customerId);
            
            if(customerQuery.Any())
            {
                var customer = customerQuery.First();
                customer.Status = 2;
                dbContext.SaveChanges();
                response = responseService.Get(ResponseCode.Success);
            }
            else
            {
                response = responseService.Get(ResponseCode.NoData);
            }            
            return response;
        }
    }
}