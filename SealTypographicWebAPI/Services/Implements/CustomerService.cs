using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using AutoMapper;
using SealTypographicWebAPI.Utils;
using Microsoft.EntityFrameworkCore;
using DBEntities.Consts;
using AutoMapper.QueryableExtensions;
using SealTypographicWebAPI.Models.Accountant;
using DBEntities.Entities;
using DBEntities;
using DBEntities.Entities.CustomerModels;
using DBEntities.Utils;

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

        ///<inheritdoc />
        public CustomerDetailViewModel GetDetail(int customerId, int userId = 1)
        {
            logger.LogInformation("GetDetail input customerId: {@customerId} userId: {@userId}", customerId, userId);
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

        ///<inheritdoc />
        public CustomerPaginateSummary GetPaginate(CustomerSearch customerSearch, int userId = 1) 
        {
            logger.LogInformation("GetCustomerPaginate input {@customerSearch} userId: {@userId}", customerSearch, userId);

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
                        || customer.Name.ToLower().Contains(customerSearch.KeyWord.ToLower())
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
                    
                    PageUtil.SetPaginate(customerPaginateSummary, customerSearch.PageNumber, customerSearch.PageSize, customerQuery.Count());
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

        ///<inheritdoc />
        public CreateCustomerResponse New(CustomerForm customerForm, int userId = 1)
        {
            logger.LogInformation("New input {@customerForm} userId: {@userId}", customerForm, userId);

            CreateCustomerResponse createCustomerResponse = new();            
            int companyId = 1;
            
            try
            {
                //尋找公司並與客戶關聯
                Company? companyQuery = dbContext.Companys.Include(x => x.Customers).FirstOrDefault(x => x.Id == companyId);

                if (companyQuery != null)
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
                        InputUtil.Set(dbCustomer, true, userId);
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
                else
                {
                    createCustomerResponse.DbNoData();
                }
                logger.LogInformation("New output {@output}", createCustomerResponse);
            }
            catch (DbUpdateException ex)
            {
                createCustomerResponse.DbError();
                logger.LogInformation("New dbError {@dbError}", ex.Message);
            }
            catch (Exception ex)
            {
                createCustomerResponse.Error();
                logger.LogInformation("New error {@error}", ex.Message);
            }                                
            
            return createCustomerResponse;
        }

        ///<inheritdoc />
        public ResponseViewModel Update(CustomerUpdateForm customerFormUpdate, int userId = 1)
        {
            logger.LogInformation("Update input {@customerFormUpdate} userId: {@userId}", customerFormUpdate, userId);

            ResponseViewModel response = new();            

            try
            {
                Customer? customerQuery = dbContext.Customers.Find(customerFormUpdate.Id);

                if (customerQuery != null)
                {
                    mapper.Map(customerFormUpdate, customerQuery);                    
                    InputUtil.Set(customerQuery, false, userId);
                    dbContext.SaveChanges();
                    response.Success();
                }
                else
                {
                    response.UpdateCustomerNoData();
                }
                logger.LogInformation("Update output {@output}", response);
            }
            catch (DbUpdateException ex)
            {
                response.DbError();
                logger.LogInformation("Update dbError {@dbError}", ex.Message);
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogInformation("Update error {@error}", ex.Message);
            }
            
            return response;
        }

        ///<inheritdoc /> 
        public ResponseViewModel Delete(int customerId, int userId = 1)
        {
            logger.LogInformation("Delete input customerId: {@customerId} userId: {@userId}", customerId, userId);
            
            ResponseViewModel response = new();            

            try
            {
                Customer? customerQuery = dbContext.Customers.Find(customerId);

                if (customerQuery != null)
                {
                    customerQuery.DeleteStatus = DeleteStatus.Yes;                   
                    InputUtil.Set(customerQuery, false, userId);
                    dbContext.SaveChanges();
                    response.Success();
                }
                else
                {
                    response.DeleteCustomerNoData();
                }
                logger.LogInformation("Update output {@output}", response);
            }
            catch (DbUpdateException ex)
            {
                response.DbError();
                logger.LogInformation("Delete dbError {@dbError}", ex.Message);
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogInformation("Delete error {@error}", ex.Message);
            }

            return response;
        }
    }
}