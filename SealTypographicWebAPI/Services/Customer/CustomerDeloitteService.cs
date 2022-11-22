using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.DbModels;
using SealTypographicWebAPI.Consts;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SealTypographicWebAPI.Services.Customer
{
    /// <summary>
    /// 勤業用的顧客資料
    /// </summary>
    public class CustomerDeloitteService : ICustomerService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ResponseService responseService;
        private readonly IMapper mapper;

        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="responseService"></param>
        /// <param name="mapper"></param>
        public CustomerDeloitteService(SealTypographicDbContext dbContext,ResponseService responseService,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.responseService = responseService;
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得單筆顧客資料
        /// </summary>
        /// <param name="customerId">顧客ID</param>
        /// <returns></returns>
        public CustomerResponse GetCustomer(string customerId)
        {
            CustomerData? customer = new();
            Response response;
            IQueryable<DbModels.Customer>? customerQuery = dbContext.Customers                                   
                                    .Where(customer => customer.Id == customerId);
            
            if (customerQuery.Any())
            {
                
                DbModels.Customer customerResponse = customerQuery.First();
                customer = mapper.Map<CustomerData>(customerResponse);

                response = responseService.Get(ResponseCode.Success);
            }
            else
            {
                customer = null;
                response = responseService.Get(ResponseCode.NoData);
            }
            return new CustomerResponse()
            {
                //回傳結果訊息用
                Code = response.Code,
                Message = response.Message,

                Data = customer
            };
        }

        /// <summary>
        /// 依搜尋條件獲得顧客資料列表
        /// </summary>
        /// <param name="customerQueryPage">搜尋條件</param>  
        /// <returns></returns>
        public CustomerResponsePage GetCustomerViewModels(ImageGroupQueryPage customerQueryPage)
        {            
            List<CustomerViewModel> customerViewModels = new();
            Response response = new();
            int totalPage = 0;
            int totalCount = 0;
            IQueryable<DbModels.Customer> customerQuery;

            if (customerQueryPage.CustomerIdOrName != null) 
            {
                customerQuery = dbContext.Customers.Where
                    (
                        customer =>
                        customer.Id.Contains(customerQueryPage.CustomerIdOrName)
                        || customer.Name.Contains(customerQueryPage.CustomerIdOrName)
                    );
            }
            else
            {
                customerQuery = dbContext.Customers;
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
                totalPage = (customerQuery.Count() / customerQueryPage.PageSize) + (customerQuery.Count() % customerQueryPage.PageSize == 0 ? 0 : 1);
                totalCount = customerQuery.Count();
                foreach (var customerBase in pageNumberCustomers)
                {
                    customerViewModels.Add(mapper.Map<CustomerViewModel>(customerBase));
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
                PageSize = customerQueryPage.PageSize,
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
        public Response CreateCustomer(CustomerData customerBaseData)
        {
            Response response = new();
            IQueryable<DbModels.Customer> customerQuery = dbContext.Customers
                                .Where(customer => customer.Id == customerBaseData.Id);

            if (!customerQuery.Any())
            {
                DbModels.Customer dbCustomer = mapper.Map<DbModels.Customer>(customerBaseData);
                dbContext.Customers.Add(dbCustomer);
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
        public Response UpdateCustomer(CustomerData customerBaseData)
        {
            Response response = new();
            DbModels.Customer? customerQuery = dbContext.Customers
                                .Where(customer => customer.Id == customerBaseData.Id)
                                .FirstOrDefault();
            
            if (customerQuery != null)
            {
                mapper.Map(customerBaseData, customerQuery);
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