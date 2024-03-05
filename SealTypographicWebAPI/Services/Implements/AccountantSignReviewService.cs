using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantSignReview;
using SealTypographicWebAPI.Utils;
using DBEntities.Consts;
using AutoMapper.QueryableExtensions;
using DBEntities;
using DBEntities.Entities.AccountantModels;
using CommonLib.Enums;
using SealTypographicWebAPI.Models.LogReport.AccountantSignLog;
using SealTypographicWebAPI.Models.LogReport.OperationLog;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 會計師簽印審核管理    
    /// </summary>
    public class AccountantSignReviewService : IAccountantSignReviewService
    {
        private readonly SealTypographicDbContext dbContext;        
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private readonly ILogger<AccountantSignReviewService> logger;
        private readonly ILogReportService logReportService;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        /// <param name="logReportService"></param>    
        public AccountantSignReviewService(SealTypographicDbContext dbContext, IMapper mapper, ILogger<AccountantSignReviewService> logger, ILogReportService logReportService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;            
            this.logger = logger;
            this.logReportService = logReportService;
        }

        ///<inheritdoc />
        public async Task<AccountantSignGroupReviewPaginate> GetReviewPaginate(AccountantSignSearchReview accountantSignSearchReview, int userId = 1)
        {
            logger.LogInformation("GetReviewPaginate input {@input} userId {@userId}", accountantSignSearchReview, userId);

            AccountantSignGroupReviewPaginate accountantSignGroupReviewPaginate = new();

            try
            {
                IQueryable<AccountantSignGroup> accountantSignGroupQuery = dbContext.AccountantSignGroups
                                                                            .Include(x => x.TypographicResources)
                                                                            .Include(x => x.Accountant)
                                                                            .ThenInclude(x => x.AccountantGroups)
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
                    accountantSignGroupReviewPaginate.ViewModels = await PageUtil.SetPaginateViewModelAsync<AccountantSignGroup, AccountantSignGroupReviewViewModel>
                                                                    (
                                                                        accountantSignGroupQuery,                                                                        
                                                                        configurationProvider,
                                                                        accountantSignSearchReview.PageNumber,
                                                                        accountantSignSearchReview.PageSize
                                                                    );

                    PageUtil.SetPaginate(accountantSignGroupReviewPaginate, accountantSignSearchReview.PageNumber, accountantSignSearchReview.PageSize, accountantSignGroupQuery.Count());
                    accountantSignGroupReviewPaginate.Success();
                    foreach(AccountantSignGroupReviewViewModel accountantSignGroupReviewViewModel in accountantSignGroupReviewPaginate.ViewModels)
                    {                        
                        await logReportService.SaveOperationLog(mapper.Map<OperationLogSave>(accountantSignGroupReviewViewModel));
                    }
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
        public async Task<AccountantSignGroupDetailReviewResponse> GetReviewDetail(int accountantSignGroupId, int userId = 1)
        {
            logger.LogInformation("GetReviewDetail accountantSignGroupId {@accountantSignGroupId} userId {@userId}", accountantSignGroupId, userId);

            AccountantSignGroupDetailReviewResponse accountantSignGroupDetailReviewResponse = new();

            try
            {
                AccountantSignGroupDetailReviewViewModel? accountantSignGroupQuery = await dbContext.AccountantSignGroups
                                                                                    .Include(x => x.Accountant)
                                                                                    .ThenInclude(x => x.AccountantGroups)
                                                                                    .Include(x => x.TypographicResources)
                                                                                    .Where(x => x.Id == accountantSignGroupId)
                                                                                    .ProjectTo<AccountantSignGroupDetailReviewViewModel>(configurationProvider)
                                                                                    .FirstOrDefaultAsync();

                if (accountantSignGroupQuery != null)
                {
                    accountantSignGroupDetailReviewResponse.ViewModel = accountantSignGroupQuery;
                    accountantSignGroupDetailReviewResponse.Success();
                    await logReportService.SaveOperationLog(mapper.Map<OperationLogSave>(accountantSignGroupQuery));
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
        public async Task<ResponseViewModel> StatusChange(List<int> accountantSignGroupIds, ReviewStatus reviewStatus, int userId = 1)
        {
            logger.LogInformation("StatusChange accountantSignGroupIds: {@accountantSignGroupIds}, reviewStatus: {@reviewStatus}, userId: {@userId} "
                                    , accountantSignGroupIds, reviewStatus, userId);

            List<AccountantSignEventLogSave> accountantSignEventLogSaves = new ();

            ResponseViewModel response = new();

            try
            {
                
                foreach (int accountantSignGroupId in accountantSignGroupIds)
                {
                    AccountantSignGroup? accountantSignGroupQuery = dbContext.AccountantSignGroups.Include(x => x.Accountant)
                                                                                .FirstOrDefault(x => x.Id == accountantSignGroupId);
                    if (accountantSignGroupQuery != null)
                    {
                        accountantSignGroupQuery.ReviewUserId = userId;
                        accountantSignGroupQuery.ReviewStatus = reviewStatus;
                        accountantSignGroupQuery.ReviewDate = DateTime.Now;
                        switch (reviewStatus)
                        {
                            case ReviewStatus.Approval:
                                //變更啟用與結束日期
                                accountantSignGroupQuery.StartDate = DateTime.Now;
                                accountantSignGroupQuery.EndDate = DateTime.Parse("9999/12/31");
                                //找出審核通過的簽印組
                                IQueryable<AccountantSignGroup>? accountantSignGroups = dbContext.AccountantSignGroups
                                                                                       .Where
                                                                                       (
                                                                                           x => x.Accountant.Id == accountantSignGroupQuery.Accountant.Id
                                                                                           && x.Id != accountantSignGroupId
                                                                                           && x.ReviewStatus == ReviewStatus.Approval
                                                                                       );
                                //如有審核通過的簽印組則停用(會計師簽印只能有一組是通過的)
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
                                accountantSignGroupQuery.DeleteStatus = DeleteStatus.Yes;
                                break;
                        }
                        //加入異動紀錄
                        accountantSignEventLogSaves.Add(mapper.Map<AccountantSignEventLogSave>(accountantSignGroupQuery));                        
                    }
                    else
                    {
                        response.ErrorItem += $"{accountantSignGroupId},";
                    }
                }

                if (response.ErrorItem == null)
                {
                    await dbContext.SaveChangesAsync();
                    response.Success();
                    //異動紀錄存檔(審核)
                    foreach(AccountantSignEventLogSave accountantSignEventLogSave in accountantSignEventLogSaves)
                    {
                        await logReportService.SaveAccountantSignEventLog(accountantSignEventLogSave, OperateType.Review);
                    }                    
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
