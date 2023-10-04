using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBEntities;
using DBEntities.Consts;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.CustomerSealReview;
using SealTypographicWebAPI.Utils;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 客戶印鑑審核管理
    /// </summary>
    public class CustomerSealReviewService : ICustomerSealReviewService
    {
        private readonly SealTypographicDbContext dbContext;        
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>        
        public CustomerSealReviewService(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;            
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;
        }

        ///<inheritdoc />
        public CustomerSealQuarterReviewPaginate GetReviewList(CustomerSealSearchReview customerSealSearchReview)
        {
            CustomerSealQuarterReviewPaginate customerSealQuarterResponse = new ();
            int companyId = 1;

            IQueryable<CustomerSealGroup> customerSealQuarterQuery = dbContext.CustomerSealGroups
                                                                    .Include(customerSealGroup => customerSealGroup.Customer)
                                                                    .Include(x => x.QuarterYear)
                                                                    .Where
                                                                    (
                                                                        customerSealGroup => customerSealGroup.DeleteStatus == DeleteStatus.No
                                                                        && customerSealGroup.ReviewStatus < ReviewStatus.Disabled
                                                                        && customerSealGroup.Customer.Company.Id == companyId      
                                                                    ).OrderByDescending(x => x.QuarterYear.Id);
                            

            if (!string.IsNullOrWhiteSpace(customerSealSearchReview.KeyWord))
            {
                customerSealQuarterQuery = customerSealQuarterQuery.Where
                    (
                        x => 
                        x.Customer.Code.ToLower().Contains(customerSealSearchReview.KeyWord.ToLower())
                        || x.Customer.Name.Contains(customerSealSearchReview.KeyWord)
                    );
            }

            if (customerSealSearchReview.ReviewStatus != null)
            {
                customerSealQuarterQuery = customerSealQuarterQuery.Where(customerSealJournal => customerSealJournal.ReviewStatus == customerSealSearchReview.ReviewStatus);
            }

            if (customerSealQuarterQuery.Any())
            {
                //取得該頁                   
                customerSealQuarterResponse.ViewModels = customerSealQuarterQuery
                                                        .Include(x => x.TypographicResources)
                                                        .Skip((customerSealSearchReview.PageNumber - 1) * customerSealSearchReview.PageSize)
                                                        .Take(customerSealSearchReview.PageSize)
                                                        .ProjectTo<CustomerSealQuarterReviewViewModel>(configurationProvider)
                                                        .ToList();
                //計算總頁數
                int totalPage = customerSealQuarterQuery.Count();
                customerSealQuarterResponse.TotalPage = TotalPageUtil.GetTotalPage(totalPage, customerSealSearchReview.PageSize);
                customerSealQuarterResponse.TotalCount = totalPage;
                customerSealQuarterResponse.PageNumber = customerSealSearchReview.PageNumber;
                customerSealQuarterResponse.PageSize = customerSealSearchReview.PageSize;
            }
            customerSealQuarterResponse.Success();

            return customerSealQuarterResponse;
        }
        
        ///<inheritdoc />
        public CustomerSealQuarterDetailReviewResponse GetReviewDetail(int customerSealQuarterId)
        {
            CustomerSealQuarterDetailReviewResponse customerSealReviewDetailResponse = new();
            CustomerSealQuarterDetailReviewViewModel? customerSealGroupQuery = dbContext.CustomerSealGroups
                                                                                .Include(x => x.Customer)
                                                                                .Include(x => x.QuarterYear)
                                                                                .Include(x => x.TypographicResources)
                                                                                .Where(x => x.Id == customerSealQuarterId)
                                                                                .ProjectTo<CustomerSealQuarterDetailReviewViewModel>(configurationProvider)
                                                                                .FirstOrDefault();

            if (customerSealGroupQuery != null)
            {
                customerSealReviewDetailResponse.ViewModel = customerSealGroupQuery;
                customerSealReviewDetailResponse.Success();
            }            
        
            return customerSealReviewDetailResponse;
        }

        ///<inheritdoc />
        public ResponseViewModel Approval(List<int> customerSealQuarterIds)
        {
            int userId = 0;//之後要調整從驗證帳號中取得ID
            return StatusChange(customerSealQuarterIds, ReviewStatus.Approval, userId);
        }

        ///<inheritdoc />
        public ResponseViewModel Reject(List<int> customerSealQuarterIds)
        {
            int userId = 0;//之後要調整從驗證帳號中取得ID
            return StatusChange(customerSealQuarterIds, ReviewStatus.Reject, userId);
        }

        ///<inheritdoc />
        public ResponseViewModel Refuse(List<int> customerSealQuarterIds)
        {
            int userId = 0;//之後要調整從驗證帳號中取得ID
            return StatusChange(customerSealQuarterIds, ReviewStatus.Refuse, userId);
        }

        /// <summary>
        /// 更換審核狀態
        /// </summary>
        /// <param name="customerSealQuarterIds">審核季度Id</param>
        /// <param name="reviewStatus">審核狀態</param>
        /// <param name="userId">使用者Id</param>
        /// <returns></returns>
        private ResponseViewModel StatusChange(List<int> customerSealQuarterIds, ReviewStatus reviewStatus, int userId)
        {
            ResponseViewModel response = new();
            foreach (int customerSealQuarterId in customerSealQuarterIds)
            {
                CustomerSealGroup? customerSealGroup = dbContext.CustomerSealGroups.Find(customerSealQuarterId);
                if (customerSealGroup != null)
                {
                    customerSealGroup.ReviewUserId = userId;
                    customerSealGroup.ReviewStatus = reviewStatus;
                    customerSealGroup.ReviewDate = DateTime.Now;
                    if(reviewStatus == ReviewStatus.Approval)
                    {
                        customerSealGroup.StartDate = DateTime.Now;
                        customerSealGroup.EndDate = DateTime.Parse("9999/12/31");
                    }
                    if(reviewStatus == ReviewStatus.Refuse)
                    {
                        customerSealGroup.DeleteStatus = DeleteStatus.Yes;
                    }
                }
                else
                {
                    response.ErrorItem += $"{customerSealQuarterId},";                        
                }
            }

            if(response.ErrorItem == null)
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
