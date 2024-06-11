using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBEntities;
using DBEntities.Consts;
using DBEntities.Entities;
using DBEntities.Entities.AccountantModels;
using DBEntities.Entities.CustomerModels;
using DBEntities.Entities.TemplateModels;
using DBEntities.Entities.TypographicModels;
using DBEntities.Utils;
using DJImageLib.Utils;
using k8s.Models;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.TemporarySeal;
using SealTypographicWebAPI.Utils;
using Serilog;
using SixLabors.ImageSharp;

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

        ///<inheritdoc />
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
                        temporarySealViewModel.ImageBase64 = ImageTransparentUtil.ToDataUrlFromDataUrl(temporarySealViewModel.ImageBase64);                           
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

        ///<inheritdoc />
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
                temporarySealPaginateViewModel.TotalCount = temporarySealGroupQuery.Count();
            }
            temporarySealPaginateViewModel.Success();

            return temporarySealPaginateViewModel;
        }


        ///<inheritdoc />  
        public async Task<ResponseViewModel> New(TemporarySealForm temporarySealForm, int userId = 1)
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
                        BaseInputTemporarySealGroup(temporarySealGroup, true, userId);

                        typographicResources = await imageService.NewTypographyResource(temporarySealForm.Seals, customerQuery.Code, userId);
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

        ///<inheritdoc />
        public async Task<ResponseViewModel> Update(TemporarySealUpdateForm temporarySealUpdateForm, int userId = 1)
        {
            ResponseViewModel response = new();            
            TemporarySealGroup? temporarySealGroup = dbContext.TemporarySealGroups
                                                            .Include(temporarySealQuarterJournal => temporarySealQuarterJournal.Customer)
                                                            .Include(temporarySealQuarterJournal => temporarySealQuarterJournal.TypographicResources)
                                                            .FirstOrDefault(temporarySealGroup => temporarySealGroup.Id == temporarySealUpdateForm.Id);
            if(temporarySealGroup != null)
            {                
                //更新臨時章印鑑組
                foreach (TemporarySealUpdate temporarySealUpdate in temporarySealUpdateForm.SealsToUpdate)
                {
                    TypographicResource? temporarySealJournalQuery = temporarySealGroup.TypographicResources.FirstOrDefault(x => x.Id == temporarySealUpdate.Id);
                    if(temporarySealJournalQuery != null)
                    {
                        //新增更新後的臨時章
                        TemporarySeal temporarySeal = new()
                        {                            
                            Sequence = temporarySealUpdate.Sequence,                                                        
                            ImageBase64 = temporarySealUpdate.ImageBase64,
                            SealType = SealType.TemporarySeal
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
                    InputUtil.Set(deleteSeal, userId, false);
                }

                //新增臨時章
                await imageService.NewTypographyResource(temporarySealUpdateForm.SealsToCreate, temporarySealGroup.Customer.Code, userId);

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

        ///<inheritdoc />
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
    }
}
