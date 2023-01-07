using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
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
        private readonly ImageSharpService imageSharpService;
        private readonly IMapper mapper;

        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        /// <param name="imageSharpService"></param>
        public CustomerSealService(SealTypographicDbContext dbContext, IMapper mapper, ImageSharpService imageSharpService)
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
        public CustomerSealQuarterViews GetCustomerSealQuarters(int customerId)
        {
            CustomerSealQuarterViews customerSealQuarters = new();
            List<CustomerSealQuarterView> customerSealQuarterQuery = dbContext.SealReviewJournals
                                                        .Include(x => x.CustomerSealJournal)
                                                        .Where
                                                        (
                                                            sealReviewJournal => sealReviewJournal.CustomerSealJournal.CustomerId == customerId                                                            
                                                            && sealReviewJournal.ReviewStatus <= ReviewStatus.Draft
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
            List<SealReviewJournal> sealReviewJournalQuery = dbContext.SealReviewJournals.Where
                                                                    (
                                                                        sealReviewJournal => 
                                                                        sealReviewJournal.CustomerSealJournal.CustomerId == customerSealQuarter.CustomerId                                                                        
                                                                        && sealReviewJournal.Quarter == customerSealQuarter.Quarter
                                                                        && sealReviewJournal.DeleteStatus == DeleteStatus.NO
                                                                        && sealReviewJournal.ReviewStatus <= ReviewStatus.Draft
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
                    //customerSealViewModel.ImageBase64 = sealReviewJournal.CustomerSealJournal.ImagePath;
                    customerSealViewModel.SealMappingConfigId = (int)sealReviewJournal.CustomerSealJournal.ConfigType;
                    sealViewModels.Add(customerSealViewModel);
                }
                customerSealViewModels.ReviewStatus = sealReviewJournalQuery.First().ReviewStatus;
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
            List<SealReviewJournal> sealReviewJournals = new();
            int userId = 0; //以後從帳號驗證取得Id

            SealReviewJournal? sealReviewJournalQuery = dbContext.SealReviewJournals.FirstOrDefault
                                                        (
                                                            x => x.CustomerSealJournal.CustomerId == customerSeals.First().CustomerId
                                                            && x.Quarter == customerSeals.First().Quarter
                                                        );

            if (sealReviewJournalQuery == null)
            {
                int count = 1;
                ImageBase64Info imageBase64Info = new()
                {
                    Code = GetCode(customerSeals.First().CustomerId),
                    CreateTime = DateTime.Now,
                    SealType = SealType.Customer
                };

                foreach (CustomerSealForm customerSeal in customerSeals)
                {
                    CustomerSealJournal customerSealJournal = new()
                    {
                        CustomerId = customerSeal.CustomerId,
                        ConfigType = (CustomerSealConfigType)customerSeal.SealMappingConfigId
                    };
                    SealReviewJournal sealReviewJournal = new()
                    {
                        Quarter = customerSeal.Quarter,
                        Sequence = customerSeal.Sequence
                    };

                    
                    //ImageBase64轉圖檔並存到指定資料夾
                    imageBase64Info.ImageBase64 = customerSeal.ImageBase64;
                    customerSealJournal.ImagePath = imageSharpService.SaveBase64ToFile(imageBase64Info, count);                    
                    //customerSealJournal.ImagePath = customerSeal.ImageBase64;
                    

                    BaseInputCustomerSealJournal(customerSealJournal, true, userId);
                    BaseInputSealReviewJournal(sealReviewJournal, true, userId);
                    
                    sealReviewJournal.CustomerSealJournal = customerSealJournal;
                    dbContext.SealReviewJournals.Add(sealReviewJournal);
                    //customerSealJournals.Add(customerSealJournal);
                    //sealReviewJournals.Add(sealReviewJournal);
                    count++;
                }

                //dbContext.CustomerSealJournals.AddRange(customerSealJournals);
                //dbContext.SealReviewJournals.AddRange(sealReviewJournals);
                //dbContext.BulkSaveChanges();
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
        public List<ResponseViewModel> UpdateCustomerSeals(CustomerSealUpdate customerSealUpdate)
        {            
            List<ResponseViewModel> responseViewModels = new();
            //List<CustomerSealJournal> updateCustomerSealJournals = new();
            //List<SealReviewJournal> updateCustomerSealJournals = new();
            //List<CustomerSealJournal> createCustomerSealJournals = new();
            //List<SealReviewJournal> createCustomerSealJournals = new();
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
                //CustomerSealJournal? deletecustomerSealQuery = dbContext.CustomerSealJournals.FirstOrDefault
                //                                                (
                //                                                    customerSeal => customerSeal.Id == customerSealId
                //                                                    && customerSeal.DeleteStatus == DeleteStatus.NO
                //                                                );
                SealReviewJournal? deleteSealQuery = dbContext.SealReviewJournals
                                                            .Include(sealReview => sealReview.CustomerSealJournal)
                                                            .FirstOrDefault
                                                            (
                                                                sealReview => sealReview.Id == sealReviewId
                                                                && sealReview.DeleteStatus == DeleteStatus.NO
                                                            );
                if (deleteSealQuery != null)
                {
                    deleteSealQuery.DeleteStatus = DeleteStatus.Yes;
                    deleteSealQuery.UpdateUserId = userId;
                    deleteSealQuery.UpdateDate = DateTime.Now;
                    deleteSealQuery.CustomerSealJournal.DeleteStatus = DeleteStatus.NO;
                    deleteSealQuery.CustomerSealJournal.UpdateUserId = userId;
                    deleteSealQuery.CustomerSealJournal.UpdateDate = DateTime.Now;
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
            foreach (CustomerSealFormUpdate customerSealFormUpdate in customerSealUpdate.UpdateCustomerSeals)
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
                    //CustomerSealJournal customerSealJournal = updatecustomerSealQuery;
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
                    imageBase64Info.CreateTime = DateTime.Now;                    
                    customerSealJournal.ImagePath = imageSharpService.SaveBase64ToFile(imageBase64Info, count);
                    //customerSealJournal.ImagePath = customerSealFormUpdate.ImageBase64;

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
            foreach (CustomerSealForm createCustomerSeal in customerSealUpdate.CreateCustomerSeals)
            {
                if (CheckRepeatSequence(mapper.Map<CustomerSealSequenceCheck>(createCustomerSeal), customerSealUpdate.DeleteCustomerSealIds)) //確認序號是否重複
                {
                    CustomerSealJournal customerSealJournal = new()
                    {
                        CustomerId = customerSealUpdate.CustomerId,
                        ConfigType = (CustomerSealConfigType)createCustomerSeal.SealMappingConfigId
                    };
                    SealReviewJournal sealReviewJournal = new()
                    {
                        Quarter = createCustomerSeal.Quarter,
                        Sequence = createCustomerSeal.Sequence
                    };

                    
                    //ImageBase64轉圖檔並存到指定資料夾
                    imageBase64Info.ImageBase64 = createCustomerSeal.ImageBase64;
                    customerSealJournal.ImagePath = imageSharpService.SaveBase64ToFile(imageBase64Info, count);                    
                    //customerSealJournal.ImagePath = createCustomerSeal.ImageBase64;


                    BaseInputCustomerSealJournal(customerSealJournal, true, userId);
                    BaseInputSealReviewJournal(sealReviewJournal, true, userId);

                    sealReviewJournal.CustomerSealJournal = customerSealJournal;
                    dbContext.SealReviewJournals.Add(sealReviewJournal);


                    //CustomerSealJournal customerSealJournal = mapper.Map<CustomerSealJournal>(createCustomerSeal);
                                            
                    //ImageBase64轉圖檔並存到指定資料夾                    
                    //imageBase64Info.ImageBase64 = createCustomerSeal.ImageBase64;
                    //imageBase64Info.CreateTime = DateTime.Now;
                    ////sealReviewJournal.ImagePath = imageSharpService.SaveBase64ToFile(imageBase64Info, count);

                    ////BaseInputCustomerSealJournal(sealReviewJournal, true, userId);
                    //createCustomerSealJournals.Add(sealReviewJournal);

                    count++;
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

                //dbContext.CustomerSealJournals.AddRange(updateCustomerSealJournals);
                //dbContext.CustomerSealJournals.AddRange(createCustomerSealJournals);
                //dbContext.BulkSaveChanges();
                dbContext.SaveChanges();
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
                //ReSequece(customerSealJournalQuery);
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
            ResponseViewModel response = ChangeDraftReviewStatus(customerSealQuarter, ReviewStatus.Pending);
            return response;
        }
        /// <summary>
        /// 變更此季度印鑑作廢
        /// </summary>
        /// <param name="customerSealQuarter">客戶ID與季度</param>        
        public ResponseViewModel InvalidCustomerSeal(CustomerSealQuarterSearch customerSealQuarter)
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
        /// 確認客戶序號是否重複 true 不重複 false 重複
        /// </summary>
        /// <param name="customerSealSequenceCheck">查詢參數</param>
        /// <param name="deleteCustomerSealIds">異動中刪除的印鑑</param>
        /// <returns></returns>
        private bool CheckRepeatSequence(CustomerSealSequenceCheck customerSealSequenceCheck, List<int> deleteCustomerSealIds)
        {
            //CustomerSealJournal? customerSealQuery = dbContext.CustomerSealJournals.FirstOrDefault
            //                                        (
            //                                            customerSealJournal => customerSealJournal.CustomerId == customerSealSequenceCheck.CustomerId
            //                                            && customerSealJournal.ConfigType == (CustomerSealConfigType)customerSealSequenceCheck.SealMappingConfigId
            //                                            && customerSealJournal.Quarter == customerSealSequenceCheck.Quarter
            //                                            && customerSealJournal.Sequence == customerSealSequenceCheck.Sequence
            //                                            && customerSealJournal.DeleteStatus == DeleteStatus.NO
            //                                            && !deleteCustomerSealIds.Contains(customerSealJournal.Id)
            //                                        );

            SealReviewJournal? sealReviewQuery = dbContext.SealReviewJournals.FirstOrDefault
                                        (
                                            sealReviewJournal => sealReviewJournal.CustomerSealJournal.CustomerId == customerSealSequenceCheck.CustomerId
                                            && sealReviewJournal.CustomerSealJournal.ConfigType == (CustomerSealConfigType)customerSealSequenceCheck.SealMappingConfigId
                                            && sealReviewJournal.Quarter == customerSealSequenceCheck.Quarter
                                            && sealReviewJournal.Sequence == customerSealSequenceCheck.Sequence
                                            && sealReviewJournal.DeleteStatus == DeleteStatus.NO
                                            && !deleteCustomerSealIds.Contains(sealReviewJournal.Id)
                                        );

            return sealReviewQuery == null;
        }

        /// <summary>
        /// 刪除印鑑時重新排序
        /// </summary>
        /// <param name="deleteCustomerSeal"></param>
        //private void ReSequece(CustomerSealJournal deleteCustomerSeal)
        //{
        //    int sequence = 0;
        //    IQueryable<CustomerSealJournal> customerSealJournalQuery = dbContext.CustomerSealJournals.Where
        //                                                                (
        //                                                                    customerSeal => customerSeal.Quarter == deleteCustomerSeal.Quarter
        //                                                                    && customerSeal.ConfigType == deleteCustomerSeal.ConfigType
        //                                                                    && customerSeal.CustomerId == deleteCustomerSeal.CustomerId
        //                                                                    && customerSeal.DeleteStatus == DeleteStatus.NO
        //                                                                )
        //                                                                .OrderBy(customerSeal => customerSeal.Sequence);

        //    if (customerSealJournalQuery.Any())
        //    {
        //        if (deleteCustomerSeal.Sequence == 1)
        //        {
        //            sequence = 1;
        //            foreach (CustomerSealJournal customerSeal in customerSealJournalQuery)
        //            {
        //                customerSeal.Sequence = sequence;
        //                customerSeal.UpdateDate = DateTime.Now;
        //                sequence++;
        //            }
        //            dbContext.SaveChanges();
        //        }
        //        else
        //        {
        //            if (deleteCustomerSeal.Sequence < customerSealJournalQuery.Last().Sequence)//如果刪除的序號是最後一個就不用做任何處理
        //            {
        //                IQueryable<CustomerSealJournal> moreThanTheSequenceDeleteCustomerSeals = customerSealJournalQuery
        //                                                            .Where(customerSeal => customerSeal.Sequence > deleteCustomerSeal.Sequence);
        //                if (moreThanTheSequenceDeleteCustomerSeals.Any())
        //                {
        //                    sequence = deleteCustomerSeal.Sequence;
        //                    foreach (CustomerSealJournal customerSeal in moreThanTheSequenceDeleteCustomerSeals)
        //                    {
        //                        customerSeal.Sequence = sequence;
        //                        customerSeal.UpdateDate = DateTime.Now;
        //                        sequence++;
        //                    }
        //                    dbContext.SaveChanges();
        //                }
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 客戶印鑑草稿狀態變更。
        /// </summary>
        /// <param name="customerSealQuarter">客戶ID與季度</param>
        /// <param name="reviewStatus">審查狀態</param>        
        private ResponseViewModel ChangeDraftReviewStatus(CustomerSealQuarterSearch customerSealQuarter, ReviewStatus reviewStatus)
        {
            ResponseViewModel response = new();
            int userId = 0;//從帳號驗證取得
                           //IQueryable<CustomerSealJournal> customerSealJournalQuery = dbContext.CustomerSealJournals.Where
                           //                                                (
                           //                                                    customerSeal => customerSeal.CustomerId == customerSealQuarter.CustomerId
                           //                                                    //&& customerSeal.Quarter == customerSealQuarter.Quarter
                           //                                                    //&& customerSeal.ReviewStatus == ReviewStatus.Draft
                           //                                                );

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
