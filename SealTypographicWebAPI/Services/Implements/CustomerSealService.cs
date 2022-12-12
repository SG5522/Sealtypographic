using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Util;
using SealTypographicWebAPI.Utils;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 勤業用的顧客印鑑組
    /// </summary>
    public class CustomerSealService : ICustomerSealService
    {
        private readonly SealTypographicDbContext dbContext;        
        private readonly IMapper mapper;
        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        public CustomerSealService(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;            
            this.mapper = mapper;
        }
        /// <summary>
        /// 取得顧客印鑑季度表
        /// </summary>
        /// <param name="customerId">客戶ID</param>        
        /// <returns></returns>
        public CustomerSealQuarters GetCustomerSealQuarters(int customerId)
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
                    customerSealQuarters.Add(new()
                    {
                        CustomerId = customerId,
                        Quarter = quarter
                    });
                }
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.DbNoData();
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
                response = ResponseUtil.DbNoData();
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
                if (CheckRepeatSequence(mapper.Map<CustomerSealSequenceCheck>(customerSeal))) //確認序號是否重複
                {
                    //這段之後會做成IMAGE64的處理並另存在指定的位置
                    string imagePath = customerSeal.ImageBase64;

                    CustomerSealJournal customerSealJournal = mapper.Map<CustomerSealJournal>(customerSeal);
                    customerSealJournal.ImagePath = imagePath;
                    customerSealJournal.CreateDate = DateTime.Now;
                    customerSealJournal.StartDate = AvailableDateUtil.NotActivated();
                    customerSealJournal.EndDate = AvailableDateUtil.NotActivated(); //暫時加上                
                    customerSealJournal.ReviewStatus = ReviewStatus.Pending;
                    customerSealJournal.DeleteStatus = DeleteStatus.NO;
                    customerSealJournals.Add(customerSealJournal);
                }
                else
                {
                    return ResponseUtil.SealSequenceError();
                }
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
        public ResponseViewModel UpdateCustomerSeals(List<CustomerSealFormUpdate> customerSeals)
        {
            ResponseViewModel response = new();
            foreach (CustomerSealFormUpdate customerSeal in customerSeals)
            {
                CustomerSealJournal? customerSealJournalQuery = dbContext.CustomerSealJournals.Where
                                           (
                                                customerSealJournal =>
                                                customerSealJournal.Id == customerSeal.Id
                                           ).FirstOrDefault();
                if (customerSealJournalQuery != null)
                {
                    bool sequenceRepeatCheck = true;

                    //印鑑序號如果是新的就進行重複確認
                    if(customerSealJournalQuery.Sequence != customerSeal.Sequence)
                    {
                        CustomerSealSequenceCheck customerSealSequenceCheck = mapper.Map<CustomerSealSequenceCheck>(customerSealJournalQuery);
                        customerSealSequenceCheck.Sequence = customerSeal.Sequence;
                        sequenceRepeatCheck = CheckRepeatSequence(customerSealSequenceCheck);
                    }
                    
                    if (sequenceRepeatCheck)
                    {
                        //這段之後會做成IMAGE64的處理並另存在指定的位置
                        string imagePath = customerSeal.ImageBase64;
                        mapper.Map(customerSeal, customerSealJournalQuery);

                        customerSealJournalQuery.ImagePath = imagePath;
                        customerSealJournalQuery.UpdateDate = DateTime.Now;
                        customerSealJournalQuery.StartDate = AvailableDateUtil.NotActivated();
                        customerSealJournalQuery.EndDate = AvailableDateUtil.NotActivated(); //暫時加上
                        customerSealJournalQuery.ReviewStatus = ReviewStatus.Pending;
                    }
                    else
                    {
                        return ResponseUtil.SealSequenceError();
                    }
                }
                else
                {
                    response = ResponseUtil.DbNoData();
                    return response;
                }
            }            
            dbContext.SaveChanges();
            response = ResponseUtil.Success();
            return response;
        }

        /// <summary>
        /// 變更此客戶狀態為刪除。
        /// </summary>
        /// <param name="customerSealId">客戶ID</param>        
        public ResponseViewModel DeleteCustomerSeal(int customerSealId)
        {
            ResponseViewModel response = new();
            CustomerSealJournal? customerSealJournalQuery = dbContext.CustomerSealJournals
                                .Where(customerSealJournal => customerSealJournal.Id == customerSealId)
                                .FirstOrDefault();

            if (customerSealJournalQuery != null)
            {
                customerSealJournalQuery.DeleteStatus = DeleteStatus.Yes;
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
        /// 確認客戶序號是否重複 true 不重複 false 重複
        /// </summary>
        /// <param name="customerSealSequenceCheck">查詢參數</param>
        /// <returns></returns>
        private bool CheckRepeatSequence(CustomerSealSequenceCheck customerSealSequenceCheck)
        {
            CustomerSealJournal? customerSealQuery = dbContext.CustomerSealJournals
                                .Where
                                (
                                    customerSealJournal => customerSealJournal.CustomerId == customerSealSequenceCheck.CustomerId
                                    && customerSealJournal.SealMappingConfigId == customerSealSequenceCheck.SealMappingConfigId
                                    && customerSealJournal.Quarter == customerSealSequenceCheck.Quarter
                                    && customerSealJournal.Sequence == customerSealSequenceCheck.Sequence
                                    && customerSealJournal.DeleteStatus == DeleteStatus.NO
                                ).FirstOrDefault();

            if(customerSealQuery == null )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
