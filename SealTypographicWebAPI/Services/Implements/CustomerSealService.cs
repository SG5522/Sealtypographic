using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Utils;

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
        /// 建構
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
        /// 取得客戶印鑑季度表
        /// </summary>
        /// <param name="customerId">客戶ID</param>        
        /// <returns></returns>
        public CustomerSealQuarterResponse GetQuarter(int customerId)
        {
            CustomerSealQuarterResponse customerSealQuarters = new()
            {
                CustomerSealQuarters = dbContext.CustomerSealQuarterJournals.Where
                            (
                                customerSealQuarterJournal => customerSealQuarterJournal.Customer.Id == customerId
                                && customerSealQuarterJournal.ReviewStatus <= ReviewStatus.Disabled
                                && customerSealQuarterJournal.DeleteStatus == DeleteStatus.No
                            )
                            .Select(customerSealQuarterJournal => new CustomerSealQuarterViewModel()
                            {
                                Id = customerSealQuarterJournal.Id,
                                Quarter = customerSealQuarterJournal.Quarter,
                                ReviewStatus = customerSealQuarterJournal.ReviewStatus
                            })
                            .OrderByDescending(customerSealQuarterJournal => customerSealQuarterJournal.Quarter)
                            .ToList()
            };

            if (customerSealQuarters.CustomerSealQuarters.Any())
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
        /// <param name="customerSealQuarterId">客戶印鑑季度Id</param>
        /// <returns></returns>
        public CustomerSealViewModels GetSeals(int customerSealQuarterId)
        {
            CustomerSealViewModels customerSealViewModels = new();
            
            CustomerSealQuarterJournal? customerSealQuarterJournalQuery = dbContext.CustomerSealQuarterJournals
                                                                        .Include(customerSealQuarterJournal => customerSealQuarterJournal.CustomerSealJournals)
                                                                        .FirstOrDefault
                                                                        (
                                                                            customerSealQuarterJournal => customerSealQuarterJournal.Id == customerSealQuarterId                                                                        
                                                                        );  
                                                                                                                                                                                                        
            if (customerSealQuarterJournalQuery != null)
            {
                List<CustomerSealJournal> customerSeals = customerSealQuarterJournalQuery.CustomerSealJournals
                                                            .Where(x => x.DeleteStatus == DeleteStatus.No)
                                                            .OrderBy(x => x.ConfigType)
                                                            .ThenBy(x => x.Sequence)
                                                            .ToList();

                foreach (CustomerSealJournal customerSealJournal in customerSeals)
                {
                    CustomerSealViewModel customerSealViewModel = mapper.Map<CustomerSealViewModel>(customerSealJournal);
                    customerSealViewModel.ImageBase64 = imageSharpService.GetPathToBase64(customerSealJournal.ImageFullPath); //資料庫取得圖檔路徑轉BASE64                                       
                    customerSealViewModel.SealMappingConfigId = customerSealJournal.ConfigType;
                    customerSealViewModels.SealViewModels.Add(customerSealViewModel);
                }
                customerSealViewModels.CustomerSealQuarterId = customerSealQuarterId;
                customerSealViewModels.ReviewStatus = customerSealQuarterJournalQuery.ReviewStatus;
                customerSealViewModels.Success();
            }
            else
            {
                customerSealViewModels.CustomerSealNoData();
            }            
            return customerSealViewModels;
        }

        /// <summary>
        /// 新增客戶印鑑組資料
        /// </summary>
        /// <param name="customerSealForms">客戶印鑑組資料</param>
        /// <returns></returns>
        public ResponseViewModel New(CustomerSealForm customerSealForms)
        {
            ResponseViewModel response = new();            
            List<CustomerSealJournal> customerSealJournals = new();
            int userId = 0; //以後從帳號驗證取得Id


            Customer? customerQuery = dbContext.Customers.Include(customer => customer.CustomerSealQuarterJournals)
                                       .FirstOrDefault(x => x.Id == customerSealForms.CustomerId);            

            if (customerQuery != null)
            {
                CustomerSealQuarterJournal? sealQuarterJournalQuery = customerQuery.CustomerSealQuarterJournals
                                                                    .FirstOrDefault(customerSealQuarterJournal => customerSealQuarterJournal.Quarter == customerSealForms.Quarter);

                if (sealQuarterJournalQuery == null)
                {                    
                    CustomerSealQuarterJournal customerSealQuarterJournal = new();
                    ImageBase64Info imageBase64Info = new()
                    {
                        Code = GetCode(customerSealForms.CustomerId),
                        SealType = SealType.Customer
                    };

                    customerSealQuarterJournal.Quarter = customerSealForms.Quarter;                    
                    BaseInputQuarterJournal(customerSealQuarterJournal, true, userId);
                    
                    foreach (CustomerSeal customerSeal in customerSealForms.Seals)
                    {
                        CustomerSealJournal customerSealJournal = new()
                        {
                            ConfigType = customerSeal.SealMappingConfigId,
                            Sequence = customerSeal.Sequence
                        };
                        //ImageBase64轉圖檔並存到指定資料夾
                        imageBase64Info.ImageBase64 = customerSeal.ImageBase64;
                        customerSealJournal.ImageFullPath = imageSharpService.GetImageBase64FullPath(imageBase64Info, false);
                        customerSealJournal.ThumbnailFullPath = imageSharpService.GetImageBase64FullPath(imageBase64Info, true);

                        BaseInputCustomerSealJournal(customerSealJournal, true, userId);
                        customerSealJournals.Add(customerSealJournal);                        
                    }
                    
                    customerSealQuarterJournal.CustomerSealJournals = customerSealJournals;
                    customerQuery.CustomerSealQuarterJournals.Add(customerSealQuarterJournal);
                    dbContext.SaveChanges();
                    response.Success();
                }                
                else
                {
                    response.CreateCustomerSealQuarterRepeat();
                }
            }
            else
            {
                response.CustomeNoData();
            }

            return response;
        }

        /// <summary>
        /// 異動客戶印鑑
        /// </summary>
        /// <param name="customerSealUpdate">需要異動客戶印鑑資料</param>
        /// <returns></returns>
        public List<ResponseViewModel> Update(CustomerSealUpdate customerSealUpdate)
        {            
            List<ResponseViewModel> responseViewModels = new();
            
            int userId = 0; //從帳號驗證取得Id          

            List<int> updateCustomerSealIds = customerSealUpdate.UpdateCustomerSeals.Select(x => x.Id).ToList();

            CustomerSealQuarterJournal? customerSealQuarterQuery = dbContext.CustomerSealQuarterJournals
                                                                    .Include(customerSealQuarterJournal => customerSealQuarterJournal.Customer)
                                                                    .Include(customerSealQuarterJournal => customerSealQuarterJournal.CustomerSealJournals)
                                                                    .FirstOrDefault
                                                                    (
                                                                        customerSealQuarterJournal => customerSealQuarterJournal.Id == customerSealUpdate.CustomerSealQuarterId                                                                                                                                                
                                                                    );

            if (customerSealQuarterQuery != null)
            {
                ImageBase64Info imageBase64Info = new()
                {
                    Code = GetCode(customerSealQuarterQuery.Customer.Id),
                    SealType = SealType.Customer
                };

                //刪除印鑑
                foreach (int deleteSealId in customerSealUpdate.DeleteCustomerSealIds)
                {
                    //CustomerSealJournal? deleteSealQuery = dbContext.CustomerSealJournals.FirstOrDefault
                    //                                        (
                    //                                            customerSealJournal => customerSealJournal.Id == deleteSealId
                    //                                            && customerSealJournal.DeleteStatus == DeleteStatus.No
                    //                                        );

                    CustomerSealJournal? deleteSealQuery = customerSealQuarterQuery.CustomerSealJournals.FirstOrDefault(x => x.Id == deleteSealId);

                    if (deleteSealQuery != null)
                    {
                        //原印鑑刪除(Hide)
                        deleteSealQuery.DeleteStatus = DeleteStatus.Yes;
                        BaseInputCustomerSealJournal(deleteSealQuery, false, userId);
                    }
                    else
                    {
                        ResponseViewModel response = new();
                        response.DeleteCustomerNoData();
                        response.ErrorItem = $"Delete CustomerSealId:{deleteSealId}";
                        responseViewModels.Add(response);
                    }
                }

                //修改印鑑
                foreach (CustomerSealUpdateForm customerSealFormUpdate in customerSealUpdate.UpdateCustomerSeals)
                {
                    //CustomerSealJournal? updateSealQuery = dbContext.CustomerSealJournals                                                
                    //                            .FirstOrDefault
                    //                            (
                    //                                customerSealJournal => customerSealJournal.Id == customerSealFormUpdate.Id
                    //                                && customerSealJournal.DeleteStatus == DeleteStatus.No
                    //                            );

                    CustomerSealJournal? updateSealQuery = customerSealQuarterQuery.CustomerSealJournals.FirstOrDefault(x => x.Id == customerSealFormUpdate.Id);


                    if (updateSealQuery != null)
                    {
                        //新增印鑑
                        CustomerSealJournal customerSealJournal = new()
                        {
                            Sequence = updateSealQuery.Sequence,
                            ConfigType = updateSealQuery.ConfigType
                        };                        
                        //customerSealJournal.CustomerSealQuarterJournal.Id = customerSealQuarterQuery.Id;

                        //ImageBase64轉圖檔並存到指定資料夾                    
                        imageBase64Info.ImageBase64 = customerSealFormUpdate.ImageBase64;
                        customerSealJournal.ImageFullPath = imageSharpService.GetImageBase64FullPath(imageBase64Info, false);
                        customerSealJournal.ThumbnailFullPath = imageSharpService.GetImageBase64FullPath(imageBase64Info, true);
                        BaseInputCustomerSealJournal(customerSealJournal, true, userId);

                        customerSealQuarterQuery.CustomerSealJournals.Add(customerSealJournal);                        

                        //原印鑑刪除(Hide)
                        updateSealQuery.DeleteStatus = DeleteStatus.Yes;
                        BaseInputCustomerSealJournal(updateSealQuery, false, userId);                        
                    }
                    else
                    {
                        ResponseViewModel response = new();
                        response.UpdateCustomerSealNoData();
                        response.ErrorItem = $"Update CustomerSealId:{customerSealFormUpdate.Id}";
                        responseViewModels.Add(response);
                    }
                }

                //新增印鑑                
                foreach (CustomerSeal createCustomerSeal in customerSealUpdate.CreateCustomerSeals)
                {
                    CustomerSealSequenceCheck customerSealSequenceCheck = new()
                    {                        
                        CustomerSealQuarterJournalId = customerSealQuarterQuery.Id,
                        SealMappingConfigId = createCustomerSeal.SealMappingConfigId,
                        Sequence = createCustomerSeal.Sequence,
                    };
                    

                    if (!CheckRepeatSequence(customerSealSequenceCheck, customerSealUpdate.DeleteCustomerSealIds, updateCustomerSealIds)) //確認印鑑序號是否重複
                    {
                        CustomerSealJournal customerSealJournal = new()
                        {
                            Sequence = createCustomerSeal.Sequence,
                            ConfigType = createCustomerSeal.SealMappingConfigId
                        };

                        //ImageBase64轉圖檔並存到指定資料夾
                        imageBase64Info.ImageBase64 = createCustomerSeal.ImageBase64;
                        customerSealJournal.ImageFullPath = imageSharpService.GetImageBase64FullPath(imageBase64Info, false);
                        customerSealJournal.ThumbnailFullPath = imageSharpService.GetImageBase64FullPath(imageBase64Info, true);
                        BaseInputCustomerSealJournal(customerSealJournal, true, userId);
                        customerSealQuarterQuery.CustomerSealJournals.Add(customerSealJournal);                                                
                    }
                    else
                    {
                        ResponseViewModel response = new();
                        response.CreateCustomerSealSequenceRepeat();
                        response.ErrorItem = $"New SealMappingConfigId:{createCustomerSeal.SealMappingConfigId}" +
                                             $"Sequence:{ createCustomerSeal.Sequence}";
                        responseViewModels.Add(response);
                    }
                }
                //無任何回傳訊息(錯誤訊息)就更新資料庫
                if (!responseViewModels.Any())
                {
                    ResponseViewModel response = new();                    
                    customerSealQuarterQuery.ReviewStatus = ReviewStatus.Draft;

                    dbContext.SaveChanges();
                    response.Success();
                    responseViewModels.Add(response);
                }
            }
            
            return responseViewModels;
        }

        ///<inheritdoc />   
        public ResponseViewModel Pending(int customerSealQuarterId)
        {
            ResponseViewModel response = ChangeReviewStatus(customerSealQuarterId, ReviewStatus.Pending);
            return response;
        }

        /// <inheritdoc />
        public ResponseViewModel Invalid(int customerSealQuarterId)
        {
            ResponseViewModel response = ChangeReviewStatus(customerSealQuarterId, ReviewStatus.Invalid);
            return response;
        }

        ///<inheritdoc />   
        public ResponseViewModel CancelReview(int customerSealQuarterId)
        {
            ResponseViewModel response = ChangeReviewStatus(customerSealQuarterId, ReviewStatus.Draft);
            return response;
        }

        /// <summary>
        /// 客戶印鑑新增修改時基本的資料輸入
        /// </summary>
        /// <param name="customerSealQuarterJournal">Db上的印鑑資料</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">userId</param>
        private static void BaseInputQuarterJournal(CustomerSealQuarterJournal customerSealQuarterJournal, bool isCreate, int userId)
        {
            if (isCreate)
            {
                customerSealQuarterJournal.CreateUserId = userId;
                customerSealQuarterJournal.CreateDate = DateTime.Now;
                customerSealQuarterJournal.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                customerSealQuarterJournal.UpdateUserId = userId;
                customerSealQuarterJournal.UpdateDate = DateTime.Now;
            }
            customerSealQuarterJournal.StartDate = AvailableDateUtil.NotActivated();
            customerSealQuarterJournal.EndDate = AvailableDateUtil.NotActivated();                
            customerSealQuarterJournal.ReviewStatus = ReviewStatus.Draft;
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
                customerSealJournal.DeleteStatus = DeleteStatus.No;                
            }
            else
            {
                customerSealJournal.UpdateUserId = userId;
                customerSealJournal.UpdateDate = DateTime.Now;
            }
        }

        /// <summary>
        /// 確認印鑑序號是否重複 true 重複 false 不重複
        /// </summary>
        /// <param name="customerSealSequenceCheck">查詢參數</param>
        /// <param name="deleteCustomerSealIds">異動中刪除的印鑑Id</param>
        /// <param name="updateCustomerSealIds">異動中更新的印鑑Id</param>
        /// <returns></returns>
        private bool CheckRepeatSequence(CustomerSealSequenceCheck customerSealSequenceCheck, List<int> deleteCustomerSealIds, List<int> updateCustomerSealIds)
        {
            CustomerSealJournal? sealQuery = dbContext.CustomerSealJournals.FirstOrDefault
                                                    (
                                                        customerSealJournal => customerSealJournal.CustomerSealQuarterJournal.Id == customerSealSequenceCheck.CustomerSealQuarterJournalId                                                                                                  
                                                        && customerSealJournal.ConfigType == customerSealSequenceCheck.SealMappingConfigId
                                                        && customerSealJournal.Sequence == customerSealSequenceCheck.Sequence
                                                        && customerSealJournal.DeleteStatus == DeleteStatus.No
                                                        && !deleteCustomerSealIds.Contains(customerSealJournal.Id)
                                                        && !updateCustomerSealIds.Contains(customerSealJournal.Id)                                            
                                                    );

            return sealQuery != null;
        }

        /// <summary>
        /// 客戶印鑑狀態變更。
        /// </summary>
        /// <param name="customerSealQuarterId">客戶印鑑季度Id</param>
        /// <param name="reviewStatus">審查狀態</param>        
        private ResponseViewModel ChangeReviewStatus(int customerSealQuarterId, ReviewStatus reviewStatus)
        {
            ResponseViewModel response = new();
            int userId = 0;//從帳號驗證取得

            CustomerSealQuarterJournal? customerSealQuarterQuery = dbContext.CustomerSealQuarterJournals.Find(customerSealQuarterId);

            if (customerSealQuarterQuery != null)
            {
                customerSealQuarterQuery.UpdateDate = DateTime.Now;
                customerSealQuarterQuery.ReviewStatus = reviewStatus;
                customerSealQuarterQuery.UpdateUserId = userId;
                if(reviewStatus == ReviewStatus.Invalid)
                {
                    customerSealQuarterQuery.DeleteStatus = DeleteStatus.Yes;
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
