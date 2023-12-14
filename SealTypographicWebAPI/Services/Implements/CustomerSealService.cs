using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Utils;
using DBEntities.Consts;
using DJLib.Models;
using AutoMapper.QueryableExtensions;
using SealTypographicWebAPI.Models.CustomerSeal;
using SealTypographicWebAPI.Models.Customer;
using DBEntities.Entities;
using DBEntities;
using DBEntities.Entities.TypographicModels;
using DBEntities.Entities.CustomerModels;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 客戶印鑑管理
    /// </summary>
    public class CustomerSealService : ICustomerSealService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ImageService imageService;        
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private readonly ILogger<CustomerSealService> logger;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        /// <param name="imageService"></param>
        /// <param name="logger"></param>
        public CustomerSealService(SealTypographicDbContext dbContext, IMapper mapper, ImageService imageService, ILogger<CustomerSealService> logger)
        {
            this.dbContext = dbContext;                 
            this.imageService = imageService;                   
            this.logger = logger;
            configurationProvider = mapper.ConfigurationProvider;
        }

        ///<inheritdoc />
        public CustomerPaginateViewModel GetPaginate(CustomerSearch customerSearch, TypographyType typographyType, int userId = 1)
        {
            logger.LogInformation("GetPaginate input {@customerSearch} typographyType: {@TypographyType} userId: {userId}"
                , customerSearch, typographyType, userId);

            CustomerPaginateViewModel customerPaginateViewModel = new();
            int companyId = 1;

            try
            {                
                IQueryable<Customer> customerQuery = dbContext.Customers                                                    
                                                    .Where
                                                    (
                                                        x => x.Company.Id == companyId
                                                        && x.DeleteStatus == DeleteStatus.No
                                                        && x.CustomerSealGroups.Any
                                                        (
                                                            customerSealQuery => customerSealQuery.TypographyType == typographyType
                                                            && customerSealQuery.DeleteStatus == DeleteStatus.No
                                                        )
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
                    customerPaginateViewModel.ViewModels = customerQuery                                                            
                                                            .Skip((customerSearch.PageNumber - 1) * customerSearch.PageSize)
                                                            .Take(customerSearch.PageSize)                                                            
                                                            .Select(customer => new CustomerViewModel()
                                                            {
                                                                Id = customer.Id,
                                                                Name = customer.Name,
                                                                BAN = customer.BAN!,
                                                                Code = customer.Code,
                                                                IsDraff = customer.CustomerSealGroups.Any(x => x.ReviewStatus == ReviewStatus.Draft),
                                                                IsPending = customer.CustomerSealGroups.Any(x => x.ReviewStatus == ReviewStatus.Pending),
                                                                IsReject = customer.CustomerSealGroups.Any(x => x.ReviewStatus == ReviewStatus.Refuse),
                                                                CustomerSealQuarterId = customer.CustomerSealGroups
                                                                                        .Where(
                                                                                                sealGroup => sealGroup.TypographyType == typographyType
                                                                                                && sealGroup.DeleteStatus == DeleteStatus.No
                                                                                        )
                                                                                        .OrderByDescending(x => x.Id)
                                                                                        .First().Id
                                                            })
                                                            .ToList();

                    PageUtil.SetPaginate(customerPaginateViewModel, customerSearch.PageNumber, customerSearch.PageSize, customerQuery.Count());
                    customerPaginateViewModel.Success();
                }
                else
                {
                    customerPaginateViewModel.CustomeNoData();
                }                
                logger.LogInformation("GetPaginate output {@Output}", customerPaginateViewModel);
            }
            catch (Exception ex) 
            {
                customerPaginateViewModel.Error();
                customerPaginateViewModel.Message = ex.Message;
                logger.LogError("GetPaginate error {@Error}", ex.Message);
            }
            
            return customerPaginateViewModel;
        }

        ///<inheritdoc />
        public CustomerSealQuarterPaginateViewModel GetQuarterYear(CustomerSealQuarterPaginateSearch customerSealQuarterPaginateSearch, 
            bool isTypographic, TypographyType typographyType, int userId = 1)
        {
            logger.LogInformation("GetQuarterYear input {@customerSealQuarterPaginateSearch} isTypographicUse: {@isTypographic} typographyType: {@TypographyType} userId: {@userId}"
                , customerSealQuarterPaginateSearch, isTypographic, typographyType, userId);

            CustomerSealQuarterPaginateViewModel customerSealQuarterPaginateViewModel = new ();

            try
            {
                IQueryable<CustomerSealGroup> customerSealGroupsQuery = dbContext.CustomerSealGroups
                                                                    .Include(customerSealGroups => customerSealGroups.QuarterYear)
                                                                    .Where
                                                                    (
                                                                        customerSealGroup => customerSealGroup.Customer.Id == customerSealQuarterPaginateSearch.CustomerId
                                                                        && customerSealGroup.TypographyType == typographyType
                                                                        && customerSealGroup.DeleteStatus == DeleteStatus.No
                                                                    );
                if (isTypographic)
                {
                    customerSealGroupsQuery = customerSealGroupsQuery.Where(customerSealGroup => customerSealGroup.ReviewStatus == ReviewStatus.Approval);
                }

                customerSealGroupsQuery = customerSealGroupsQuery.OrderBy(customerSealGroup => customerSealGroup.QuarterYear.Id).ThenBy(x => x.Id);

                if (customerSealGroupsQuery.Any())
                {
                    customerSealQuarterPaginateViewModel.CustomerSealQuarters = customerSealGroupsQuery
                                                                                .Skip((customerSealQuarterPaginateSearch.PageNumber - 1) * customerSealQuarterPaginateSearch.PageSize)
                                                                                .Take(customerSealQuarterPaginateSearch.PageSize)
                                                                                .ProjectTo<CustomerSealQuarterViewModel>(configurationProvider)
                                                                                .ToList();

                    PageUtil.SetPaginate(customerSealQuarterPaginateViewModel, customerSealQuarterPaginateSearch.PageNumber
                        , customerSealQuarterPaginateSearch.PageSize, customerSealGroupsQuery.Count());
                    customerSealQuarterPaginateViewModel.Success();
                }
                else
                {
                    customerSealQuarterPaginateViewModel.CustomerSealNoData();
                }
                logger.LogInformation("GetQuarterYear output {@Output}", customerSealQuarterPaginateViewModel);
            }
            catch (Exception ex)
            {
                customerSealQuarterPaginateViewModel.Error();
                customerSealQuarterPaginateViewModel.Message = ex.Message;
                logger.LogError("GetQuarterYear error {@Error}", ex.Message);
            }
            
            return customerSealQuarterPaginateViewModel;
        }

        ///<inheritdoc />
        public CustomerSealGroupResponse GetCustomerSealGroupSummry(int customerId, int quaterId, int userId)
        {
            logger.LogInformation("GetCustomerSealGroupSummry input customerId: {@customerId} quaterId: {@quaterId} userId: {@userId}", customerId, quaterId, userId);

            CustomerSealGroupResponse? customerSealGroupResponse;            

            try
            {
                customerSealGroupResponse = dbContext.CustomerSealGroups
                                            .Include(customerSealGroups => customerSealGroups.QuarterYear)
                                            .Where
                                            (
                                                customerSealGroup => customerSealGroup.Customer.Id == customerId
                                                && customerSealGroup.QuarterYear.Id == quaterId
                                                && customerSealGroup.ReviewStatus == ReviewStatus.Approval
                                                && customerSealGroup.DeleteStatus == DeleteStatus.No
                                            )
                                            .ProjectTo<CustomerSealGroupResponse>(configurationProvider)
                                            .FirstOrDefault();

                if (customerSealGroupResponse != null)
                {
                    customerSealGroupResponse.Success();
                }
                else
                {
                    customerSealGroupResponse = new();
                    customerSealGroupResponse.CustomerSealNoData();
                }
                logger.LogInformation("GetCustomerSealGroupSummry output {@output}", customerSealGroupResponse);
            }
            catch (Exception ex)
            {
                customerSealGroupResponse = new();
                customerSealGroupResponse.Error();
                customerSealGroupResponse.Message = ex.Message;
                logger.LogInformation("GetCustomerSealGroupSummry error {@error}", ex.Message);
            }

            return customerSealGroupResponse;
        }

        ///<inheritdoc />
        public CustomerSealViewModels GetSeals(int customerSealQuarterId, bool isTransparent, int userId = 1)
        {
            logger.LogInformation("GetSeals input customerSealQuarterId: {@customerSealQuarterId} isTransparent= {@isTransparent} userId: {@userId}"
                , customerSealQuarterId, isTransparent, userId);

            CustomerSealViewModels? customerSealViewModels;

            try
            {
                customerSealViewModels = dbContext.CustomerSealGroups
                                        .Include(customerSealGroup => customerSealGroup.TypographicResources)
                                        .Include(customerSealGroup => customerSealGroup.QuarterYear)
                                        .Where(x => x.Id == customerSealQuarterId)
                                        .ProjectTo<CustomerSealViewModels>(configurationProvider)
                                        .FirstOrDefault();

                if (customerSealViewModels != null)
                {
                    if (isTransparent)
                    {
                        foreach (CustomerSealViewModel customerSealViewModel in customerSealViewModels.SealViewModels)
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
                logger.LogInformation("GetSeals output {@Output}", customerSealViewModels);
            }
            catch (Exception ex) 
            {
                customerSealViewModels = new();
                customerSealViewModels.Error();
                logger.LogError("GetSeals error {@Error}", ex.Message);
            }                         
            
            return customerSealViewModels;
        }

        ///<inheritdoc />
        public async Task<ResponseViewModel> New(CustomerSealForm customerSealForm, TypographyType typographyType, int userId = 1)
        {
            logger.LogInformation("New input {@customerSealForm} typographyType: {@typographyType} userId: {@userId}", customerSealForm, typographyType, userId);    
            
            ResponseViewModel response = new();            

            try
            {                
                Customer? customerQuery = dbContext.Customers
                                        .Include(customer => customer.CustomerSealGroups)
                                        .ThenInclude(customerSealGroup => customerSealGroup.QuarterYear)
                                        .FirstOrDefault(x => x.Id == customerSealForm.CustomerId);

                if (customerQuery != null)
                {
                    //從前端提供Id
                    QuarterYear quarter = dbContext.QuarterYears.Single(x => x.Id == customerSealForm.QuarterYearId);

                    CustomerSealGroup? customerSealGroupQuery = customerQuery.CustomerSealGroups
                                                                .FirstOrDefault
                                                                (
                                                                    customerSealQuarterJournal =>
                                                                    customerSealQuarterJournal.QuarterYear.Id == quarter.Id
                                                                    && customerSealQuarterJournal.DeleteStatus == DeleteStatus.No
                                                                );

                    if (customerSealGroupQuery == null)
                    {
                        CustomerSealGroup customerSealGroup = new();

                        ImageBase64Info imageBase64Info = imageService.SetImageBase64InfoWithSeal(customerQuery.Code, SealType.Customer);

                        customerSealGroup.QuarterYear = quarter;
                        customerSealGroup.TypographyType = typographyType;
                        BaseInput(customerSealGroup, true, userId);
                        //新增印鑑資料(圖檔與DB資源)
                        customerSealGroup.TypographicResources = await NewTypographyResource(customerSealForm.Seals, imageBase64Info, userId);

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
                logger.LogInformation("New output {@Output}", response);
            }
            catch (DbUpdateException ex)
            {
                response.DbError();                
                logger.LogError("New dbError {@DbError}", ex.Message);
            }
            catch (Exception ex) 
            {
                response.Error();                
                logger.LogError("New error {@Error}", ex.Message);
            }
            
            return response;
        }

        ///<inheritdoc />
        public async Task<List<ResponseViewModel>> Update(CustomerSealUpdate customerSealUpdate, int userId = 1)
        {
            logger.LogInformation("Update input {@customerSealUpdate} userId: {@userId}", customerSealUpdate, userId);

            List<ResponseViewModel> responseViewModels = new();                        

            try
            {
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

                    //新增印鑑資料(圖檔與DB資源)
                    customerSealGroup.TypographicResources = await NewTypographyResource(customerSealUpdate.CreateCustomerSeals, imageBase64Info, userId);

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
                logger.LogInformation("Update output {@Output}", responseViewModels);
            }
            catch (DbUpdateException ex)
            {
                ResponseViewModel response = new()
                {
                    Message = ex.Message
                };
                response.DbError();
                responseViewModels.Add(response);
                logger.LogError("Update dbError {@DbError}", ex.Message);
            }
            catch (Exception ex)
            {
                ResponseViewModel response = new()
                {
                    Message = ex.Message
                };
                response.Error();
                responseViewModels.Add(response);
                logger.LogError("Update error {@Error}", ex.Message);
            }            
            
            return responseViewModels;
        }

        ///<inheritdoc />  
        public ResponseViewModel ChangeReviewStatus(int customerSealQuarterId, ReviewStatus reviewStatus,int userId = 1)
        {
            logger.LogInformation("ChangeReviewStatus input customerSealQuarterId: {@CustomerSealQuarterId} reviewStatus: {ReviewStatus} userId {@userId}"
                , customerSealQuarterId, reviewStatus, userId);

            ResponseViewModel response = new();            

            try
            {
                CustomerSealGroup? customerSealGroup = dbContext.CustomerSealGroups.Find(customerSealQuarterId);

                if (customerSealGroup != null)
                {
                    if (reviewStatus == ReviewStatus.Invalid)
                    {
                        customerSealGroup.DeleteStatus = DeleteStatus.Yes;
                        if (customerSealGroup.ReviewStatus == ReviewStatus.Approval)
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
                logger.LogInformation("ChangeReviewStatus output {@Output} ", response);
            }
            catch (DbUpdateException ex)
            {
                response.DbError();                
                logger.LogError("ChangeReviewStatus dbError {@DbError}", ex.Message);
            }
            catch (Exception ex) 
            {
                response.Error();                
                logger.LogError("ChangeReviewStatus error {@Error}", ex.Message);
            }
            
            return response;
        }

        /// <summary>
        /// 客戶印鑑新增修改時基本的資料輸入
        /// </summary>
        /// <param name="customerSealGroup">Db上的印鑑資料</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">userId</param>
        private static void BaseInput(CustomerSealGroup customerSealGroup, bool isCreate, int userId)
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
        /// <param name="formSeals">輸入</param>        
        /// <param name="imageBase64Info">圖檔資訊</param>
        /// <param name="userId">使用者Id</param>
        /// <returns></returns>
        private async Task<List<TypographicResource>> NewTypographyResource(List<CustomerSeal> formSeals, ImageBase64Info imageBase64Info, int userId)
        {
            List<TypographicResource> typographyResources = new();
            foreach (CustomerSeal customerSeal in formSeals)
            {
                TypographicResource typographyResource = new()
                {
                    SealType = SealType.Customer,
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
            return typographyResources;
        }
    }
}
