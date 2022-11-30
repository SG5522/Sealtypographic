using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Util;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 勤業用的顧客印鑑組
    /// </summary>
    public class CustomerSealDeloitteService : ICustomerSealService
    {
        private readonly SealTypographicDbContext dbContext;        
        private readonly IMapper mapper;
        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        public CustomerSealDeloitteService(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;            
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得顧客印鑑季度表
        /// </summary>
        /// <param name="customerId">客戶ID</param>        
        /// <returns></returns>
        public CustomerSealQuarters GetCustomerSealQuarters(string customerId)
        {
            List<CustomerSealQuarter> customerSealQuarters = new();
            ResponseViewModel response;
            List<string> customerSealQuarterQuery = dbContext.CustomerSealJournals
                                           .Where(customerSealJournal => customerSealJournal.CustomerId == customerId)
                                           .Select(customerSealJournal => customerSealJournal.Quarter)
                                           .Distinct()
                                           .ToList();

            if (customerSealQuarterQuery.Any())
            {
                foreach (string quarter in customerSealQuarterQuery)
                {
                    customerSealQuarters.Add(new CustomerSealQuarter
                    {
                        CustomerId = customerId,
                        Quarter = quarter
                    });
                }
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.NoData();
            }

            return new CustomerSealQuarters()
            {

                Code = response.Code,
                Message = response.Message,

                Quarters = customerSealQuarters
            };
        }

        /// <summary>
        /// 取得客戶印鑑組
        /// </summary>
        /// <param name="customerSealQuarter">搜尋條件</param>
        /// <returns></returns>
        public CustomerSealViewModels GetCustomerSealViewModels(CustomerSealQuarter customerSealQuarter)
        {
            List<CustomerSealViewModel> customerSealViewModels = new();
            ResponseViewModel response;
            List<CustomerSealJournal> customerSealQuery = dbContext.CustomerSealJournals.Where
                                                                    (
                                                                        customerSealJournal => customerSealJournal.CustomerId == customerSealQuarter.CustomerId
                                                                        && customerSealJournal.Quarter == customerSealQuarter.Quarter
                                                                    )
                                                                    .Include(customerSealJournal => customerSealJournal.SealMappingConfig)
                                                                    .OrderBy(customerSealJournal => customerSealJournal.SealMappingConfigId)
                                                                    .ToList();
            if (customerSealQuery.Any())
            {
                foreach (CustomerSealJournal customerSealJournal in customerSealQuery)
                {
                    CustomerSealViewModel customerSealViewModel = mapper.Map<CustomerSealViewModel>(customerSealJournal);
                    customerSealViewModel.ImageBase64 = "image/..."; //之後會在做BASE64轉換
                    customerSealViewModels.Add(customerSealViewModel);
                }
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.NoData();
            }


            return new CustomerSealViewModels()
            {
                Code = response.Code,
                Message = response.Message,

                SealViewModels = customerSealViewModels
            };
        }

        /// <summary>
        /// 新增印鑑組
        /// </summary>
        /// <param name="customerSeals">印鑑組</param>
        /// <returns></returns>
        public ResponseViewModel CreateCustomerSeals(List<CustomerSealForm> customerSeals)
        {
            ResponseViewModel response = new();
            List<CustomerSealJournal> customerSealJournals = new();
            foreach (CustomerSealForm customerSeal in customerSeals)
            {
                //這段之後會做成IMAGE64的處理並另存在指定的位置
                string imagePath = customerSeal.ImageBase64;

                CustomerSealJournal customerSealJournal = mapper.Map<CustomerSealJournal>(customerSeal);
                customerSealJournal.ImagePath = imagePath;
                customerSealJournal.CreateDate = DateTime.Now;
                customerSealJournals.Add(customerSealJournal);
            }
            dbContext.CustomerSealJournals.AddRange(customerSealJournals);
            dbContext.BulkSaveChanges();
            response = ResponseUtil.Success();

            return response;
        }

        /// <summary>
        /// 修改印鑑組
        /// </summary>
        /// <param name="customerSeals">印鑑組</param>
        /// <returns></returns>
        public ResponseViewModel UpdateCustomerSeals(List<CustomerSealFormWithID> customerSeals)
        {
            ResponseViewModel response = new();
            foreach (CustomerSealFormWithID customerSeal in customerSeals)
            {
                CustomerSealJournal? customerSealJournalQuery = dbContext.CustomerSealJournals.Where
                                           (
                                                customerSealJournal =>
                                                customerSealJournal.Id == customerSeal.Id
                                           ).FirstOrDefault();
                if (customerSealJournalQuery != null)
                {
                    //這段之後會做成IMAGE64的處理並另存在指定的位置
                    string imagePath = customerSeal.ImageBase64;
                    customerSealJournalQuery.ImagePath = imagePath;
                    mapper.Map(customerSeal, customerSealJournalQuery);                    
                }
                else
                {
                    response = ResponseUtil.NoData();
                    return response;
                }
            }            
            dbContext.SaveChanges();
            response = ResponseUtil.Success();
            return response;
        }
    }
}
