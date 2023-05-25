using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using AutoMapper;
using SealTypographicWebAPI.Utils;
using Microsoft.EntityFrameworkCore;
using DBEntities;
using DBEntities.Consts;

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
                customerDetailViewModel.Success();
            }
            else
            {
                customerDetailViewModel.CustomeNoData();
            }
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
            int companyId = 1;           

            IQueryable<Customer> customerQuery = GetCustomers(companyId, customerSearch.KeyWord);

            if (customerQuery.Any())
            {
                //取得該頁            
                List<Customer> pageNumberCustomers = customerQuery
                                          .Skip((customerSearch.PageNumber - 1) * customerSearch.PageSize)
                                          .Take(customerSearch.PageSize)                                   
                                          .ToList();
                
                foreach (Customer customer in pageNumberCustomers)
                {
                    CustomerViewModel customerViewModel = mapper.Map<CustomerViewModel>(customer);

                    List<CustomerSealGroup> customerSealGroups = dbContext.CustomerSealGroups
                                                                        .Where(x => x.Customer.Id == customer.Id
                                                                        && x.DeleteStatus == DeleteStatus.No
                                                                        && x.ReviewStatus < ReviewStatus.Disabled ).ToList();

                    if(customerSealGroups.Any())
                    {
                        //取得狀態
                        if (!customerSealGroups.Where(x => x.ReviewStatus != ReviewStatus.Approval).Any())
                        {
                            customerViewModel.IsDraff = false;
                            customerViewModel.IsPending = false;
                            customerViewModel.IsReject = false;
                        }
                        else
                        {
                            if (customerSealGroups.Where(x => x.ReviewStatus == ReviewStatus.Draft).Any())
                            {
                                customerViewModel.IsDraff = true;
                            }
                            if (customerSealGroups.Where(x => x.ReviewStatus == ReviewStatus.Pending).Any())
                            {
                                customerViewModel.IsPending = true;
                            }
                            if (customerSealGroups.Where(x => x.ReviewStatus == ReviewStatus.Reject).Any())
                            {
                                customerViewModel.IsReject = true;
                            }
                        }
                        customerViewModel.CustomerSealQuarterId = customerSealGroups.OrderByDescending(x => x.Quarter).Select(x => x.Id).FirstOrDefault();                        
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
            int companyId = 1;
            
            //尋找公司並與客戶關聯
            Company? companyQuery = dbContext.Companys.Include(x => x.Customers).FirstOrDefault(x => x.Id == companyId);

            if(companyQuery != null)
            {
                //驗證編號是否重複
                List<string> customerQuery = companyQuery.Customers.Where
                                            (                                                
                                                x => x.Code == customerForm.Code
                                                && x.DeleteStatus == DeleteStatus.No
                                            ).Select(x => x.Code).ToList();
                if (!customerQuery.Any())
                {
                    Customer dbCustomer = mapper.Map<Customer>(customerForm);
                    BaseInputCustomer(dbCustomer, true, userid);
                    companyQuery.Customers.Add(dbCustomer);
                    dbContext.SaveChanges();

                    if (dbCustomer != null)
                    {
                        //回傳剛建立的客戶基本資料 使建立客戶印鑑找到該ID   
                        createCustomerResponse.CustomerId = dbCustomer.Id;
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
        /// 資料新增修改時基本資料輸入
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

        /// <summary>
        /// 取得關鍵字模糊搜尋時獲得的內容
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        private IQueryable<Customer> GetCustomers (int companyId, string? keyWord)
        {
            IQueryable<Customer> customerQuery = dbContext.Customers.Where
                                                (
                                                    x => x.Company.Id == companyId
                                                    && x.DeleteStatus == DeleteStatus.No
                                                );

            if (!string.IsNullOrWhiteSpace(keyWord))
            {
                customerQuery = customerQuery.Where
                    (
                        customer =>
                        customer.Code.ToLower().Contains(keyWord.ToLower())
                        || customer.Name.Contains(keyWord)
                    );
            }
            customerQuery = customerQuery.OrderBy(customer => customer.Code);

            return customerQuery;
        }
    }
}