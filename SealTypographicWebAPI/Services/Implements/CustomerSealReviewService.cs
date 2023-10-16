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
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private readonly ILogger<CustomerSealReviewService> logger;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        /// <param name="logger"></param>        
        public CustomerSealReviewService(SealTypographicDbContext dbContext, IMapper mapper, ILogger<CustomerSealReviewService> logger)
        {
            this.dbContext = dbContext;                        
            configurationProvider = mapper.ConfigurationProvider;
            this.logger = logger;
        }

        ///<inheritdoc />
        public CustomerSealGroupReviewPaginate GetReviewList(CustomerSealSearchReview customerSealSearchReview, TypographyType typographyType)
        {
            logger.LogInformation("GetReviewList input {@Input} typographyType= {@TypographyType}", customerSealSearchReview, typographyType);

            CustomerSealGroupReviewPaginate customerSealQuarterResponse = new ();
            int companyId = 1;

            try
            {
                IQueryable<CustomerSealGroup> customerSealQuarterQuery = dbContext.CustomerSealGroups
                                                                    .Include(customerSealGroup => customerSealGroup.Customer)
                                                                    .Include(x => x.QuarterYear)
                                                                    .Where
                                                                    (
                                                                        customerSealGroup => customerSealGroup.DeleteStatus == DeleteStatus.No
                                                                        && customerSealGroup.ReviewStatus < ReviewStatus.Disabled
                                                                        && customerSealGroup.TypographyType == typographyType
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
                                                            .ProjectTo<CustomerSealGroupReviewViewModel>(configurationProvider)
                                                            .ToList();
                    //計算總頁數
                    int totalPage = customerSealQuarterQuery.Count();
                    customerSealQuarterResponse.TotalPage = TotalPageUtil.GetTotalPage(totalPage, customerSealSearchReview.PageSize);
                    customerSealQuarterResponse.TotalCount = totalPage;
                    customerSealQuarterResponse.PageNumber = customerSealSearchReview.PageNumber;
                    customerSealQuarterResponse.PageSize = customerSealSearchReview.PageSize;
                    customerSealQuarterResponse.Success();
                }
                else
                {
                    customerSealQuarterResponse.DbNoData();
                }                
                logger.LogInformation("GetReviewList output {@Output}", customerSealQuarterResponse);
            }
            catch (Exception ex) 
            {
                customerSealQuarterResponse.Error();
                logger.LogInformation("GetReviewList error {@Error}", ex.Message);
            }

            return customerSealQuarterResponse;
        }
        
        ///<inheritdoc />
        public CustomerSealGroupDetailReviewResponse GetReviewDetail(int customerSealQuarterId)
        {
            logger.LogInformation("GetReviewDetail customerSealQuarterId {@CustomerSealQuarterId}", customerSealQuarterId);

            CustomerSealGroupDetailReviewResponse customerSealReviewDetailResponse = new();

            try
            {
                CustomerSealGroupDetailReviewViewModel? customerSealGroupQuery = dbContext.CustomerSealGroups
                                                                    .Include(x => x.Customer)
                                                                    .Include(x => x.QuarterYear)
                                                                    .Include(x => x.TypographicResources)
                                                                    .Where(x => x.Id == customerSealQuarterId)
                                                                    .ProjectTo<CustomerSealGroupDetailReviewViewModel>(configurationProvider)
                                                                    .FirstOrDefault();

                if (customerSealGroupQuery != null)
                {
                    customerSealReviewDetailResponse.ViewModel = customerSealGroupQuery;
                    customerSealReviewDetailResponse.Success();
                }
                else
                {
                    customerSealReviewDetailResponse.DbNoData();
                }
                logger.LogInformation("GetReviewDetail output {@Output}", customerSealReviewDetailResponse);
            }
            catch (Exception ex)
            {
                customerSealReviewDetailResponse.Error();
                logger.LogError("GetReviewDetail error {@Error}",ex.Message);
            }

            return customerSealReviewDetailResponse;
        }        

        /// <summary>
        /// 更換審核狀態
        /// </summary>
        /// <param name="customerSealQuarterIds">審核季度Id</param>
        /// <param name="reviewStatus">審核狀態</param>
        /// <param name="userId">使用者Id(從Keycolak取得)</param>
        /// <returns></returns>
        public ResponseViewModel StatusChange(List<int> customerSealQuarterIds, ReviewStatus reviewStatus, int userId)
        {
            logger.LogInformation("StatusChange customerSealQuarterIds {@CustomerSealQuarterIds} reviewStatus= {@ReviewStatus} userId= {@UserId}", customerSealQuarterIds, reviewStatus, userId);

            ResponseViewModel response = new();

            try
            {
                foreach (int customerSealQuarterId in customerSealQuarterIds)
                {
                    CustomerSealGroup? customerSealGroup = dbContext.CustomerSealGroups.Find(customerSealQuarterId);
                    if (customerSealGroup != null)
                    {
                        customerSealGroup.ReviewUserId = userId;
                        customerSealGroup.ReviewStatus = reviewStatus;
                        customerSealGroup.ReviewDate = DateTime.Now;
                        if (reviewStatus == ReviewStatus.Approval)
                        {
                            customerSealGroup.StartDate = DateTime.Now;
                            customerSealGroup.EndDate = DateTime.Parse("9999/12/31");
                        }
                        if (reviewStatus == ReviewStatus.Refuse)
                        {
                            customerSealGroup.DeleteStatus = DeleteStatus.Yes;
                        }
                    }
                    else
                    {
                        response.ErrorItem += $"{customerSealQuarterId},";
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
                logger.LogInformation("StatusChange output {@Output} ", response);
            }
            catch (Exception ex) 
            {
                response.Error();
                logger.LogInformation("StatusChange error {@Error} ", ex.Message);
            }
            
            return response;
        }

    }
}
