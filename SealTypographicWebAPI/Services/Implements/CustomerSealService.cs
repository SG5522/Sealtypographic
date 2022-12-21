using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Util;
using SealTypographicWebAPI.Utils;
using System.Linq;

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
            CustomerSealQuarters customerSealQuarters = new();
            List<CustomerSealQuarter> sealQuarters = new();
            List<string> customerSealQuarterQuery = dbContext.CustomerSealJournals
                                           .Where
                                           (
                                                customerSealJournal => customerSealJournal.CustomerId == customerId
                                                //&& customerSealJournal.ReviewStatus != ReviewStatus.Pending //待審狀態過濾用
                                           )
                                           .Select(customerSealJournal => customerSealJournal.Quarter)
                                           .Distinct()
                                           .ToList();

            if (customerSealQuarterQuery.Any())
            {
                foreach (string quarter in customerSealQuarterQuery)
                {
                    sealQuarters.Add(new()
                    {
                        CustomerId = customerId,
                        Quarter = quarter
                    });
                }
                customerSealQuarters.Quarters = sealQuarters;
                customerSealQuarters.Success();
            }
            else
            {
                customerSealQuarters.DbNoData();
            }

            return customerSealQuarters;
        }

        /// <summary>
        /// 取得客戶印鑑組
        /// </summary>
        /// <param name="customerSealQuarter">搜尋條件</param>
        /// <returns></returns>
        public CustomerSealViewModels GetCustomerSealViewModels(CustomerSealQuarter customerSealQuarter)
        {
            CustomerSealViewModels customerSealViewModels = new();
            List<CustomerSealViewModel> sealViewModels = new();

            customerSealViewModels.CustomerId = customerSealQuarter.CustomerId;
            customerSealViewModels.Quarter = customerSealQuarter.Quarter;
            List<CustomerSealJournal> customerSealQuery = dbContext.CustomerSealJournals.Where
                                                                    (
                                                                        customerSealJournal => customerSealJournal.CustomerId == customerSealQuarter.CustomerId
                                                                        && customerSealJournal.Quarter == customerSealQuarter.Quarter
                                                                        && customerSealJournal.DeleteStatus == DeleteStatus.NO
                                                                    )
                                                                    .Include(customerSealJournal => customerSealJournal.SealMappingConfig)
                                                                    .OrderBy(customerSealJournal => customerSealJournal.SealMappingConfigId)
                                                                    .ThenBy(customerSealJournal => customerSealJournal.Sequence)
                                                                    .ToList();
            //if (!customerSealQuery.Any(customerSealJournal => customerSealJournal.ReviewStatus == ReviewStatus.Pending))
            if (customerSealQuery.Any())
            {
                foreach (CustomerSealJournal customerSealJournal in customerSealQuery)
                {
                    CustomerSealViewModel customerSealViewModel = mapper.Map<CustomerSealViewModel>(customerSealJournal);
                    customerSealViewModel.ImageBase64 = customerSealJournal.ImagePath; //之後會在做BASE64轉換                    
                    sealViewModels.Add(customerSealViewModel);
                }

                if (customerSealQuery.Any(customerSealJournal => customerSealJournal.ReviewStatus == ReviewStatus.Reject))
                {
                    customerSealViewModels.ReviewStatus = ReviewStatusUtil.Reject();
                }
                else
                {
                    customerSealViewModels.ReviewStatus = ReviewStatusUtil.Approval();
                }

                customerSealViewModels.SealViewModels = sealViewModels;
                customerSealViewModels.Success();
            }
            else
            {
                customerSealViewModels.DbNoData();
            }

            return customerSealViewModels;
        }

        /// <summary>
        /// 新增印鑑組
        /// </summary>
        /// <param name="customerSeals">印鑑組</param>
        /// <returns></returns>
        public ResponseViewModel CreateCustomerSeals(List<CustomerSealForm> customerSeals)
        {
            List<CustomerSealJournal> customerSealJournals = new();
            foreach (CustomerSealForm customerSeal in customerSeals)
            {
                if (CheckRepeatSequence(mapper.Map<CustomerSealSequenceCheck>(customerSeal))) //確認序號是否重複
                {
                    //這段之後會做成IMAGE64的處理並另存在指定的位置
                    string imagePath = customerSeal.ImageBase64;

                    CustomerSealJournal customerSealJournal = mapper.Map<CustomerSealJournal>(customerSeal);
                    customerSealJournal.ImagePath = imagePath;
                    BaseInputCustomerSealJournal(customerSealJournal, true);
                    customerSealJournals.Add(customerSealJournal);
                }
                else
                {
                    return ResponseUtil.CustomerSealSequenceRepeat();
                }
            }
            dbContext.CustomerSealJournals.AddRange(customerSealJournals);
            dbContext.BulkSaveChanges();

            return ResponseUtil.Success();
        }

        /// <summary>
        /// 異動客戶印鑑的處理
        /// </summary>
        /// <param name="customerSealUpdate">刪除修改新增的list</param>
        /// <returns></returns>
        public List<ResponseViewModel> UpdateCustomerSeals(CustomerSealUpdate customerSealUpdate)
        {
            List<ResponseViewModel> responseViewModels = new();                        
            List<CustomerSealJournal> customerSealJournals = new();
            ResponseViewModel response = new();
            //刪除印鑑
            foreach (int customerSealId in customerSealUpdate.DeleteCustomerSealIds)
            {
                CustomerSealJournal? deletecustomerSealQuery = dbContext.CustomerSealJournals.FirstOrDefault
                                                                (
                                                                    customerSeal => customerSeal.Id == customerSealId
                                                                    && customerSeal.DeleteStatus == DeleteStatus.NO
                                                                );
                if (deletecustomerSealQuery != null)
                {
                    deletecustomerSealQuery.DeleteStatus = DeleteStatus.Yes;
                    deletecustomerSealQuery.UpdateDate = DateTime.Now;                    
                }
                else
                {
                    //ResponseViewModel response = new();
                    response.CustomerSealNoData();
                    response.ErrorItem = "Delete CustomerSealId:" + customerSealId;
                    responseViewModels.Add(response);
                }
            }
            //修改印鑑
            foreach (CustomerSealFormUpdate customerSealFormUpdate in customerSealUpdate.UpdateCustomerSeals)
            {
                CustomerSealJournal? UpdatecustomerSealQuery = dbContext.CustomerSealJournals.FirstOrDefault
                                                            (
                                                                customerSeal => customerSeal.Id == customerSealFormUpdate.Id
                                                                && customerSeal.DeleteStatus == DeleteStatus.NO
                                                            );
                if (UpdatecustomerSealQuery != null)
                {
                    mapper.Map(customerSealFormUpdate, UpdatecustomerSealQuery);
                    string imagePath = customerSealFormUpdate.ImageBase64;//這段之後會做成IMAGE64的處理並另存在指定的位置

                    UpdatecustomerSealQuery.ImagePath = imagePath;
                    BaseInputCustomerSealJournal(UpdatecustomerSealQuery, false);
                }
                else
                {
                    //ResponseViewModel response = new();
                    response.UpdateCustomerSealNoData();                    
                    response.ErrorItem = "Update CustomerSealId:" + customerSealFormUpdate.Id;
                    responseViewModels.Add(response);
                }
            }
            //新增印鑑
            foreach (CustomerSealForm createCustomerSeal in customerSealUpdate.CreateCustomerSeals)
            {
                if (CheckRepeatSequence(mapper.Map<CustomerSealSequenceCheck>(createCustomerSeal))) //確認序號是否重複
                {
                    //這段之後會做成IMAGE64的處理並另存在指定的位置
                    string imagePath = createCustomerSeal.ImageBase64;

                    CustomerSealJournal customerSealJournal = mapper.Map<CustomerSealJournal>(createCustomerSeal);
                    customerSealJournal.ImagePath = imagePath;
                    BaseInputCustomerSealJournal(customerSealJournal, true);
                    customerSealJournals.Add(customerSealJournal);
                }
                else
                {
                    //ResponseViewModel response = new();
                    response.CreateCustomerSealSequenceRepeat();
                    response.ErrorItem = "Create CustomerId:" + createCustomerSeal.CustomerId
                                       + " SealMappingConfigId:" + createCustomerSeal.SealMappingConfigId
                                       + " Sequence:" + createCustomerSeal.Sequence;
                    responseViewModels.Add(response);
                }
            }            
            //更新資料庫
            if (!responseViewModels.Any())
            {
                //ResponseViewModel response = new();
                dbContext.CustomerSealJournals.AddRange(customerSealJournals);
                dbContext.BulkSaveChanges();
                response.Success();
                responseViewModels.Add(response);                
            }            

            return responseViewModels;
        }

        /// <summary>
        /// 變更此客戶狀態為刪除。
        /// </summary>
        /// <param name="customerSealId">客戶ID</param>        
        public ResponseViewModel DeleteCustomerSeal(int customerSealId)
        {
            ResponseViewModel response = new();
            CustomerSealJournal? customerSealJournalQuery = dbContext.CustomerSealJournals.Find(customerSealId);

            if (customerSealJournalQuery != null)
            {
                customerSealJournalQuery.DeleteStatus = DeleteStatus.Yes;
                customerSealJournalQuery.UpdateDate = DateTime.Now;
                //排序印鑑
                ReSequece(customerSealJournalQuery);
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.DeleteCustomerSealNoData();
            }
            return response;
        }

        /// <summary>
        /// 客戶印鑑新增修改時基本的資料輸入
        /// </summary>
        /// <param name="customerSealJournal">Db上的印鑑資料</param>
        /// <param name="isCreate">對Db所做的行動</param>
        private static void BaseInputCustomerSealJournal(CustomerSealJournal customerSealJournal, bool isCreate)
        {
            if (isCreate)
            {
                customerSealJournal.CreateDate = DateTime.Now;
                customerSealJournal.DeleteStatus = DeleteStatus.NO;
            }
            else
            {
                customerSealJournal.UpdateDate = DateTime.Now;
            }
            customerSealJournal.StartDate = AvailableDateUtil.NotActivated();
            customerSealJournal.EndDate = AvailableDateUtil.NotActivated(); //暫時加上                
            customerSealJournal.ReviewStatus = ReviewStatus.Pending;
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

            if (customerSealQuery == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 刪除印鑑時重新排序
        /// </summary>
        /// <param name="deleteCustomerSeal"></param>
        private void ReSequece(CustomerSealJournal deleteCustomerSeal)
        {
            int sequence = 0;
            IQueryable<CustomerSealJournal> customerSealJournalQuery = dbContext.CustomerSealJournals.Where
                                                                        (
                                                                            customerSeal => customerSeal.Quarter == deleteCustomerSeal.Quarter
                                                                            && customerSeal.SealMappingConfigId == deleteCustomerSeal.SealMappingConfigId
                                                                            && customerSeal.CustomerId == deleteCustomerSeal.CustomerId
                                                                            && customerSeal.DeleteStatus == DeleteStatus.NO
                                                                        )
                                                                        .OrderBy(customerSeal => customerSeal.Sequence);

            if (customerSealJournalQuery.Any())
            {
                if (deleteCustomerSeal.Sequence == 1)
                {
                    sequence = 1;
                    foreach (CustomerSealJournal customerSeal in customerSealJournalQuery)
                    {
                        customerSeal.Sequence = sequence;
                        customerSeal.UpdateDate = DateTime.Now;
                        sequence++;
                    }
                    dbContext.SaveChanges();
                }
                else
                {
                    if (deleteCustomerSeal.Sequence < customerSealJournalQuery.Last().Sequence)//如果刪除的序號是最後一個就不用做任何處理
                    {
                        IQueryable<CustomerSealJournal> moreThanTheSequenceDeleteCustomerSeals = customerSealJournalQuery
                                                                    .Where(customerSeal => customerSeal.Sequence > deleteCustomerSeal.Sequence);
                        if (moreThanTheSequenceDeleteCustomerSeals.Any())
                        {
                            sequence = deleteCustomerSeal.Sequence;
                            foreach (CustomerSealJournal customerSeal in moreThanTheSequenceDeleteCustomerSeals)
                            {
                                customerSeal.Sequence = sequence;
                                customerSeal.UpdateDate = DateTime.Now;
                                sequence++;
                            }
                            dbContext.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
