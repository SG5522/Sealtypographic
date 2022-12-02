using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Consts;
using AutoMapper;
using SealTypographicWebAPI.Util;
using SealTypographicWebAPI.Utils;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 勤業用的顧客資料
    /// </summary>
    public class CustomerDeloitteService : ICustomerService
    {
        private readonly SealTypographicDbContext dbContext;        
        private readonly IMapper mapper;

        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public CustomerDeloitteService(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;            
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得單筆顧客資料
        /// </summary>
        /// <param name="customerId">顧客ID</param>
        /// <returns></returns>
        public CustomerResponseViewModel GetCustomerViewModel(string customerId)
        {
            CustomerForm? customer = new();
            ResponseViewModel response;
            IQueryable<Customer>? customerQuery = dbContext.Customers
                                    .Where(customer => customer.Id == customerId);

            if (customerQuery.Any())
            {

                Customer customerResponse = customerQuery.First();
                customer = mapper.Map<CustomerForm>(customerResponse);

                response = ResponseUtil.Success();
            }
            else
            {
                customer = null;
                response = ResponseUtil.NoData();
            }
            return new CustomerResponseViewModel()
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
        /// <param name="customerSearch">搜尋條件</param>  
        /// <returns></returns>
        public CustomerPaginatesViewModel GetCustomerPaginatesViewModel(CustomerSearch customerSearch)
        {
            List<CustomerPaginateViewModel> customerViewModels = new();
            ResponseViewModel response = new();
            int totalPage = 0;
            int totalCount = 0;
            IQueryable<Customer> customerQuery = dbContext.Customers;            
            if (!string.IsNullOrWhiteSpace(customerSearch.CustomerIdOrName))
            {
                customerQuery = customerQuery.Where
                    (
                        customer =>
                        customer.Id.Contains(customerSearch.CustomerIdOrName)
                        || customer.Name.Contains(customerSearch.CustomerIdOrName)                        
                    );
            }
            if(customerSearch.Status != (int)Status.All)
            {
                customerQuery = customerQuery.Where(customer => customer.Status == customerSearch.Status);
            }
            customerQuery = customerQuery.OrderBy(customer => customer.Id);

            if (customerQuery.Any())
            {
                //取得該頁            
                var pageNumberCustomers = customerQuery
                                          .Skip((customerSearch.PageNumber - 1) * customerSearch.PageSize)
                                          .Take(customerSearch.PageSize)
                                          .ToList();
                //計算總頁數
                totalPage = customerQuery.Count() / customerSearch.PageSize + (customerQuery.Count() % customerSearch.PageSize == 0 ? 0 : 1);
                totalCount = customerQuery.Count();
                foreach (var customerBase in pageNumberCustomers)
                {
                    customerViewModels.Add(mapper.Map<CustomerPaginateViewModel>(customerBase));
                }
                //取得成功訊息
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.NoData();
            }
            
            return new()
            {
                PageNumber = customerSearch.PageNumber,
                PageSize = customerSearch.PageSize,
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
        /// <param name="customerForm">基本資料</param>
        public ResponseViewModel CreateCustomer(CustomerForm customerForm)
        {
            ResponseViewModel response = new();
            IQueryable<Customer> customerQuery = dbContext.Customers
                                .Where(customer => customer.Id == customerForm.Id);

            if (!customerQuery.Any())
            {
                Customer dbCustomer = mapper.Map<Customer>(customerForm);
                dbCustomer.CreateDate = DateTime.Now;
                dbCustomer.AvailableDate = AvailableDateUtil.NotActivated();
                dbContext.Customers.Add(dbCustomer);
                dbContext.SaveChanges();
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.UniqueConstraintFailed();
            }

            return response;
        }

        /// <summary>
        /// 更新客戶基本資料
        /// </summary>
        /// <param name="customerForm">客戶基本資料 customerBaseData.id 為搜尋條件</param>        
        public ResponseViewModel UpdateCustomer(CustomerForm customerForm)
        {
            ResponseViewModel response = new();
            Customer? customerQuery = dbContext.Customers
                                .Where(customer => customer.Id == customerForm.Id)
                                .FirstOrDefault();

            if (customerQuery != null)
            {
                mapper.Map(customerForm, customerQuery);
                //修改資料時要改回審察與啟用日期設為不啟用(0000/01/01)
                customerQuery.Status = (int)Status.Pending;
                customerQuery.AvailableDate = AvailableDateUtil.NotActivated();

                dbContext.SaveChanges();
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.NoData();
            }

            return response;
        }

        /// <summary>
        /// 變更此客戶狀態為刪除。
        /// </summary>
        /// <param name="customerId">客戶ID</param>        
        public ResponseViewModel DeleteCustomer(string customerId)
        {
            ResponseViewModel response = new();
            Customer? customerQuery = dbContext.Customers
                                .Where(customer => customer.Id == customerId)
                                .FirstOrDefault();

            if (customerQuery != null)
            {
                customerQuery.Status = (int)Status.Hidden;
                customerQuery.AvailableDate = AvailableDateUtil.NotActivated();
                dbContext.SaveChanges();
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.NoData();
            }
            return response;
        }
    }
}