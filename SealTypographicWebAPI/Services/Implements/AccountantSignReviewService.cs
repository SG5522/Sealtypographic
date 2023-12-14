using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantSignReview;
using SealTypographicWebAPI.Utils;
using DBEntities.Consts;
using AutoMapper.QueryableExtensions;
using DBEntities;
using DBEntities.Entities.AccountantModels;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 會計師簽印審核管理    
    /// </summary>
    public class AccountantSignReviewService : IAccountantSignReviewService
    {
        private readonly SealTypographicDbContext dbContext;        
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private readonly ILogger<AccountantSignReviewService> logger;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        /// <param name="logger"></param>    
        public AccountantSignReviewService(SealTypographicDbContext dbContext, IMapper mapper, ILogger<AccountantSignReviewService> logger)
        {
            this.dbContext = dbContext;
            configurationProvider = mapper.ConfigurationProvider;
            this.logger = logger;
        }

        ///<inheritdoc />
        public AccountantSignGroupReviewPaginate GetReviewPaginate(AccountantSignSearchReview accountantSignSearchReview, int userId = 1)
        {
            logger.LogInformation("GetReviewPaginate input {@input} userId {@userId}", accountantSignSearchReview, userId);

            AccountantSignGroupReviewPaginate accountantSignGroupReviewPaginate = new();

            try
            {
                IQueryable<AccountantSignGroup> accountantSignGroupQuery = dbContext.AccountantSignGroups
                                                                        .Include(x => x.Accountant)
                                                                        .Where
                                                                        (
                                                                            x => x.DeleteStatus == DeleteStatus.No
                                                                            && x.ReviewStatus < ReviewStatus.Disabled
                                                                        ).OrderByDescending(x => x.Id);

                if (!string.IsNullOrWhiteSpace(accountantSignSearchReview.KeyWord))
                {
                    accountantSignGroupQuery = accountantSignGroupQuery.Where
                                            (
                                                x =>
                                                x.Accountant.Code.ToLower().Contains(accountantSignSearchReview.KeyWord.ToLower())
                                                || x.Accountant.Name.Contains(accountantSignSearchReview.KeyWord)
                                                || x.Accountant.AccountantGroups.Any(x => x.Name.Contains(accountantSignSearchReview.KeyWord))
                                            );
                }

                if (accountantSignSearchReview.ReviewStatus != null)
                {
                    accountantSignGroupQuery = accountantSignGroupQuery.Where(x => x.ReviewStatus == accountantSignSearchReview.ReviewStatus);
                }

                if (accountantSignGroupQuery.Any())
                {
                    //取得該頁                            
                    accountantSignGroupReviewPaginate.ViewModels = accountantSignGroupQuery
                                                                .Include(x => x.TypographicResources)
                                                                .Include(x => x.Accountant)
                                                                .ThenInclude(x => x.AccountantGroups)
                                                                .Skip((accountantSignSearchReview.PageNumber - 1) * accountantSignSearchReview.PageSize)
                                                                .Take(accountantSignSearchReview.PageSize)
                                                                .ProjectTo<AccountantSignGroupReviewViewModel>(configurationProvider)
                                                                .ToList();

                    PageUtil.SetPaginate(accountantSignGroupReviewPaginate, accountantSignSearchReview.PageNumber, accountantSignSearchReview.PageSize, accountantSignGroupQuery.Count());
                    accountantSignGroupReviewPaginate.Success();
                }
                else
                {
                    accountantSignGroupReviewPaginate.DbNoData();
                }

                logger.LogInformation("GetReviewPaginate output {@output}", accountantSignGroupReviewPaginate);
            }
            catch (Exception ex)
            {
                accountantSignGroupReviewPaginate.Error();
                logger.LogInformation("GetReviewPaginate error {@error}", ex.Message);
            }            

            return accountantSignGroupReviewPaginate;
        }

        ///<inheritdoc />
        public AccountantSignGroupDetailReviewResponse GetReviewDetail(int accountantSignGroupId, int userId = 1)
        {
            logger.LogInformation("GetReviewDetail accountantSignGroupId {@accountantSignGroupId} userId {@userId}", accountantSignGroupId, userId);

            AccountantSignGroupDetailReviewResponse accountantSignGroupDetailReviewResponse = new();

            try
            {
                AccountantSignGroupDetailReviewViewModel? accountantSignGroupQuery = dbContext.AccountantSignGroups
                                                                                .Include(x => x.Accountant)
                                                                                .ThenInclude(x => x.AccountantGroups)
                                                                                .Include(x => x.TypographicResources)
                                                                                .Where(x => x.Id == accountantSignGroupId)
                                                                                .ProjectTo<AccountantSignGroupDetailReviewViewModel>(configurationProvider)
                                                                                .FirstOrDefault();

                if (accountantSignGroupQuery != null)
                {
                    accountantSignGroupDetailReviewResponse.ViewModel = accountantSignGroupQuery;
                    accountantSignGroupDetailReviewResponse.Success();
                }
                else 
                {
                    accountantSignGroupDetailReviewResponse.DbNoData();
                }
                logger.LogInformation("GetReviewDetail output {@output}", accountantSignGroupDetailReviewResponse);
            }
            catch (Exception ex)
            {
                accountantSignGroupDetailReviewResponse.Error();
                logger.LogInformation("GetReviewDetail error {@error}", ex.Message);
            }            
            
            return accountantSignGroupDetailReviewResponse;            
        }

        /// <summary>
        /// 更換審核狀態
        /// </summary>
        /// <param name="accountantSignGroupIds">會計師簽印群組Id</param>
        /// <param name="reviewStatus">審核狀態</param>
        /// <param name="userId">從Keycloak驗證取得</param>
        /// <returns></returns>
        public ResponseViewModel StatusChange(List<int> accountantSignGroupIds, ReviewStatus reviewStatus, int userId = 1)
        {
            logger.LogInformation("StatusChange accountantSignGroupIds: {@accountantSignGroupIds}, reviewStatus: {@reviewStatus}, userId: {@userId} "
                                    , accountantSignGroupIds, reviewStatus, userId);

            ResponseViewModel response = new();

            try
            {
                
                foreach (int accountantSignGroupId in accountantSignGroupIds)
                {
                    AccountantSignGroup? accountantSignGroupJournal = dbContext.AccountantSignGroups.Include(x => x.Accountant)
                                                                                                    .FirstOrDefault(x => x.Id == accountantSignGroupId);
                    if (accountantSignGroupJournal != null)
                    {
                        accountantSignGroupJournal.ReviewUserId = userId;
                        accountantSignGroupJournal.ReviewStatus = reviewStatus;
                        accountantSignGroupJournal.ReviewDate = DateTime.Now;
                        switch (reviewStatus)
                        {
                            case ReviewStatus.Approval:
                                //變更啟用與結束日期
                                accountantSignGroupJournal.StartDate = DateTime.Now;
                                accountantSignGroupJournal.EndDate = DateTime.Parse("9999/12/31");
                                //找出審核通過的簽印組
                                IQueryable<AccountantSignGroup>? accountantSignGroups = dbContext.AccountantSignGroups
                                                                                       .Where
                                                                                       (
                                                                                           x => x.Accountant.Id == accountantSignGroupJournal.Accountant.Id
                                                                                           && x.Id != accountantSignGroupId
                                                                                           && x.ReviewStatus == ReviewStatus.Approval
                                                                                       );
                                //如有審核通過的簽印組則停用
                                if (accountantSignGroups != null)
                                {
                                    foreach (AccountantSignGroup accountantSignGroup in accountantSignGroups)
                                    {
                                        accountantSignGroup.ReviewStatus = ReviewStatus.Disabled;
                                        accountantSignGroup.EndDate = DateTime.Now;
                                    }
                                }

                                break;
                            case ReviewStatus.Refuse:
                                accountantSignGroupJournal.DeleteStatus = DeleteStatus.Yes;
                                break;
                        }
                    }
                    else
                    {
                        response.ErrorItem += $"{accountantSignGroupId},";
                    }
                }

                if (response.ErrorItem == null)
                {
                    dbContext.SaveChanges();
                    response.Success();
                }
                else
                {
                    response.ErrorItem = response.ErrorItem.Remove(response.ErrorItem.Length - 1, 1);
                    response.CustomerSealNoData();
                }
                logger.LogInformation("StatusChange output: {@output}", response);
            }
            catch (Exception ex) 
            {
                response.Error();
                logger.LogInformation("StatusChange error: {@error}", ex.Message);
            }
            
            return response;
        }        
    }
}
