using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Utils;
using DBEntities;
using DBEntities.Consts;
using AutoMapper.QueryableExtensions;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 信頭管理
    /// </summary>
    public class LetterheadService : ILetterheadService
    {
        private readonly SealTypographicDbContext dbContext;        
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private readonly ILogger<LetterheadService> logger;

        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public LetterheadService(SealTypographicDbContext dbContext, IMapper mapper, ILogger<LetterheadService> logger)
        {
            this.dbContext = dbContext;            
            configurationProvider = mapper.ConfigurationProvider;
            this.logger = logger;
        }

        ///<inheritdoc />
        public LetterheadPaginateViewModel GetPaginate(LetterheadSearch letterheadSearch, int userId = 0)
        {
            logger.LogInformation("GetPaginate input {@letterheadSearch} userId: {@userId}", letterheadSearch, userId);

            LetterheadPaginateViewModel letterheadPaginateViewModel = new ();            

            try
            {
                IQueryable<Letterhead> letterheadQuery = dbContext.Letterheads.Where(letterhead => letterhead.DeleteStatus == DeleteStatus.No)
                                    .Include(letterhead => letterhead.TypographicResources);

                if (!string.IsNullOrWhiteSpace(letterheadSearch.Name))
                {
                    letterheadQuery = letterheadQuery.Where
                                    (
                                        letterhead => letterhead.Name.Contains(letterheadSearch.Name)
                                    );
                }

                letterheadQuery = letterheadQuery.OrderBy(letterhead => letterhead.Id);

                if (letterheadQuery.Any())
                {
                    //取得該頁  
                    letterheadPaginateViewModel.ViewModels = letterheadQuery
                                                              .Skip((letterheadSearch.PageNumber - 1) * letterheadSearch.PageSize)
                                                              .Take(letterheadSearch.PageSize)
                                                              .ProjectTo<LetterheadViewModel>(configurationProvider)
                                                              .ToList();

                    PageUtil.SetPaginate(letterheadPaginateViewModel, letterheadSearch.PageNumber, letterheadSearch.PageSize, letterheadQuery.Count());
                    letterheadPaginateViewModel.Success();
                }
                else
                {
                    letterheadPaginateViewModel.DbNoData();
                }
                logger.LogInformation("GetPaginate output {@output}", letterheadPaginateViewModel);
            }
            catch (Exception ex) 
            {
                letterheadPaginateViewModel.Error();
                logger.LogInformation("GetPaginate error {@error}", ex.Message);
            }            

            return letterheadPaginateViewModel;
        }

        ///<inheritdoc />
        public ResponseViewModel Delete(int id, int userId = 0)
        {
            logger.LogInformation("Delete input id: {@id} userId: {userId}", id, userId);

            ResponseViewModel response = new();       
            
            try
            {
                Letterhead? letterheadQuery = dbContext.Letterheads.Include(x => x.TypographicResources)
                                          .FirstOrDefault(x => x.Id == id);

                if (letterheadQuery != null)
                {
                    letterheadQuery.DeleteStatus = DeleteStatus.Yes;
                    letterheadQuery.Status = LetterheadImageStatus.Disabled; //應該用不到之後移除或是未來需要審查時在來調整。
                    BaseInputLetterhead(letterheadQuery, false, userId);

                    foreach (TypographicResource typographicResource in letterheadQuery.TypographicResources)
                    {
                        typographicResource.DeleteStatus = DeleteStatus.Yes;
                    }

                    dbContext.SaveChanges();
                    response.Success();
                }
                else
                {
                    response.DbNoData();
                }
                logger.LogInformation("Delete output {@output}", response);
            }
            catch (DbUpdateException ex)
            {
                response.DbError();
                logger.LogInformation("GetPaginate dberror {@dberror}", ex.Message);
            }
            catch (Exception ex) 
            {
                response.Error();
                logger.LogInformation("GetPaginate error {@error}", ex.Message);
            }
            
            return response;            
        }

        /// <summary>
        /// 信頭資料新增修改時基本資料輸入
        /// </summary>
        /// <param name="letterhead">DB上的信頭資料</param>
        /// <param name="isCreate">確認是否新增的動作</param>
        /// <param name="userid">使用者ID</param>
        private static void BaseInputLetterhead(Letterhead letterhead, bool isCreate, int userid)
        {
            if(isCreate)
            {
                letterhead.CreateUserId = userid;
                letterhead.CreateDate = DateTime.Now;
                letterhead.DeleteStatus = DeleteStatus.No;                
            }
            else
            {
                letterhead.UpdateUserId = userid;
                letterhead.UpdateDate = DateTime.Now;
            }            
        }
    }


}