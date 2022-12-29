using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Customer;
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
        public CustomerSealQuarterViews GetCustomerSealQuarters(int customerId)
        {            
            CustomerSealQuarterViews customerSealQuarters = new();
            List<CustomerSealQuarterView> customerSealQuarterQuery = dbContext.CustomerSealJournals
                                           .Where
                                           (
                                                customerSealJournal => customerSealJournal.CustomerId == customerId
                                                && customerSealJournal.ReviewStatus <= ReviewStatus.Draft //過濾待審或退件的狀態                                                
                                                && customerSealJournal.DeleteStatus == DeleteStatus.NO
                                           )
                                           .Select(customerSealJournal => new CustomerSealQuarterView()
                                           {
                                               CustomerId = customerSealJournal.CustomerId,
                                               Quarter = customerSealJournal.Quarter,
                                               ReviewStatus = customerSealJournal.ReviewStatus
                                           })
                                           .GroupBy(customerSealJournal => customerSealJournal.Quarter)
                                           .OrderByDescending(g => g.Key)
                                           .Select(customerSealJournal => customerSealJournal.First())
                                           .ToList();                                                                                    


            if (customerSealQuarterQuery.Any())
            {
                customerSealQuarters.Quarters = customerSealQuarterQuery;
                customerSealQuarters.Success();
            }
            else
            {
                customerSealQuarters.CustomerSealNoData();
            }

            return customerSealQuarters;
        }

        /// <summary>
        /// 取得客戶印鑑組
        /// </summary>
        /// <param name="customerSealQuarter">搜尋條件</param>
        /// <returns></returns>
        public CustomerSealViewModels GetCustomerSealViewModels(CustomerSealQuarterSearch customerSealQuarter)
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
            if (customerSealQuery.Any())
            {
                foreach (CustomerSealJournal customerSealJournal in customerSealQuery)
                {
                    CustomerSealViewModel customerSealViewModel = mapper.Map<CustomerSealViewModel>(customerSealJournal);                    
                    customerSealViewModel.ImageBase64 = customerSealJournal.ImagePath; //之後會在做BASE64轉換
                    
                    sealViewModels.Add(customerSealViewModel);
                }
                customerSealViewModels.ReviewStatus = customerSealQuery.First().ReviewStatus;
                customerSealViewModels.SealViewModels = sealViewModels;
                customerSealViewModels.Success();
            }
            else
            {
                customerSealViewModels.CustomerSealNoData();
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
            ResponseViewModel response = new();
            List<CustomerSealJournal> customerSealJournals = new();
            int userId = 0; //以後從帳號驗證取得Id

            CustomerSealJournal? customerSealJournalQuery = dbContext.CustomerSealJournals
                                                        .FirstOrDefault
                                                        (
                                                            x => x.CustomerId == customerSeals.First().CustomerId
                                                            && x.Quarter == customerSeals.First().Quarter                        
                                                        );
            if(customerSealJournalQuery == null)
            {
                foreach (CustomerSealForm customerSeal in customerSeals)
                {
                    //這段之後會做成IMAGE64的處理並另存在指定的位置
                    string imagePath = customerSeal.ImageBase64;

                    CustomerSealJournal customerSealJournal = mapper.Map<CustomerSealJournal>(customerSeal);
                    customerSealJournal.ImagePath = imagePath;
                    BaseInputCustomerSealJournal(customerSealJournal, true, userId);
                    customerSealJournals.Add(customerSealJournal);
                }
                dbContext.CustomerSealJournals.AddRange(customerSealJournals);
                dbContext.BulkSaveChanges();
                response.Success();
            }
            else
            {
                response.CreateCustomerSealQuarterRepeat();
            }

            return response;
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
            int userId = 0; //從帳號驗證取得Id
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
                    deletecustomerSealQuery.UpdateUserId = userId;
                    deletecustomerSealQuery.UpdateDate = DateTime.Now;                           
                }
                else
                {
                    ResponseViewModel response = new();
                    response.DeleteCustomerNoData();
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
                    BaseInputCustomerSealJournal(UpdatecustomerSealQuery, false, userId);
                }
                else
                {
                    ResponseViewModel response = new();
                    response.UpdateCustomerSealNoData();                    
                    response.ErrorItem = "Update CustomerSealId:" + customerSealFormUpdate.Id;
                    responseViewModels.Add(response);
                }
            }
            //新增印鑑
            foreach (CustomerSealForm createCustomerSeal in customerSealUpdate.CreateCustomerSeals)
            {
                if (CheckRepeatSequence(mapper.Map<CustomerSealSequenceCheck>(createCustomerSeal), customerSealUpdate.DeleteCustomerSealIds)) //確認序號是否重複
                {
                    //這段之後會做成IMAGE64的處理並另存在指定的位置
                    string imagePath = createCustomerSeal.ImageBase64;

                    CustomerSealJournal customerSealJournal = mapper.Map<CustomerSealJournal>(createCustomerSeal);
                    customerSealJournal.ImagePath = imagePath;
                    BaseInputCustomerSealJournal(customerSealJournal, true, userId);
                    customerSealJournals.Add(customerSealJournal);
                }
                else
                {
                    ResponseViewModel response = new();
                    response.CreateCustomerSealSequenceRepeat();
                    response.ErrorItem = "Create CustomerId:" + createCustomerSeal.CustomerId
                                       + " SealMappingConfigId:" + createCustomerSeal.SealMappingConfigId
                                       + " Sequence:" + createCustomerSeal.Sequence;
                    responseViewModels.Add(response);
                }
            }            
            //無任何回傳訊息(錯誤訊息)就更新資料庫
            if (!responseViewModels.Any())
            {
                ResponseViewModel response = new();

                //將此季度的印鑑審查狀態全變更為草稿(更新時需要重審)
                IQueryable<CustomerSealJournal> customerSealJournalQuery = dbContext.CustomerSealJournals.Where
                                                (
                                                    customerSeal => customerSeal.CustomerId == customerSealUpdate.CustomerId
                                                    && customerSeal.Quarter == customerSealUpdate.Quarter
                                                    && customerSeal.DeleteStatus == DeleteStatus.NO
                                                );
                foreach (CustomerSealJournal customerSealJournal in customerSealJournalQuery)
                {
                    customerSealJournal.ReviewStatus = ReviewStatus.Draft;
                }

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
        /// 變更此季度印鑑待審
        /// </summary>
        /// <param name="customerSealQuarter">客戶ID與季度</param>        
        public ResponseViewModel PendingCustomerSeal(CustomerSealQuarterSearch customerSealQuarter)
        {
            ResponseViewModel response = ChangeDraftReviewStatusCustomerSeal(customerSealQuarter, ReviewStatus.Pending);
            return response;
        }
        /// <summary>
        /// 變更此季度印鑑作廢
        /// </summary>
        /// <param name="customerSealQuarter">客戶ID與季度</param>        
        public ResponseViewModel InvalidCustomerSeal(CustomerSealQuarterSearch customerSealQuarter)
        {
            ResponseViewModel response = ChangeDraftReviewStatusCustomerSeal(customerSealQuarter, ReviewStatus.Invalid);
            return response;
        }
        /// <summary>
        /// 客戶印鑑新增修改時基本的資料輸入
        /// </summary>
        /// <param name="customerSealJournal">Db上的印鑑資料</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">userId</param>
        private static void BaseInputCustomerSealJournal(CustomerSealJournal customerSealJournal, bool isCreate , int userId)
        {
            if (isCreate)
            {
                customerSealJournal.CreateUserId = userId;
                customerSealJournal.CreateDate = DateTime.Now;
                customerSealJournal.DeleteStatus = DeleteStatus.NO;                
            }
            else
            {
                customerSealJournal.UpdateUserId = userId;
                customerSealJournal.UpdateDate = DateTime.Now;
            }
            customerSealJournal.StartDate = AvailableDateUtil.NotActivated();
            customerSealJournal.EndDate = AvailableDateUtil.NotActivated(); //暫時加上                
            customerSealJournal.ReviewStatus = ReviewStatus.Draft;
        }

        /// <summary>
        /// 確認客戶序號是否重複 true 不重複 false 重複
        /// </summary>
        /// <param name="customerSealSequenceCheck">查詢參數</param>
        /// <param name="deleteCustomerSealIds">異動中刪除的印鑑</param>
        /// <returns></returns>
        private bool CheckRepeatSequence(CustomerSealSequenceCheck customerSealSequenceCheck, List<int> deleteCustomerSealIds)
        {
            CustomerSealJournal? customerSealQuery = dbContext.CustomerSealJournals.FirstOrDefault
                                                    (
                                                        customerSealJournal => customerSealJournal.CustomerId == customerSealSequenceCheck.CustomerId
                                                        && customerSealJournal.SealMappingConfigId == customerSealSequenceCheck.SealMappingConfigId
                                                        && customerSealJournal.Quarter == customerSealSequenceCheck.Quarter
                                                        && customerSealJournal.Sequence == customerSealSequenceCheck.Sequence
                                                        && customerSealJournal.DeleteStatus == DeleteStatus.NO
                                                        && !deleteCustomerSealIds.Contains(customerSealJournal.Id)
                                                    );

            return customerSealQuery == null;
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

        /// <summary>
        /// 客戶印鑑草稿狀態變更。
        /// </summary>
        /// <param name="customerSealQuarter">客戶ID與季度</param>
        /// <param name="reviewStatus">審查狀態</param>        
        private ResponseViewModel ChangeDraftReviewStatusCustomerSeal(CustomerSealQuarterSearch customerSealQuarter, ReviewStatus reviewStatus)
        {
            ResponseViewModel response = new();
            int userId = 0;//從帳號驗證取得
            IQueryable<CustomerSealJournal> customerSealJournalQuery = dbContext.CustomerSealJournals.Where
                                                            (
                                                                customerSeal => customerSeal.CustomerId == customerSealQuarter.CustomerId
                                                                && customerSeal.Quarter == customerSealQuarter.Quarter
                                                                && customerSeal.ReviewStatus == ReviewStatus.Draft
                                                            );
            
            if (customerSealJournalQuery.Any())
            {
                foreach(CustomerSealJournal customerSealJournal in customerSealJournalQuery)               
                {
                    customerSealJournal.ReviewStatus = reviewStatus;
                    customerSealJournal.UpdateDate = DateTime.Now;
                    customerSealJournal.UpdateUserId = userId;
                }
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.UpdateCustomerSealNoData();                                
            }

            return response;
        }
    }
}
