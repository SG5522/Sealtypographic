using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Consts;
using AutoMapper;
using SealTypographicWebAPI.Utils;
using Microsoft.EntityFrameworkCore;

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
                customerDetailViewModel.CustomeNoData();                
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
            IQueryable<Customer> customerQuery = dbContext.Customers.Where(customer => customer.DeleteStatus == DeleteStatus.NO)
                                                .Include(customer => customer.CustomerSealJournals);
            
            if (!string.IsNullOrWhiteSpace(customerSearch.CustomerNumberOrName))
            {
                customerQuery = customerQuery.Where
                    (
                        customer =>
                        customer.Code.ToLower().Contains(customerSearch.CustomerNumberOrName.ToLower())
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
                
                foreach (Customer customerBase in pageNumberCustomers)
                {
                    CustomerViewModel customerViewModel = mapper.Map<CustomerViewModel>(customerBase);
                    if(customerBase.CustomerSealJournals.Count > 0)
                    {                        
                        customerViewModel.Quarter = customerBase.CustomerSealJournals.Max(x => x.Quarter);
                    }
                    customerViewModels.Add(customerViewModel);
                }
                customerPaginateViewModel.ViewModels = customerViewModels;
                customerPaginateViewModel.PageNumber = customerSearch.PageNumber;
                customerPaginateViewModel.PageSize= customerSearch.PageSize;
                //計算總頁數
                customerPaginateViewModel.TotalPage = TotalPageUtil.GetTotalPage(customerQuery.Count(), customerSearch.PageSize);
                customerPaginateViewModel.TotalCount = customerQuery.Count();
                customerPaginateViewModel.Success();
            }
            else
            {
                customerPaginateViewModel.CustomeNoData();                
            }

            return customerPaginateViewModel;
        }

        /// <summary>
        /// 新增顧客基本資料
        /// </summary>
        /// <param name="customerForm">基本資料</param>
        public CreateCustomerResponse CreateCustomer(CustomerForm customerForm)
        {
            CreateCustomerResponse createCustomerResponse = new();
            int userid = 0; //帳號驗證取得ID
            IQueryable<Customer> customerQuery = dbContext.Customers
                                .Where(customer => customer.Code == customerForm.Code);

            if (!customerQuery.Any())
            {
                Customer dbCustomer = mapper.Map<Customer>(customerForm);
                BaseInputCustomer(dbCustomer, true, userid);
                dbContext.Customers.Add(dbCustomer);
                dbContext.SaveChanges();

                //回傳剛建立的客戶基本資料 使建立客戶印鑑找到該ID
                Customer? customer = dbContext.Customers
                                    .FirstOrDefault(customer => customer.Code == customerForm.Code);
                                     
                if (customer != null) 
                {
                    createCustomerResponse.CustomerId = customer.Id;
                    createCustomerResponse.Success();
                }                                      
                else
                {
                    createCustomerResponse.CreateCustomerFailed();
                }
            }
            else
            {
                createCustomerResponse.CreateCustomerNumberRepeat();
            }
            return createCustomerResponse;
        }

        /// <summary>
        /// 更新客戶基本資料
        /// </summary>
        /// <param name="customerFormUpdate">客戶基本資料 customerForm.CustomerNumber 為搜尋條件</param>        
        public ResponseViewModel UpdateCustomer(CustomerFormUpdate customerFormUpdate)
        {
            ResponseViewModel response = new();
            int userId = 0;//帳號驗證取得ID
            Customer? customerQuery = dbContext.Customers.Find(customerFormUpdate.Id);

            if (customerQuery != null)
            {
                mapper.Map(customerFormUpdate, customerQuery);
                BaseInputCustomer(customerQuery, false, userId);
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.UpdateCustomerNoData();
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
            int userId = 0;//帳號驗證取得ID
            Customer? customerQuery = dbContext.Customers.Find(customerId);

            if (customerQuery != null)
            {
                customerQuery.DeleteStatus = DeleteStatus.Yes;
                BaseInputCustomer(customerQuery, false, userId);
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.DeleteCustomerNoData();
            }
            return response;
        }

        /// <summary>
        /// 信頭資料新增修改時基本資料輸入
        /// </summary>
        /// <param name="customer">DB上的客戶資料</param>
        /// <param name="isCreate">確認是否新增的動作</param>
        /// <param name="userid">使用者ID</param>
        private static void BaseInputCustomer(Customer customer, bool isCreate, int userid)
        {
            if (isCreate)
            {
                customer.CreateUserId = userid;
                customer.CreateDate = DateTime.Now;
                customer.DeleteStatus = DeleteStatus.NO;
            }
            else
            {
                customer.UpdateUserId = userid;
                customer.UpdateDate = DateTime.Now;
            }
        }
    }
}