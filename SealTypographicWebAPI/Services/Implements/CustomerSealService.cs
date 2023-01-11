using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
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
    /// 客戶印鑑管理
    /// </summary>
    public class CustomerSealService : ICustomerSealService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ImageService imageSharpService;
        private readonly IMapper mapper;

        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        /// <param name="imageSharpService"></param>
        public CustomerSealService(SealTypographicDbContext dbContext, IMapper mapper, ImageService imageSharpService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.imageSharpService = imageSharpService;
        }

        /// <summary>
        /// 取得顧客印鑑季度表
        /// </summary>
        /// <param name="customerId">客戶ID</param>        
        /// <returns></returns>
        public CustomerSealQuarterViews GetQuarter(int customerId)
        {
            CustomerSealQuarterViews customerSealQuarters = new();
            customerSealQuarters.Quarters = dbContext.SealReviewJournals
                                            .Include(x => x.CustomerSealJournal)
                                            .Where
                                            (
                                                sealReviewJournal => sealReviewJournal.CustomerSealJournal.CustomerId == customerId                                                            
                                                && sealReviewJournal.ReviewStatus <= ReviewStatus.Pending
                                                && sealReviewJournal.DeleteStatus == DeleteStatus.NO
                                            )
                                            .Select(sealReviewJournal => new CustomerSealQuarterView()
                                            {
                                                CustomerId = customerId,
                                                Quarter = sealReviewJournal.Quarter,
                                                ReviewStatus = sealReviewJournal.ReviewStatus
                                            })
                                            .GroupBy(customerSealQuarterView => customerSealQuarterView.Quarter)
                                            .OrderByDescending(g => g.Key)
                                            .Select(customerSealQuarter => customerSealQuarter.First())
                                            .ToList();

            if (customerSealQuarters.Quarters.Any())
            {                
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
        public CustomerSealViewModels GetSeal(CustomerSealQuarter customerSealQuarter)
        {
            CustomerSealViewModels customerSealViewModels = new()
            {                
                CustomerId = customerSealQuarter.CustomerId,
                Quarter = customerSealQuarter.Quarter
            };
            List<SealReviewJournal> sealReviewJournalQuery = dbContext.SealReviewJournals.Where
                                                                    (
                                                                        sealReviewJournal => 
                                                                        sealReviewJournal.CustomerSealJournal.CustomerId == customerSealQuarter.CustomerId                                                                        
                                                                        && sealReviewJournal.Quarter == customerSealQuarter.Quarter
                                                                        && sealReviewJournal.DeleteStatus == DeleteStatus.NO
                                                                        && sealReviewJournal.ReviewStatus <= ReviewStatus.Pending
                                                                    )               
                                                                    .Include(sealReviewJournal => sealReviewJournal.CustomerSealJournal)
                                                                    .OrderBy(sealReviewJournal => sealReviewJournal.CustomerSealJournal.ConfigType)
                                                                    .ThenBy(sealReviewJournal => sealReviewJournal.Sequence)
                                                                    .ToList();
            if (sealReviewJournalQuery.Any())
            {
                foreach (SealReviewJournal sealReviewJournal in sealReviewJournalQuery)
                {
                    CustomerSealViewModel customerSealViewModel = mapper.Map<CustomerSealViewModel>(sealReviewJournal);
                    customerSealViewModel.ImageBase64 = imageSharpService.GetPathToBase64(sealReviewJournal.CustomerSealJournal.ImagePath, SealType.Customer); //資料庫取得圖檔路徑轉BASE64                                       
                    customerSealViewModel.SealMappingConfigId = sealReviewJournal.CustomerSealJournal.ConfigType;
                    customerSealViewModels.SealViewModels.Add(customerSealViewModel);
                }
                customerSealViewModels.ReviewStatus = sealReviewJournalQuery.First().ReviewStatus;
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
        /// <param name="customerSealForms">印鑑組</param>
        /// <returns></returns>
        public ResponseViewModel Create(CustomerSealForm customerSealForms)
        {
            ResponseViewModel response = new();
            List<CustomerSealJournal> customerSealJournals = new();
            List<SealReviewJournal> sealReviewJournals = new();
            int userId = 0; //以後從帳號驗證取得Id

            SealReviewJournal? sealReviewJournalQuery = dbContext.SealReviewJournals.FirstOrDefault
                                                        (
                                                            x => x.CustomerSealJournal.CustomerId == customerSealForms.CustomerId
                                                            && x.Quarter == customerSealForms.Quarter
                                                        );

            if (sealReviewJournalQuery == null)
            {
                int count = 1;
                ImageBase64Info imageBase64Info = new()
                {
                    Code = GetCode(customerSealForms.CustomerId),                    
                    SealType = SealType.Customer
                };

                foreach (CustomerSeal customerSeal in customerSealForms.Seals)
                {
                    CustomerSealJournal customerSealJournal = new()
                    {
                        CustomerId = customerSealForms.CustomerId,
                        ConfigType = customerSeal.SealMappingConfigId
                    };
                    SealReviewJournal sealReviewJournal = new()
                    {
                        Quarter = customerSealForms.Quarter,
                        Sequence = customerSeal.Sequence
                    };

                    
                    //ImageBase64轉圖檔並存到指定資料夾
                    imageBase64Info.ImageBase64 = customerSeal.ImageBase64;
                    customerSealJournal.ImagePath = imageSharpService.SaveBase64ToFile(imageBase64Info, count);                   

                    BaseInputCustomerSealJournal(customerSealJournal, true, userId);
                    BaseInputSealReviewJournal(sealReviewJournal, true, userId);
                    
                    sealReviewJournal.CustomerSealJournal = customerSealJournal;
                    dbContext.SealReviewJournals.Add(sealReviewJournal);
                    count++;
                }

                dbContext.SaveChanges();
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
        public List<ResponseViewModel> Update(CustomerSealUpdate customerSealUpdate)
        {            
            List<ResponseViewModel> responseViewModels = new();
            int userId = 0; //從帳號驗證取得Id
            int count = 1;            

            ImageBase64Info imageBase64Info = new()
            {
                Code = GetCode(customerSealUpdate.CustomerId),                
                SealType = SealType.Customer
            };

            //刪除印鑑
            foreach (int sealReviewId in customerSealUpdate.DeleteCustomerSealIds)
            {
                SealReviewJournal? deleteSealQuery = dbContext.SealReviewJournals
                                                            .Include(sealReview => sealReview.CustomerSealJournal)
                                                            .FirstOrDefault
                                                            (
                                                                sealReview => sealReview.Id == sealReviewId
                                                                && sealReview.DeleteStatus == DeleteStatus.NO
                                                            );
                if (deleteSealQuery != null)
                {
                    //原印鑑刪除(Hide)
                    deleteSealQuery.DeleteStatus = DeleteStatus.Yes;
                    deleteSealQuery.CustomerSealJournal.DeleteStatus = DeleteStatus.Yes;
                    BaseInputCustomerSealJournal(deleteSealQuery.CustomerSealJournal, false, userId);
                    BaseInputSealReviewJournal(deleteSealQuery, false, userId);
                }
                else
                {
                    ResponseViewModel response = new();
                    response.DeleteCustomerNoData();
                    response.ErrorItem = "Delete CustomerSealId:" + sealReviewId;
                    responseViewModels.Add(response);
                }
            }
            //修改印鑑
            foreach (CustomerSealUpdateForm customerSealFormUpdate in customerSealUpdate.UpdateCustomerSeals)
            {
                SealReviewJournal? updateSealQuery = dbContext.SealReviewJournals
                                            .Include(sealReview => sealReview.CustomerSealJournal)
                                            .FirstOrDefault
                                            (
                                                sealReview => sealReview.Id == customerSealFormUpdate.Id
                                                && sealReview.DeleteStatus == DeleteStatus.NO
                                            );

                if (updateSealQuery != null)
                {
                    //新增印鑑                    
                    SealReviewJournal sealReviewJournal = new()
                    {
                        Quarter = customerSealUpdate.Quarter,
                        Sequence = customerSealFormUpdate.Sequence
                    };
                    CustomerSealJournal customerSealJournal = new()
                    {
                        CustomerId = customerSealUpdate.CustomerId,
                        ConfigType = updateSealQuery.CustomerSealJournal.ConfigType
                    };
                    //ImageBase64轉圖檔並存到指定資料夾                    
                    imageBase64Info.ImageBase64 = customerSealFormUpdate.ImageBase64;
                    customerSealJournal.ImagePath = imageSharpService.SaveBase64ToFile(imageBase64Info, count);                    

                    BaseInputCustomerSealJournal(customerSealJournal, true, userId);
                    BaseInputSealReviewJournal(sealReviewJournal, true, userId);

                    sealReviewJournal.CustomerSealJournal = customerSealJournal;                                                            
                    dbContext.SealReviewJournals.Add(sealReviewJournal);

                    //原印鑑刪除(Hide)
                    updateSealQuery.DeleteStatus = DeleteStatus.Yes;
                    updateSealQuery.CustomerSealJournal.DeleteStatus = DeleteStatus.Yes;
                    BaseInputCustomerSealJournal(updateSealQuery.CustomerSealJournal, false, userId);
                    BaseInputSealReviewJournal(updateSealQuery, false, userId);                    

                    count++;
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
            count = 1;
            foreach (CustomerSeal createCustomerSeal in customerSealUpdate.CreateCustomerSeals)
            {
                CustomerSealSequenceCheck customerSealSequenceCheck = new()
                {
                    CustomerId = customerSealUpdate.CustomerId,
                    Quarter = customerSealUpdate.Quarter,
                    SealMappingConfigId = createCustomerSeal.SealMappingConfigId,
                    Sequence = createCustomerSeal.Sequence,                    
                };

                List<int> updateCustomerSealIds = customerSealUpdate.UpdateCustomerSeals.Select(x =>  x.Id).ToList();                

                if (!CheckRepeatSequence(customerSealSequenceCheck, customerSealUpdate.DeleteCustomerSealIds, updateCustomerSealIds)) //確認序號是否重複
                {
                    CustomerSealJournal customerSealJournal = new()
                    {
                        CustomerId = customerSealUpdate.CustomerId,
                        ConfigType = createCustomerSeal.SealMappingConfigId
                    };
                    SealReviewJournal sealReviewJournal = new()
                    {
                        Quarter = customerSealUpdate.Quarter,
                        Sequence = createCustomerSeal.Sequence
                    };

                    
                    //ImageBase64轉圖檔並存到指定資料夾
                    imageBase64Info.ImageBase64 = createCustomerSeal.ImageBase64;
                    customerSealJournal.ImagePath = imageSharpService.SaveBase64ToFile(imageBase64Info, count);                    

                    BaseInputCustomerSealJournal(customerSealJournal, true, userId);
                    BaseInputSealReviewJournal(sealReviewJournal, true, userId);

                    sealReviewJournal.CustomerSealJournal = customerSealJournal;
                    dbContext.SealReviewJournals.Add(sealReviewJournal);


                    count++;
                }
                else
                {
                    ResponseViewModel response = new();
                    response.CreateCustomerSealSequenceRepeat();
                    response.ErrorItem = "Create CustomerId:" + customerSealUpdate.CustomerId
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
                IQueryable<SealReviewJournal> sealReviewJournalQuery = dbContext.SealReviewJournals.Where
                                                (
                                                    customerSeal => customerSeal.CustomerSealJournal.CustomerId == customerSealUpdate.CustomerId
                                                    && customerSeal.Quarter == customerSealUpdate.Quarter
                                                    && customerSeal.DeleteStatus == DeleteStatus.NO
                                                );
                foreach (SealReviewJournal sealReviewJournal in sealReviewJournalQuery)
                {
                    sealReviewJournal.ReviewStatus = ReviewStatus.Draft;
                }

                dbContext.SaveChanges();
                response.Success();
                responseViewModels.Add(response);                
            }            

            return responseViewModels;
        }
        
        /// <summary>
        /// 變更此季度印鑑待審
        /// </summary>
        /// <param name="customerSealQuarter">客戶ID與季度</param>        
        public ResponseViewModel PendingCustomerSeal(CustomerSealQuarter customerSealQuarter)
        {
            ResponseViewModel response = ChangeDraftReviewStatus(customerSealQuarter, ReviewStatus.Pending);
            return response;
        }
        /// <summary>
        /// 變更此季度印鑑作廢
        /// </summary>
        /// <param name="customerSealQuarter">客戶ID與季度</param>        
        public ResponseViewModel InvalidCustomerSeal(CustomerSealQuarter customerSealQuarter)
        {
            ResponseViewModel response = ChangeDraftReviewStatus(customerSealQuarter, ReviewStatus.Invalid);
            return response;
        }        

        /// <summary>
        /// 客戶印鑑新增修改時基本的資料輸入
        /// </summary>
        /// <param name="sealReviewJournal">Db上的印鑑資料</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">userId</param>
        private static void BaseInputSealReviewJournal(SealReviewJournal sealReviewJournal, bool isCreate, int userId)
        {
            if (isCreate)
            {
                sealReviewJournal.CreateUserId = userId;
                sealReviewJournal.CreateDate = DateTime.Now;
                sealReviewJournal.DeleteStatus = DeleteStatus.NO;
            }
            else
            {
                sealReviewJournal.UpdateUserId = userId;
                sealReviewJournal.UpdateDate = DateTime.Now;
            }
            sealReviewJournal.StartDate = AvailableDateUtil.NotActivated();
            sealReviewJournal.EndDate = AvailableDateUtil.NotActivated(); //暫時加上                
            sealReviewJournal.ReviewStatus = ReviewStatus.Draft;
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
        }

        /// <summary>
        /// 確認客戶序號是否重複 true 重複 false 不重複
        /// </summary>
        /// <param name="customerSealSequenceCheck">查詢參數</param>
        /// <param name="deleteCustomerSealIds">異動中刪除的印鑑Id</param>
        /// <param name="updateCustomerSealIds">異動中更新的印鑑Id(也會被標上刪除)</param>
        /// <returns></returns>
        private bool CheckRepeatSequence(CustomerSealSequenceCheck customerSealSequenceCheck, List<int> deleteCustomerSealIds, List<int> updateCustomerSealIds)
        {
            SealReviewJournal? sealReviewQuery = dbContext.SealReviewJournals.FirstOrDefault
                                        (
                                            sealReviewJournal => sealReviewJournal.CustomerSealJournal.CustomerId == customerSealSequenceCheck.CustomerId
                                            && sealReviewJournal.CustomerSealJournal.ConfigType == customerSealSequenceCheck.SealMappingConfigId
                                            && sealReviewJournal.Quarter == customerSealSequenceCheck.Quarter
                                            && sealReviewJournal.Sequence == customerSealSequenceCheck.Sequence
                                            && sealReviewJournal.DeleteStatus == DeleteStatus.NO
                                            && !deleteCustomerSealIds.Contains(sealReviewJournal.Id)
                                            && !updateCustomerSealIds.Contains(sealReviewJournal.Id)                                            
                                        );

            return sealReviewQuery != null;
        }
     
        /// <summary>
        /// 客戶印鑑草稿狀態變更。
        /// </summary>
        /// <param name="customerSealQuarter">客戶ID與季度</param>
        /// <param name="reviewStatus">審查狀態</param>        
        private ResponseViewModel ChangeDraftReviewStatus(CustomerSealQuarter customerSealQuarter, ReviewStatus reviewStatus)
        {
            ResponseViewModel response = new();
            int userId = 0;//從帳號驗證取得

            IQueryable<SealReviewJournal> customerSealJournalQuery = dbContext.SealReviewJournals                                                                    
                                                                    .Where
                                                                    (
                                                                        sealReviewJournal => sealReviewJournal.CustomerSealJournal.CustomerId == customerSealQuarter.CustomerId
                                                                        && sealReviewJournal.Quarter == customerSealQuarter.Quarter
                                                                        && sealReviewJournal.ReviewStatus == ReviewStatus.Draft
                                                                    );

            if (customerSealJournalQuery.Any())
            {
                foreach (SealReviewJournal sealReview in customerSealJournalQuery)
                {
                    sealReview.ReviewStatus = reviewStatus;
                    sealReview.UpdateDate = DateTime.Now;
                    sealReview.UpdateUserId = userId;
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

        private string GetCode(int customerId)
        {
            string code;
            Customer? customer = dbContext.Customers.Find(customerId);
            if (customer != null)
            {
                code = customer.Code;
            }
            else
            {
                code = string.Empty;
            }
            return code;
        }
    }
}
