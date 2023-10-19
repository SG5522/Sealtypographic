using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using AutoMapper;
using SealTypographicWebAPI.Utils;
using Microsoft.EntityFrameworkCore;
using DBEntities;
using DBEntities.Consts;
using AutoMapper.QueryableExtensions;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 客戶資料管理
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly SealTypographicDbContext dbContext;        
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private readonly ILogger<CustomerService> logger;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public CustomerService(SealTypographicDbContext dbContext, IMapper mapper, ILogger<CustomerService> logger)
        {
            this.dbContext = dbContext;            
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;
            this.logger = logger;
        }

        /// <summary>
        /// 取得客戶詳細基本資料
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        public CustomerDetailViewModel GetDetail(int customerId)
        {
            logger.LogInformation("GetDetail input customerId {@input}", customerId);
            CustomerDetailViewModel customerDetailViewModel = new();                                    

            try
            {
                CustomerDetail? customerDetail = dbContext.Customers
                                .Where(x => x.Id == customerId)
                                .ProjectTo<CustomerDetail>(configurationProvider)
                                .FirstOrDefault();

                if (customerDetail != null)
                {
                    customerDetailViewModel.CustomerDetail = customerDetail;
                    customerDetailViewModel.Success();
                }
                else
                {
                    customerDetailViewModel.CustomeNoData();
                }
                logger.LogInformation("GetDetail output {@output}",customerDetailViewModel);
            }
            catch (Exception ex) 
            {
                customerDetailViewModel.Error();
                logger.LogError("GetDetail error {@error}", ex.Message);
            }

            return customerDetailViewModel;
        }

        /// <summary>
        /// 取得客戶資料列表(分頁)
        /// </summary>
        /// <param name="customerSearch">客戶分頁搜尋</param>        
        /// <returns></returns>
        public CustomerPaginateSummary GetCustomerPaginate(CustomerSearch customerSearch) 
        {
            logger.LogInformation("GetCustomerPaginate input {@input}", customerSearch);

            CustomerPaginateSummary customerPaginateSummary = new();
            int companyId = 1;

            try
            {
                IQueryable<Customer> customerQuery = dbContext.Customers.Where
                                                (
                                                    x => x.Company.Id == companyId
                                                    && x.DeleteStatus == DeleteStatus.No
                                                );

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
                    customerPaginateSummary.Summarys = customerQuery
                                                        .Skip((customerSearch.PageNumber - 1) * customerSearch.PageSize)
                                                        .Take(customerSearch.PageSize)
                                                        .ProjectTo<CustomerSummary>(configurationProvider)
                                                        .ToList();
                    
                    PageUtil.GetPageData(customerPaginateSummary, customerSearch.PageNumber, customerSearch.PageSize, customerQuery.Count());
                    customerPaginateSummary.Success();
                }
                else
                {
                    customerPaginateSummary.CustomeNoData();
                }
                logger.LogInformation("GetCustomerPaginate output {@output}", customerPaginateSummary);
            }
            catch (Exception ex)
            {
                customerPaginateSummary.Error();
                logger.LogError("GetCustomerPaginate error {@error}", ex.Message);
            }            

            return customerPaginateSummary;
        }


        /// <summary>
        /// 取得客戶資料列表(分頁)
        /// </summary>
        /// <param name="customerSearch">客戶分頁搜尋</param>
        /// <param name="isTypographicUse">是否給排版使用</param>  
        /// <returns></returns>
        public CustomerPaginateViewModel GetPaginate(CustomerSearch customerSearch, bool isTypographicUse)
        {
            logger.LogInformation("GetPaginate input {@input} isTypographicUse: {@isTypographicUse}", customerSearch, isTypographicUse);

            CustomerPaginateViewModel customerPaginateViewModel = new();
            int companyId = 1;           
            
            try
            {
                IQueryable<Customer> customerQuery = dbContext.Customers.Where
                                                (
                                                    x => x.Company.Id == companyId
                                                    && x.DeleteStatus == DeleteStatus.No
                                                );

                if (isTypographicUse)
                {
                    customerQuery = customerQuery.Where(accountant => accountant.CustomerSealGroups.Any(x => x.ReviewStatus == ReviewStatus.Approval));
                }

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
                    List<CustomerViewModel> thisPageCustomers = customerQuery
                                                                .Skip((customerSearch.PageNumber - 1) * customerSearch.PageSize)
                                                                .Take(customerSearch.PageSize)
                                                                .ProjectTo<CustomerViewModel>(configurationProvider)
                                                                .ToList();

                    foreach (CustomerViewModel customerViewModel in thisPageCustomers)
                    {
                        IQueryable<CustomerSealGroup> customerSealGroups = dbContext.CustomerSealGroups
                                                                            .Where(x => x.Customer.Id == customerViewModel.Id
                                                                            && x.DeleteStatus == DeleteStatus.No
                                                                            && x.ReviewStatus < ReviewStatus.Disabled);

                        if (customerSealGroups.Any())
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
                            customerViewModel.CustomerSealQuarterId = customerSealGroups.OrderByDescending(x => x.QuarterYear).Select(x => x.Id).FirstOrDefault();
                        }

                        customerPaginateViewModel.ViewModels.Add(customerViewModel);
                    }
                   
                    PageUtil.GetPageData(customerPaginateViewModel, customerSearch.PageNumber, customerSearch.PageSize, customerQuery.Count());
                    customerPaginateViewModel.Success();
                }
                else
                {
                    customerPaginateViewModel.CustomeNoData();
                }

                
            }
            catch (Exception ex)
            {
                customerPaginateViewModel.Error();
                logger.LogError("GetPaginate error {@error}", ex.Message);
            }            

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
                    BaseInput(dbCustomer, true, userid);
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
                BaseInput(customerQuery, false, userId);
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
                BaseInput(customerQuery, false, userId);
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
        private static void BaseInput(Customer customer, bool isCreate, int userid)
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