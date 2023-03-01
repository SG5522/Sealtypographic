using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using DBEntities;
using DBEntities.Consts;
using AutoMapper;
using SealTypographicWebAPI.Utils;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 客戶資料管理
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly SealTypographicDbContext dbContext;        
        private readonly IMapper mapper;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public CustomerService(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;            
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得客戶詳細基本資料
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        public CustomerDetailViewModel GetDetail(int customerId)
        {
            CustomerDetailViewModel customerDetailViewModel = new();                        
            Customer? customerQuery = dbContext.Customers.Find(customerId);         
            
            if (customerQuery != null)
            {
                customerDetailViewModel.CustomerDetail = mapper.Map<CustomerDetail>(customerQuery);                                
            }
            customerDetailViewModel.Success();

            return customerDetailViewModel;
        }

        /// <summary>
        /// 取得客戶資料列表(分頁)
        /// </summary>
        /// <param name="customerSearch">客戶分頁搜尋</param>  
        /// <returns></returns>
        public CustomerPaginateViewModel GetPaginate(CustomerSearch customerSearch)
        {
            CustomerPaginateViewModel customerPaginateViewModel = new();            
            IQueryable<Customer> customerQuery = dbContext.Customers.Where(customer => customer.DeleteStatus == DeleteStatus.No);                                                
            
            if (!string.IsNullOrWhiteSpace(customerSearch.KeyWord))
            {
                customerQuery = customerQuery.Where
                    (
                        customer =>
                        customer.Code.ToLower().Contains(customerSearch.KeyWord.ToLower())
                        || customer.Name.Contains(customerSearch.KeyWord)                       
                    );
            }
            customerQuery = customerQuery.OrderBy(customer => customer.Code);

            if (customerQuery.Any())
            {
                //取得該頁            
                List<Customer> pageNumberCustomers = customerQuery
                                          .Skip((customerSearch.PageNumber - 1) * customerSearch.PageSize)
                                          .Take(customerSearch.PageSize)                                   
                                          .ToList();
                //取得狀態
                foreach (Customer customer in pageNumberCustomers)
                {
                    CustomerViewModel customerViewModel = mapper.Map<CustomerViewModel>(customer);

                    List<CustomerSealQuarterJournal> customerSealQuarterJournals = dbContext.CustomerSealQuarterJournals
                                                                                    .Where(x => x.Customer.Id == customer.Id
                                                                                    && x.DeleteStatus == DeleteStatus.No
                                                                                    && x.ReviewStatus < ReviewStatus.Disabled ).ToList();

                    if(customerSealQuarterJournals.Any())
                    {
                        if (!customerSealQuarterJournals.Where(x => x.ReviewStatus != ReviewStatus.Approval).Any())
                        {
                            customerViewModel.IsDraff = false;
                            customerViewModel.IsPending = false;
                            customerViewModel.IsReject = false;
                        }
                        else
                        {
                            if (customerSealQuarterJournals.Where(x => x.ReviewStatus == ReviewStatus.Draft).Any())
                            {
                                customerViewModel.IsDraff = true;
                            }
                            if (customerSealQuarterJournals.Where(x => x.ReviewStatus == ReviewStatus.Pending).Any())
                            {
                                customerViewModel.IsPending = true;
                            }
                            if (customerSealQuarterJournals.Where(x => x.ReviewStatus == ReviewStatus.Reject).Any())
                            {
                                customerViewModel.IsReject = true;
                            }
                        }
                        customerViewModel.CustomerSealQuarterId = customerSealQuarterJournals.OrderByDescending(x => x.Quarter).Select(x => x.Id).FirstOrDefault();                        
                    }
                                    
                    customerPaginateViewModel.ViewModels.Add(customerViewModel);                    
                }                
                customerPaginateViewModel.PageNumber = customerSearch.PageNumber;
                customerPaginateViewModel.PageSize= customerSearch.PageSize;
                //計算總頁數
                customerPaginateViewModel.TotalPage = TotalPageUtil.GetTotalPage(customerQuery.Count(), customerSearch.PageSize);
                customerPaginateViewModel.TotalCount = customerQuery.Count();                
            }
            customerPaginateViewModel.Success();

            return customerPaginateViewModel;
        }

        /// <summary>
        /// 新增客戶基本資料
        /// </summary>
        /// <param name="customerForm">基本資料</param>
        public CreateCustomerResponse New(CustomerForm customerForm)
        {
            CreateCustomerResponse createCustomerResponse = new();
            int userid = 0; //帳號驗證取得ID

            Customer? customerQuery = dbContext.Customers
                                .FirstOrDefault(customer => customer.Code == customerForm.Code);

            if (customerQuery == null)
            {
                Customer dbCustomer = mapper.Map<Customer>(customerForm);
                BaseInputCustomer(dbCustomer, true, userid);
                dbContext.Customers.Add(dbCustomer);
                dbContext.SaveChanges();

                //回傳剛建立的客戶基本資料 使建立客戶印鑑找到該ID
                Customer? customer = dbContext.Customers.Find(dbCustomer.Id);
                                     
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
        /// 更新基本資料
        /// </summary>
        /// <param name="customerFormUpdate">基本資料</param>
        /// <returns></returns>
        public ResponseViewModel Update(CustomerUpdateForm customerFormUpdate)
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
        /// 刪除基本資料，
        /// 此刪除為更動狀態使其一般使用者看不到資料，
        /// 而不是真正的刪除。。
        /// </summary>
        /// <param name="customerId">客戶ID</param>        
        public ResponseViewModel Delete(int customerId)
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
                customer.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                customer.UpdateUserId = userid;
                customer.UpdateDate = DateTime.Now;
            }
        }
    }
}