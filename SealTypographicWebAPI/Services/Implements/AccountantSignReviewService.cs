using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.AccountantSignReview;
using SealTypographicWebAPI.Utils;
using DBEntities;
using DBEntities.Consts;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Configuration;
using SealTypographicWebAPI.Models.CustomerSealReview;

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

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>    
        public AccountantSignReviewService(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;
        }

        ///<inheritdoc />
        public AccountantSignGroupReviewPaginate GetReviewPaginate(AccountantSignSearchReview accountantSignSearchReview)
        {
            AccountantSignGroupReviewPaginate accountantSignGroupReviewPaginate = new();
            IQueryable<AccountantSignGroup> accountantSignGroupQuery = dbContext.AccountantSignGroups
                                                                        .Include(x => x.Accountant)
                                                                        .ThenInclude(x => x.AccountantGroup)
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
                                            || x.Accountant.AccountantGroup.Name.Contains(accountantSignSearchReview.KeyWord)
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
                                                            .Skip((accountantSignSearchReview.PageNumber - 1) * accountantSignSearchReview.PageSize)
                                                            .Take(accountantSignSearchReview.PageSize)
                                                            .ProjectTo<AccountantSignGroupReviewViewModel>(configurationProvider)
                                                            .ToList();

                accountantSignGroupReviewPaginate.PageNumber = accountantSignSearchReview.PageNumber;
                accountantSignGroupReviewPaginate.PageSize = accountantSignSearchReview.PageSize;
                //計算總頁數
                accountantSignGroupReviewPaginate.TotalPage = TotalPageUtil.GetTotalPage(accountantSignGroupQuery.Count(), accountantSignSearchReview.PageSize);
                accountantSignGroupReviewPaginate.TotalCount = accountantSignGroupQuery.Count();                
            }
            accountantSignGroupReviewPaginate.Success();

            return accountantSignGroupReviewPaginate;
        }

        ///<inheritdoc />
        public AccountantSignGroupDetailReviewResponse GetReviewDetail(int accountantSignGroupId)
        {            
            AccountantSignGroupDetailReviewResponse accountantSignGroupDetailReviewResponse = new();
            AccountantSignGroupDetailReviewViewModel? accountantSignGroupQuery = dbContext.AccountantSignGroups
                                                                                .Include(x => x.Accountant)
                                                                                .ThenInclude(x => x.AccountantGroup)
                                                                                .Include(x => x.TypographicResources)
                                                                                .Where(x => x.Id == accountantSignGroupId)
                                                                                .ProjectTo<AccountantSignGroupDetailReviewViewModel>(configurationProvider)
                                                                                .FirstOrDefault();

            if (accountantSignGroupQuery != null)
            {                
                accountantSignGroupDetailReviewResponse.ViewModel = accountantSignGroupQuery;
                accountantSignGroupDetailReviewResponse.Success();
            }
            
            return accountantSignGroupDetailReviewResponse;            
        }

        ///<inheritdoc />
        public ResponseViewModel Approval(List<int> accountantSignGroupIds)
        {            
            int userId = 0;//之後要調整從驗證帳號中取得ID
            return StatusChange(accountantSignGroupIds, ReviewStatus.Approval, userId);
        }

        ///<inheritdoc />
        public ResponseViewModel Reject(List<int> accountantSignGroupIds)
        {
            int userId = 0;//之後要調整從驗證帳號中取得ID
            return StatusChange(accountantSignGroupIds, ReviewStatus.Reject, userId);
        }

        ///<inheritdoc />
        public ResponseViewModel Refuse(List<int> accountantSignGroupIds)
        {
            int userId = 0;//之後要調整從驗證帳號中取得ID
            return StatusChange(accountantSignGroupIds, ReviewStatus.Refuse, userId);
        }

        /// <summary>
        /// 更換審核狀態
        /// </summary>
        /// <param name="accountantSignGroupIds">會計師簽印群組Id</param>
        /// <param name="reviewStatus">審核狀態</param>
        /// <param name="userId">使用者Id</param>
        /// <returns></returns>
        private ResponseViewModel StatusChange(List<int> accountantSignGroupIds, ReviewStatus reviewStatus, int userId)
        {
            ResponseViewModel response = new();
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
                            if(accountantSignGroups != null)
                            {                                
                                foreach(AccountantSignGroup accountantSignGroup in accountantSignGroups)
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
            return response;
        }        
    }
}
