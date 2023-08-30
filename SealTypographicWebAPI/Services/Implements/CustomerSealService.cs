using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Utils;
using DBEntities;
using DBEntities.Consts;
using DJLib.Models;
using AutoMapper.QueryableExtensions;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 客戶印鑑管理
    /// </summary>
    public class CustomerSealService : ICustomerSealService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ImageService imageService;
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        /// <param name="imageService"></param>
        public CustomerSealService(SealTypographicDbContext dbContext, IMapper mapper, ImageService imageService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;
            this.imageService = imageService;                   
        }       

        /// <summary>
        /// 取得客戶印鑑季度表(分頁)
        /// </summary>
        /// <param name="customerSealQuarterPaginateSearch">印鑑季度分頁搜尋</param>
        /// <param name="isTypographic">是否排版使用</param>
        /// <returns></returns>
        public CustomerSealQuarterPaginateViewModel GetQuarter(CustomerSealQuarterPaginateSearch customerSealQuarterPaginateSearch, bool isTypographic)
        {
            CustomerSealQuarterPaginateViewModel customerSealQuarterPaginateViewModel = new ();

            IQueryable<CustomerSealGroup> customerSealGroupsQuery = dbContext.CustomerSealGroups
                                                                .Include(customerSealGroups => customerSealGroups.Quarter)
                                                                .Where
                                                                (
                                                                    customerSealGroup => customerSealGroup.Customer.Id == customerSealQuarterPaginateSearch.CustomerId
                                                                    && customerSealGroup.DeleteStatus == DeleteStatus.No                                                                                    
                                                                );                                         
            if(isTypographic)
            {
                customerSealGroupsQuery = customerSealGroupsQuery.Where(customerSealGroup => customerSealGroup.ReviewStatus == ReviewStatus.Approval);
            }

            customerSealGroupsQuery = customerSealGroupsQuery.OrderBy(customerSealGroup => customerSealGroup.Quarter.Id);

            if (customerSealGroupsQuery.Any())
            {                                
                customerSealQuarterPaginateViewModel.CustomerSealQuarters = customerSealGroupsQuery                                                                                
                                                                            .Skip((customerSealQuarterPaginateSearch.PageNumber - 1) * customerSealQuarterPaginateSearch.PageSize)
                                                                            .Take(customerSealQuarterPaginateSearch.PageSize)                                                                            
                                                                            .ProjectTo<CustomerSealQuarterViewModel>(configurationProvider)
                                                                            .ToList();

                customerSealQuarterPaginateViewModel.TotalCount = customerSealGroupsQuery.Count();
                //計算總頁數
                customerSealQuarterPaginateViewModel.TotalPage = TotalPageUtil.GetTotalPage(customerSealQuarterPaginateViewModel.TotalCount, customerSealQuarterPaginateSearch.PageSize);
                customerSealQuarterPaginateViewModel.PageNumber = customerSealQuarterPaginateSearch.PageNumber;
                customerSealQuarterPaginateViewModel.PageSize = customerSealQuarterPaginateSearch.PageSize;                
                
                customerSealQuarterPaginateViewModel.Success();
            }
            else
            {
                customerSealQuarterPaginateViewModel.CustomerSealNoData();
            }

            return customerSealQuarterPaginateViewModel;
        }


        /// <summary>
        /// 取得客戶印鑑組
        /// </summary>
        /// <param name="customerSealQuarterId">客戶印鑑季度Id</param>
        /// <param name="isTransparent">是否白底透明化</param>
        /// <returns></returns>
        public CustomerSealViewModels GetSeals(int customerSealQuarterId, bool isTransparent)
        {            
            CustomerSealViewModels? customerSealViewModels = dbContext.CustomerSealGroups
                                                            .Include(customerSealGroup => customerSealGroup.TypographicResources)
                                                            .Include(customerSealGroup => customerSealGroup.Quarter) 
                                                            .Where(x => x.Id == customerSealQuarterId)
                                                            .ProjectTo<CustomerSealViewModels>(configurationProvider)
                                                            .FirstOrDefault();            
            
            if (customerSealViewModels != null)
            {
                if(isTransparent)
                {
                    foreach(CustomerSealViewModel customerSealViewModel in customerSealViewModels.SealViewModels)
                    {
                        ImageInfo imageInfo = ImageInfo.FromImageBase64(customerSealViewModel.ImageBase64);                        
                        customerSealViewModel.ImageBase64 = imageInfo.TransparentToImageBase64();
                    }
                }
                customerSealViewModels.Success();
            }
            else
            {
                customerSealViewModels = new();
                customerSealViewModels.CustomerSealNoData();
            }
        
            return customerSealViewModels;
        }

        /// <summary>
        /// 新增客戶印鑑組資料
        /// </summary>
        /// <param name="customerSealForms">客戶印鑑組資料</param>
        /// <returns></returns>
        public async Task<ResponseViewModel> New(CustomerSealForm customerSealForms)
        {
            ResponseViewModel response = new();            
            List<TypographicResource> typographyResources = new();
            int userId = 1; //以後從帳號驗證取得Id

            Customer? customerQuery = dbContext.Customers.Include(customer => customer.CustomerSealGroups)
                                       .ThenInclude(customerSealGroup => customerSealGroup.Quarter)
                                       .FirstOrDefault(x => x.Id == customerSealForms.CustomerId);            

            if (customerQuery != null)
            {
                //之後輸入要從前端提供Id
                Quarter quarter = dbContext.Quarters
                                    .Single
                                    (
                                        x => x.TaiwanYear == customerSealForms.Quarter.Substring(0, 3)
                                        && x.Period == customerSealForms.Quarter.Substring(3)
                                    );

                CustomerSealGroup? customerSealGroupQuery = customerQuery.CustomerSealGroups                                                            
                                                            .FirstOrDefault
                                                            (
                                                                customerSealQuarterJournal => 
                                                                customerSealQuarterJournal.Quarter.Id == quarter.Id
                                                                && customerSealQuarterJournal.DeleteStatus == DeleteStatus.No                                                                        
                                                            );

                if (customerSealGroupQuery == null)
                {
                    CustomerSealGroup customerSealGroup = new();
                    //Consts之後要調整到新的Db
                    ImageBase64Info imageBase64Info = imageService.SetImageBase64InfoWithSeal(customerQuery.Code, SealType.Customer);

                    customerSealGroup.Quarter = quarter;                    
                    BaseInputQuarterJournal(customerSealGroup, true, userId);                   
                    await NewTypographyResource(customerSealForms.Seals, typographyResources, imageBase64Info, userId);

                    customerSealGroup.TypographicResources = typographyResources;
                    customerQuery.CustomerSealGroups.Add(customerSealGroup);                    
                    await dbContext.SaveChangesAsync();
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
        public async Task<List<ResponseViewModel>> Update(CustomerSealUpdate customerSealUpdate)
        {            
            List<ResponseViewModel> responseViewModels = new();            
            int userId = 1; //從帳號驗證取得Id          

            CustomerSealGroup? customerSealGroup = dbContext.CustomerSealGroups
                                                .Include(customerSealGroup => customerSealGroup.Customer)
                                                .Include(customerSealGroup => customerSealGroup.TypographicResources.Where(x => x.DeleteStatus == DeleteStatus.No))
                                                .FirstOrDefault
                                                (
                                                    customerSealGroup => customerSealGroup.Id == customerSealUpdate.CustomerSealQuarterId                                                                                                                                                
                                                );

            if (customerSealGroup != null)
            {
                ImageBase64Info imageBase64Info = imageService.SetImageBase64InfoWithSeal(customerSealGroup.Customer.Code, SealType.Customer);

                //修改印鑑(更新ID移入DeleteCustomerSealIds，更新的資料移入CreateCustomerSeals，之後下一階段調整輸入時要拔掉此項)
                foreach (CustomerSealUpdateForm customerSealFormUpdate in customerSealUpdate.UpdateCustomerSeals)
                {                    
                    TypographicResource? updateSealQuery = customerSealGroup.TypographicResources.FirstOrDefault(x => x.Id == customerSealFormUpdate.Id);                    
                    if (updateSealQuery != null)
                    {
                        CustomerSeal customerSeal = new()
                        {
                            Sequence = updateSealQuery.Sequence,                            
                            SealMappingConfigId = SealMappingConfigUtil.GetCustomerSealType(updateSealQuery.SubSealType),
                            ImageBase64 = customerSealFormUpdate.ImageBase64
                        };

                        //更新ID丟入DeleteCustomerSealIds
                        customerSealUpdate.DeleteCustomerSealIds.Add(customerSealFormUpdate.Id);
                        //更新的印鑑移入新增
                        customerSealUpdate.CreateCustomerSeals.Add(customerSeal);                     
                    }
                    else
                    {
                        ResponseViewModel response = new();
                        response.UpdateCustomerSealNoData();
                        response.ErrorItem = $"Update CustomerSealId:{customerSealFormUpdate.Id}";
                        responseViewModels.Add(response);
                    }
                }

                //刪除印鑑
                IQueryable<TypographicResource>? deleteSealQuery = customerSealGroup.TypographicResources
                                                                  .Where(x => customerSealUpdate.DeleteCustomerSealIds.Contains(x.Id)).AsQueryable();
                if (deleteSealQuery.Any())
                {
                    foreach (TypographicResource deleteSeal in deleteSealQuery)
                    {
                        //原印鑑刪除(Hide)
                        deleteSeal.DeleteStatus = DeleteStatus.Yes;
                        TypographicResourceUtil.BaseInputTypographyResource(deleteSeal, false, userId);
                    }
                }

                //新增印鑑     
                await NewTypographyResource(customerSealUpdate.CreateCustomerSeals, customerSealGroup.TypographicResources, imageBase64Info, userId);
                           
                //無任何回傳訊息(錯誤訊息)就更新資料庫
                if (!responseViewModels.Any())
                {
                    ResponseViewModel response = new();                    
                    customerSealGroup.ReviewStatus = ReviewStatus.Draft;
                    
                    await dbContext.SaveChangesAsync();
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
        /// <param name="customerSealGroup">Db上的印鑑資料</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">userId</param>
        private static void BaseInputQuarterJournal(CustomerSealGroup customerSealGroup, bool isCreate, int userId)
        {
            if (isCreate)
            {
                customerSealGroup.CreateUserId = userId;
                customerSealGroup.CreateDate = DateTime.Now;
                customerSealGroup.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                customerSealGroup.UpdateUserId = userId;
                customerSealGroup.UpdateDate = DateTime.Now;
            }
            customerSealGroup.StartDate = DateUtil.NotActivated();
            customerSealGroup.EndDate = DateUtil.NotActivated();                
            customerSealGroup.ReviewStatus = ReviewStatus.Draft;
        }

        /// <summary>
        /// 新增印鑑資料
        /// </summary>
        /// <param name="formseals">輸入</param>
        /// <param name="typographyResources">要輸入資料庫的資源</param>
        /// <param name="imageBase64Info">圖檔資訊</param>
        /// <param name="userId">使用者Id</param>
        /// <returns></returns>
        private async Task NewTypographyResource(List<CustomerSeal> formseals, List<TypographicResource> typographyResources, ImageBase64Info imageBase64Info, int userId)
        {
            foreach (CustomerSeal customerSeal in formseals)
            {
                TypographicResource typographyResource = new()
                {
                    SealType = SealType.Customer,
                    //輸入model之後要修正為新的db
                    SubSealType = SealMappingConfigUtil.GetSubSealTypeWithCustomer(customerSeal.SealMappingConfigId),
                    Sequence = customerSeal.Sequence
                };
                //ImageBase64轉圖檔並存到指定資料夾
                imageBase64Info.ImageBase64 = customerSeal.ImageBase64;
                typographyResource.ImageFullPath = await imageService.GetSavedImageFilePath(imageBase64Info);
                typographyResource.ThumbnailFullPath = await imageService.GetSavedImageThumbnailFilePath(imageBase64Info, true);

                TypographicResourceUtil.BaseInputTypographyResource(typographyResource, true, userId);
                typographyResources.Add(typographyResource);
            }
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

            CustomerSealGroup? customerSealGroup = dbContext.CustomerSealGroups.Find(customerSealQuarterId);

            if (customerSealGroup != null)
            {
                if(reviewStatus == ReviewStatus.Invalid)
                {
                    customerSealGroup.DeleteStatus = DeleteStatus.Yes;
                    if(customerSealGroup.ReviewStatus == ReviewStatus.Approval)
                    {
                        customerSealGroup.EndDate = DateTime.Now;
                    }                    
                }
                customerSealGroup.ReviewStatus = reviewStatus;
                customerSealGroup.UpdateDate = DateTime.Now;                
                customerSealGroup.UpdateUserId = userId;
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
