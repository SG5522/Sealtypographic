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
    public class CustomerService : ICustomerService
    {
        private readonly SealTypographicDbContext dbContext;        
        private readonly IMapper mapper;

        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public CustomerService(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;            
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得單筆顧客資料
        /// </summary>
        /// <param name="customerId">顧客ID</param>
        /// <returns></returns>
        public CustomerDetailViewModel GetCustomerDetailViewModel(int customerId)
        {
            CustomerDetailViewModel customerDetailViewModel = new();                        
            Customer? customerQuery = dbContext.Customers.Find(customerId);                                    
            if (customerQuery != null)
            {

                customerDetailViewModel.CustomerDetail = mapper.Map<CustomerDetail>(customerQuery);
                customerDetailViewModel.Success();                
            }
            else
            {
                customerDetailViewModel.DbNoData();                
            }
            return customerDetailViewModel;
        }

        /// <summary>
        /// 依搜尋條件獲得顧客資料列表
        /// </summary>
        /// <param name="customerSearch">搜尋條件</param>  
        /// <returns></returns>
        public CustomerPaginateViewModel GetCustomerPaginatesViewModel(CustomerSearch customerSearch)
        {
            CustomerPaginateViewModel customerPaginateViewModel = new();
            List<CustomerViewModel> customerViewModels = new();            
            IQueryable<Customer> customerQuery = dbContext.Customers.Where(customer => customer.DeleteStatus == DeleteStatus.NO);            
            if (!string.IsNullOrWhiteSpace(customerSearch.CustomerNumberOrName))
            {
                customerQuery = customerQuery.Where
                    (
                        customer =>
                        customer.CustomerNumber.Contains(customerSearch.CustomerNumberOrName)
                        || customer.Name.Contains(customerSearch.CustomerNumberOrName)                        
                    );
            }
            customerQuery = customerQuery.OrderBy(customer => customer.Id);

            if (customerQuery.Any())
            {
                //取得該頁            
                List<Customer> pageNumberCustomers = customerQuery
                                          .Skip((customerSearch.PageNumber - 1) * customerSearch.PageSize)
                                          .Take(customerSearch.PageSize)
                                          .ToList();
                
                foreach (var customerBase in pageNumberCustomers)
                {
                    customerViewModels.Add(mapper.Map<CustomerViewModel>(customerBase));
                }
                //計算總頁數
                customerPaginateViewModel.TotalPage = TotalPageUtil.GetTotalPage(customerQuery.Count(), customerSearch.PageSize);
                customerPaginateViewModel.TotalCount = customerQuery.Count();                
                customerPaginateViewModel.PageNumber = customerSearch.PageNumber;
                customerPaginateViewModel.Customers = customerViewModels;
                customerPaginateViewModel.Success();
            }
            else
            {
                customerPaginateViewModel.DbNoData();                
            }

            return customerPaginateViewModel;
        }

        /// <summary>
        /// 新增顧客基本資料
        /// </summary>
        /// <param name="customerForm">基本資料</param>
        public ResponseViewModel CreateCustomer(CustomerForm customerForm)
        {
            ResponseViewModel response = new();
            IQueryable<Customer> customerQuery = dbContext.Customers
                                .Where(customer => customer.CustomerNumber == customerForm.CustomerNumber);

            if (!customerQuery.Any())
            {
                Customer dbCustomer = mapper.Map<Customer>(customerForm);
                dbCustomer.CreateDate = DateTime.Now;
                dbCustomer.DeleteStatus = DeleteStatus.NO;
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
        /// <param name="customerFormUpdate">客戶基本資料 customerForm.CustomerNumber 為搜尋條件</param>        
        public ResponseViewModel UpdateCustomer(CustomerFormUpdate customerFormUpdate)
        {
            ResponseViewModel response = new();
            Customer? customerQuery = dbContext.Customers
                                .Where(customer => customer.Id == customerFormUpdate.Id)
                                .FirstOrDefault();

            if (customerQuery != null)
            {
                mapper.Map(customerFormUpdate, customerQuery);                
                customerQuery.UpdateDate = DateTime.Now;

                dbContext.SaveChanges();
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.DbNoData();
            }

            return response;
        }

        /// <summary>
        /// 變更此客戶狀態為刪除。
        /// </summary>
        /// <param name="customerId">客戶ID</param>        
        public ResponseViewModel DeleteCustomer(int customerId)
        {
            ResponseViewModel response = new();
            Customer? customerQuery = dbContext.Customers
                                .Where(customer => customer.Id == customerId)
                                .FirstOrDefault();

            if (customerQuery != null)
            {
                customerQuery.DeleteStatus = DeleteStatus.Yes;
                dbContext.SaveChanges();
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.DbNoData();
            }
            return response;
        }
    }
}