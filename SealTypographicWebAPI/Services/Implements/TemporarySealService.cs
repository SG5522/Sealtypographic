using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBEntities;
using DBEntities.Consts;
using DJLib.Models;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.TemporarySeal;
using SealTypographicWebAPI.Utils;
using Serilog;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 管理臨時章資料
    /// </summary>
    public class TemporarySealService : ITemporarySealService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ImageService imageService;
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="imageService"></param>
        /// <param name="mapper"></param>        
        public TemporarySealService(SealTypographicDbContext dbContext, ImageService imageService, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.imageService = imageService;
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;
        }

        /// <summary>
        /// 取得臨時章詳細基本資料
        /// </summary>
        /// <param name="temporaryId">臨時章ID</param>
        /// <param name="isTransparent">是否白底透明化</param>
        /// <returns></returns>
        public TemporarySealDetailViewModel GetDetail(int temporaryId, bool isTransparent)
        {            
            TemporarySealDetailViewModel? temporarySealDetailViewModel = dbContext.TemporarySealGroups
                                                                        .Include(x => x.TypographicResources)
                                                                        .Include(x => x.Customer)                                                                        
                                                                        .Where(x => x.Id == temporaryId)                                                                          
                                                                        .ProjectTo<TemporarySealDetailViewModel>(configurationProvider)                                                                        
                                                                        .FirstOrDefault();

            if(temporarySealDetailViewModel != null)
            {               
                if (isTransparent)
                {
                    foreach (TemporarySealViewModel temporarySealViewModel in temporarySealDetailViewModel.ViewModels)
                    {
                        ImageInfo imageInfo = ImageInfo.FromImageBase64(temporarySealViewModel.ImageBase64);
                        temporarySealViewModel.ImageBase64 = imageInfo.TransparentToImageBase64();
                    }
                }                
                temporarySealDetailViewModel.Success();                
                Log.Information("TemporarySeal detail output {@Output}", mapper.Map<TemporarySealDetailLogModel>(temporarySealDetailViewModel));
            }
            else
            {
                temporarySealDetailViewModel = new();
                temporarySealDetailViewModel.TemporarySealNoData();
            }
            return temporarySealDetailViewModel;
        }

        /// <summary>
        /// 取得臨時章資料列表(分頁)
        /// </summary>
        /// <param name="temporarySealSearch">臨時章分頁搜尋</param>        
        /// <returns></returns>
        public TemporarySealPaginateViewModel GetPaginate(TemporarySealSearch temporarySealSearch)
        {
            TemporarySealPaginateViewModel temporarySealPaginateViewModel = new();
            
            int companyId = 1;

            IQueryable<TemporarySealGroup> temporarySealGroupQuery = dbContext.TemporarySealGroups
                                                                    .Where
                                                                    (
                                                                        temporarySealGroup => temporarySealGroup.Customer.Company.Id == companyId
                                                                        && temporarySealGroup.Customer.DeleteStatus == DeleteStatus.No
                                                                        && temporarySealGroup.DeleteStatus == DeleteStatus.No
                                                                    );

            if (!string.IsNullOrEmpty(temporarySealSearch.KeyWord))
            {
                temporarySealGroupQuery = temporarySealGroupQuery.Where(temporarySealGroup => temporarySealGroup.Customer.Name.Contains(temporarySealSearch.KeyWord));
                //排板時會先取得客戶Id在過濾搜尋
                if(temporarySealSearch.CustomerId != null)
                {
                    temporarySealGroupQuery = temporarySealGroupQuery.Where(temporarySealGroup => temporarySealGroup.Customer.Id == temporarySealSearch.CustomerId);
                }
            }
            temporarySealGroupQuery = temporarySealGroupQuery.OrderByDescending(temporarySealGroup => temporarySealGroup.Customer.Id)
                                                             .ThenByDescending(temporarySealGroup => temporarySealGroup.QuarterYear);

            if(temporarySealGroupQuery.Any())
            {                
                temporarySealPaginateViewModel.ViewModels = temporarySealGroupQuery
                                                            .Include(temporarySealGroup => temporarySealGroup.Customer)
                                                            .Skip((temporarySealSearch.PageNumber - 1) * temporarySealSearch.PageSize)
                                                            .Take(temporarySealSearch.PageSize)
                                                            .ProjectTo<TemporaryViewModel>(configurationProvider)
                                                            .ToList();

                temporarySealPaginateViewModel.PageNumber = temporarySealSearch.PageNumber;
                temporarySealPaginateViewModel.PageSize = temporarySealSearch.PageSize;
                //計算總頁數
                //temporarySealPaginateViewModel.TotalPage = PageUtil.GetTotalPage(temporarySealGroupQuery.Count(), temporarySealSearch.PageSize);
                temporarySealPaginateViewModel.TotalCount = temporarySealGroupQuery.Count();
            }
            temporarySealPaginateViewModel.Success();

            return temporarySealPaginateViewModel;
        }

        /// <summary>
        /// 新增客戶基本資料
        /// </summary>
        /// <param name="temporarySealForm">基本資料</param>
        public async Task<ResponseViewModel> New(TemporarySealForm temporarySealForm)
        {
            ResponseViewModel response = new ();

            //取得季度
            //之後輸入要從前端提供Id
            QuarterYear? quarter = dbContext.QuarterYears.FirstOrDefault
                                (
                                    x => x.Id == temporarySealForm.QuarterYearId                                    
                                );

            if (quarter != null)
            {
                Customer? customerQuery = dbContext.Customers
                                        .Include(customer => customer.TemporarySealGroups)
                                        .ThenInclude(temporarySealGroup => temporarySealGroup.TypographicResources)
                                        .FirstOrDefault
                                        (
                                            customer => customer.Id == temporarySealForm.CustomerId
                                        );

                if (customerQuery != null)
                {
                    if (!customerQuery.TemporarySealGroups.Any(x => x.QuarterYear == quarter))
                    {
                        TemporarySealGroup temporarySealGroup = new();
                        List<TypographicResource> typographicResources = new();
                        ImageBase64Info imageBase64Info = imageService.SetImageBase64InfoWithSeal(customerQuery.Code, SealType.TemporarySeal);
                        int userId = 1;

                        BaseInputTemporarySealGroup(temporarySealGroup, true, userId);
                        await NewTypographyResource(temporarySealForm.Seals, typographicResources, imageBase64Info, userId);
                        temporarySealGroup.QuarterYear = quarter;
                        temporarySealGroup.TypographicResources = typographicResources;
                        customerQuery.TemporarySealGroups.Add(temporarySealGroup);
                        dbContext.SaveChanges();
                        response.Success();
                    }
                    else
                    {
                        response.TemporarySealQuarterRepeat();
                    }
                }
                else
                {
                    response.CustomeNoData();
                }
            }
            else
            {
                response.QuarterOutOfRange();
            }

            return response;
        }

        /// <summary>
        /// 更新臨時章
        /// </summary>        
        /// <param name="temporarySealUpdateForm">基本資料</param>
        public async Task<ResponseViewModel> Update(TemporarySealUpdateForm temporarySealUpdateForm)
        {
            ResponseViewModel response = new();
            int userId = 1;
            TemporarySealGroup? temporarySealGroup = dbContext.TemporarySealGroups
                                                            .Include(temporarySealQuarterJournal => temporarySealQuarterJournal.Customer)
                                                            .Include(temporarySealQuarterJournal => temporarySealQuarterJournal.TypographicResources)
                                                            .FirstOrDefault(temporarySealGroup => temporarySealGroup.Id == temporarySealUpdateForm.Id);
            if(temporarySealGroup != null)
            {                
                ImageBase64Info imageBase64Info = imageService.SetImageBase64InfoWithSeal(temporarySealGroup.Customer.Code, SealType.TemporarySeal);

                //更新臨時章印鑑組
                foreach (TemporarySealUpdate temporarySealUpdate in temporarySealUpdateForm.SealsToUpdate)
                {
                    TypographicResource? temporarySealJournalQuery = temporarySealGroup.TypographicResources.FirstOrDefault(x => x.Id == temporarySealUpdate.Id);
                    if(temporarySealJournalQuery != null)
                    {
                        imageBase64Info.ImageBase64 = temporarySealUpdate.ImageBase64;
                        //新增更新後的臨時章
                        TemporarySeal temporarySeal = new()
                        {                            
                            Sequence = temporarySealUpdate.Sequence,                                                        
                            ImageBase64 = temporarySealUpdate.ImageBase64
                        };

                        //將更新的ID帶入刪除List
                        temporarySealUpdateForm.SealIdsToDelete.Add(temporarySealUpdate.Id);
                        //更新的印鑑移入新增
                        temporarySealUpdateForm.SealsToCreate.Add(temporarySeal);
                    }
                    else
                    {
                        response.ErrorItem += $"Update temporarySeal NoData Id:{temporarySealUpdate.Id}";
                    }
                }

                //找出需要刪除&更新的臨時章
                IQueryable<TypographicResource> deleteSeals = temporarySealGroup.TypographicResources
                                                                                .Where(x => temporarySealUpdateForm.SealIdsToDelete.Contains(x.Id))
                                                                                .AsQueryable();
                //標記為刪除
                foreach(TypographicResource deleteSeal in deleteSeals)
                {
                    deleteSeal.DeleteStatus = DeleteStatus.Yes;
                    TypographicResourceUtil.BaseInputTypographyResource(deleteSeal, false, userId);                    
                }

                //新增臨時章                   
                await NewTypographyResource(temporarySealUpdateForm.SealsToCreate, temporarySealGroup.TypographicResources, imageBase64Info, userId);

                if (response.ErrorItem == null)
                {                                        
                    dbContext.SaveChanges();
                    response.Success();
                }                
            }
            else
            {
                response.UpdateTemporarySealNoData();
            }

            return response;
        }

        /// <summary>
        /// 刪除臨時章。
        /// </summary>
        /// <param name="Id"></param>
        public ResponseViewModel Delete(int Id)
        {
            ResponseViewModel response = new();
            int userId = 1;
            TemporarySealGroup? temporarySealGroup = dbContext.TemporarySealGroups.Find(Id);
            if(temporarySealGroup != null)
            {
                temporarySealGroup.DeleteStatus = DeleteStatus.Yes;
                BaseInputTemporarySealGroup(temporarySealGroup, false, userId);
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.DeleteTemporarySealNoData();
            }
            return response;
        }

        /// <summary>
        /// 客戶印鑑新增修改時基本的資料輸入
        /// </summary>
        /// <param name="temporarySealGroup">Db上的印鑑資料</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">userId</param>
        private static void BaseInputTemporarySealGroup(TemporarySealGroup temporarySealGroup, bool isCreate, int userId)
        {
            if (isCreate)
            {
                temporarySealGroup.CreateUserId = userId;
                temporarySealGroup.CreateDate = DateTime.Now;
                temporarySealGroup.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                temporarySealGroup.UpdateUserId = userId;
                temporarySealGroup.UpdateDate = DateTime.Now;
            }
        }

        /// <summary>
        /// 新增印鑑資料
        /// </summary>
        /// <param name="formseals">輸入</param>
        /// <param name="typographicResources">要輸入資料庫的資源</param>
        /// <param name="imageBase64Info">圖檔資訊</param>
        /// <param name="userId">使用者Id</param>
        /// <returns></returns>
        private async Task NewTypographyResource(IList<TemporarySeal> formseals, IList<TypographicResource> typographicResources, ImageBase64Info imageBase64Info, int userId)
        {
            foreach (TemporarySeal temporarySeal in formseals)
            {
                TypographicResource typographyResource = new()
                {
                    SealType = SealType.TemporarySeal,
                    //輸入model之後要修正為新的db
                    SubSealType = SubSealType.TemporarySeal,
                    Sequence = temporarySeal.Sequence
                };
                //ImageBase64轉圖檔並存到指定資料夾
                imageBase64Info.ImageBase64 = temporarySeal.ImageBase64;
                typographyResource.ImageFullPath = await imageService.GetSavedImageFilePath(imageBase64Info);
                typographyResource.ThumbnailFullPath = await imageService.GetSavedImageThumbnailFilePath(imageBase64Info, true);

                TypographicResourceUtil.BaseInputTypographyResource(typographyResource, true, userId);
                typographicResources.Add(typographyResource);
            }
        }
    }
}
