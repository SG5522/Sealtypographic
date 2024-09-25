using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBEntities;
using DBEntities.Consts;
using DBEntities.Entities;
using DBEntities.Entities.CustomerModels;
using DBEntities.Entities.TemplateModels;
using DBEntities.Entities.TypographicModels;
using DBEntities.Utils;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.TemporarySeal;
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
        private readonly ILogger<TemporarySealService> logger;
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="imageService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>        
        public TemporarySealService(SealTypographicDbContext dbContext, ImageService imageService, ILogger<TemporarySealService> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.imageService = imageService;
            this.logger = logger;
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;            
        }

        ///<inheritdoc />
        public TemporarySealDetailViewModel GetDetail(int temporaryId, int userId, bool isTransparent)
        {
            TemporarySealDetailViewModel? temporarySealDetailViewModel;
            try
            {
                logger.LogInformation("GetDetail input temporaryId:{@temporaryId}, " +
                                      "userId:{@userId}, " +
                                      "isTransparent:{@isTransparent}", temporaryId, userId, isTransparent);

                temporarySealDetailViewModel = dbContext.TemporarySealGroups
                                                        .Include(x => x.TypographicResources)
                                                        .Include(x => x.Customer)
                                                        .Where(x => x.Id == temporaryId)
                                                        .ProjectTo<TemporarySealDetailViewModel>(configurationProvider)
                                                        .FirstOrDefault();

                if (temporarySealDetailViewModel != null)
                {
                    imageService.DecryptSeals(temporarySealDetailViewModel.ViewModels, userId, isTransparent);
                    temporarySealDetailViewModel.Success();
                    logger.LogInformation("GetDetail output {@Output}", mapper.Map<TemporarySealDetailLogModel>(temporarySealDetailViewModel));
                }
                else
                {
                    temporarySealDetailViewModel = new();
                    temporarySealDetailViewModel.TemporarySealNoData();
                }
            }
            catch (Exception ex)
            {
                logger.LogError("GetDetail error {@Error}", ex.Message);
                temporarySealDetailViewModel = new();
                temporarySealDetailViewModel.Error();
            }

            return temporarySealDetailViewModel;
        }

        ///<inheritdoc />
        public TemporarySealPaginateViewModel GetPaginate(TemporarySealSearch temporarySealSearch, int userId)
        {
            TemporarySealPaginateViewModel temporarySealPaginateViewModel = new();
            
            try
            {
                logger.LogInformation("GetPaginate input {@Input} userId: {@userId}", temporarySealSearch, userId);
                //使用者id:1(admin)時用公司Id:1 非1使用db查詢公司Id
                int? companyId = userId == 1 ? 1 : dbContext.Companys.FirstOrDefault(x => x.ApplicationUsers.Any(user => user.Id == userId))?.Id;

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
                    if (temporarySealSearch.CustomerId != null)
                    {
                        temporarySealGroupQuery = temporarySealGroupQuery.Where(temporarySealGroup => temporarySealGroup.Customer.Id == temporarySealSearch.CustomerId);
                    }
                }
                temporarySealGroupQuery = temporarySealGroupQuery.OrderByDescending(temporarySealGroup => temporarySealGroup.Customer.Id)
                                                                 .ThenByDescending(temporarySealGroup => temporarySealGroup.QuarterYear);

                if (temporarySealGroupQuery.Any())
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
                    temporarySealPaginateViewModel.Success();                    
                }
                else
                {
                    temporarySealPaginateViewModel.DbNoData();
                }
                logger.LogInformation("GetPaginate output {@Output}", temporarySealPaginateViewModel);
            }
            catch (Exception ex) 
            {
                Log.Error("GetPaginate error {@Error}", ex.Message);
                temporarySealPaginateViewModel.Error();
            }                                    
            return temporarySealPaginateViewModel;
        }


        ///<inheritdoc />  
        public async Task<ResponseViewModel> New(TemporarySealForm temporarySealForm, int userId = 1)
        {
            ResponseViewModel response = new ();

            try
            {
                logger.LogInformation("New input {@Input} userId {@userId}", temporarySealForm, userId);

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
                            InputUtil.Set(temporarySealGroup, userId, true);

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

                    logger.LogInformation("New output {@Output}", response);
                }
                else
                {
                    response.QuarterOutOfRange();
                }
            }
            catch (DbUpdateException ex)
            {
                logger.LogInformation("New DBerror {@DBError}", ex.InnerException);
                response.DbError();
            }
            catch (Exception ex)
            {
                logger.LogInformation("New error {@Error}", ex.Message);
                response.Error();
            }
            
            return response;
        }

        ///<inheritdoc />
        public async Task<ResponseViewModel> Update(TemporarySealUpdateForm temporarySealUpdateForm, int userId = 1)
        {
            ResponseViewModel response = new();
            try
            {
                logger.LogInformation("Update input temporarySealUpdateForm {@temporarySealUpdateForm}", temporarySealUpdateForm);
                TemporarySealGroup? temporarySealGroup = dbContext.TemporarySealGroups
                                                            .Include(temporarySealQuarterJournal => temporarySealQuarterJournal.Customer)
                                                            .Include(temporarySealQuarterJournal => temporarySealQuarterJournal.TypographicResources)
                                                            .FirstOrDefault(temporarySealGroup => temporarySealGroup.Id == temporarySealUpdateForm.Id);
                if (temporarySealGroup != null)
                {
                    //更新臨時章印鑑組
                    foreach (TemporarySealUpdate temporarySealUpdate in temporarySealUpdateForm.SealsToUpdate)
                    {
                        TypographicResource? temporarySealJournalQuery = temporarySealGroup.TypographicResources.FirstOrDefault(x => x.Id == temporarySealUpdate.Id);
                        if (temporarySealJournalQuery != null)
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
                    foreach (TypographicResource deleteSeal in deleteSeals)
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
                logger.LogInformation("Update output {@Output}", response);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError("Update DbError {@DbError}", ex.InnerException);
                response.DbError();
            }
            catch (Exception ex)
            {
                logger.LogError("Update error {@Error}", ex.Message);
                response.Error();
            }                           
            return response;
        }

        ///<inheritdoc />
        public ResponseViewModel Delete(int Id, int userId = 1)
        {
            ResponseViewModel response = new();
            try
            {
                logger.LogInformation("Delete input {@Input}", response);
                TemporarySealGroup? temporarySealGroup = dbContext.TemporarySealGroups.Find(Id);
                if (temporarySealGroup != null)
                {
                    temporarySealGroup.DeleteStatus = DeleteStatus.Yes;
                    InputUtil.Set(temporarySealGroup, userId);
                    dbContext.SaveChanges();
                    response.Success();
                }
                else
                {
                    response.DeleteTemporarySealNoData();
                }

                logger.LogInformation("Delete output {@Output}", response);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError("Delete Dberror {@Dberror}", ex.InnerException);
                response.DbError();
            }
            catch (Exception ex)
            {
                logger.LogError("Delete error {@Error}", ex.Message);
                response.DbError();
            }

            return response;
        }
    }
}
