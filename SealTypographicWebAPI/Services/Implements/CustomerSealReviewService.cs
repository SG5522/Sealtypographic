using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommonLib.Enums;
using DBEntities;
using DBEntities.Consts;
using DBEntities.Entities.CustomerModels;
using DBEntities.Extensions;
using DBEntities.Utils;
using DJImageLib.Models;
using DJImageLib.Utils;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.CustomerSeal;
using SealTypographicWebAPI.Models.CustomerSealReview;
using SealTypographicWebAPI.Models.LogReport.CustomerSealEventLog;
using SealTypographicWebAPI.Models.LogReport.OperationLog;
using SealTypographicWebAPI.Utils;
using Spire.Xls;
using System.Security.Cryptography.Xml;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 客戶印鑑審核管理
    /// </summary>
    public class CustomerSealReviewService : ICustomerSealReviewService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ImageService imageService;
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private readonly ILogReportService logReportService;        
        private readonly ILogger<CustomerSealReviewService> logger;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="imageService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        /// <param name="logReportService"></param>
        public CustomerSealReviewService(
            SealTypographicDbContext dbContext,
            ImageService imageService,
            IMapper mapper,
            ILogReportService logReportService,
            ILogger<CustomerSealReviewService> logger)
        {
            this.dbContext = dbContext;
            this.imageService = imageService;
            this.mapper = mapper;
            this.logReportService = logReportService;
            configurationProvider = mapper.ConfigurationProvider;
            this.logger = logger;            
        }

        ///<inheritdoc />
        public async Task<CustomerSealReviewPaginate> GetReviewList(CustomerSealSearchReview customerSealSearchReview, TypographyType typographyType, UserInfo userInfo)
        {
            logger.LogInformation("GetReviewList input {@customerSealSearchReview} typographyType: {@TypographyType} userId : {@userId}"
                , customerSealSearchReview, typographyType, userInfo.UserId);

            CustomerSealReviewPaginate customerSealQuarterResponse = new ();
            int companyId = 1;

            try
            {
                IQueryable<CustomerSealGroup> customerSealQuarterQuery = dbContext.CustomerSealGroups
                                                                        .Include(customerSealGroup => customerSealGroup.Customer)
                                                                        .Include(x => x.QuarterYear)
                                                                        .Where
                                                                        (
                                                                            customerSealGroup => customerSealGroup.DeleteStatus == DeleteStatus.No
                                                                            && customerSealGroup.ReviewStatus != ReviewStatus.Draft
                                                                            && customerSealGroup.ReviewStatus < ReviewStatus.Disabled
                                                                            && customerSealGroup.Customer.Company.Id == companyId
                                                                            && customerSealGroup.TypographyType == typographyType
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
                    //取得RsaKey
                    RSAKey rsakey = imageService.GetRsaKey(userInfo.UserId);

                    //取得該頁                   
                    customerSealQuarterResponse.ViewModels = await PageUtil.SetPaginateViewModelAsync<CustomerSealGroup, CustomerSealReviewViewModel>
                                                            (
                                                                customerSealQuarterQuery,                                                                 
                                                                configurationProvider,
                                                                customerSealSearchReview.PageNumber,
                                                                customerSealSearchReview.PageSize
                    );

                    //解密圖片
                    foreach (CustomerSealReviewViewModel viewModel in customerSealQuarterResponse.ViewModels)
                    {
                        imageService.DecryptThumbnailSeals(viewModel.SealImageInfos, userInfo.UserId, false);
                    }

                    PageUtil.SetPaginate(customerSealQuarterResponse, customerSealSearchReview.PageNumber, customerSealSearchReview.PageSize, customerSealQuarterQuery.Count());
                    customerSealQuarterResponse.Success();                    

                    foreach(CustomerSealReviewViewModel customerSealGroupReviewViewModel in customerSealQuarterResponse.ViewModels)
                    {
                        await logReportService.SaveOperationLog(
                                                                    mapper.Map<OperationLogSave>(customerSealGroupReviewViewModel),
                                                                    userInfo.UserName,
                                                                    $"{userInfo.FirstName}{userInfo.LastName}"
                                                                );
                    }
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
        public async Task<CustomerSealDetailReviewResponse> GetReviewDetail(int customerSealQuarterId, UserInfo userInfo)
        {
            logger.LogInformation("GetReviewDetail input customerSealQuarterId: {@customerSealQuarterId} userId: {@userId}", customerSealQuarterId, userInfo.UserId);

            CustomerSealDetailReviewResponse customerSealReviewDetailResponse = new();

            try
            {
                CustomerSealDetailReviewViewModel? customerSealGroupQuery = await dbContext.CustomerSealGroups
                                                                                .Include(x => x.Customer)
                                                                                .Include(x => x.QuarterYear)
                                                                                .Include(x => x.TypographicResources)
                                                                                .Where(x => x.Id == customerSealQuarterId)
                                                                                .ProjectTo<CustomerSealDetailReviewViewModel>(configurationProvider)
                                                                                .FirstOrDefaultAsync();

                if (customerSealGroupQuery != null)
                {
                    imageService.DecryptSeals(customerSealGroupQuery.Seals, userInfo.UserId);

                    customerSealReviewDetailResponse.ViewModel = customerSealGroupQuery;
                    customerSealReviewDetailResponse.Success();
                    await logReportService.SaveOperationLog(
                                                                mapper.Map<OperationLogSave>(customerSealGroupQuery),
                                                                userInfo.UserName,
                                                                $"{userInfo.FirstName}{userInfo.LastName}"
                                                            );
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

        ///<inheritdoc />
        public async Task<ResponseViewModel> StatusChange(List<int> customerSealQuarterIds, ReviewStatus reviewStatus, UserInfo userInfo)
        {
            logger.LogInformation("StatusChange input customerSealQuarterIds {@customerSealQuarterIds} reviewStatus: {@reviewStatus} userId: {@userId}"
                , customerSealQuarterIds, reviewStatus, userInfo.UserId);

            ResponseViewModel response = new();
            
            List<CustomerSealEventLogSave> customerSealEventLogSaves = new ();

            try
            {
                foreach (int customerSealQuarterId in customerSealQuarterIds)
                {
                    CustomerSealGroup? customerSealGroup = dbContext.CustomerSealGroups
                                                            .Include(x => x.Customer)
                                                            .Include(x => x.QuarterYear)
                                                            .FirstOrDefault(x => x.Id == customerSealQuarterId);
                    if (customerSealGroup != null)
                    {
                        reviewStatus.Set(customerSealGroup, userInfo.UserId);
                        customerSealEventLogSaves.Add(mapper.Map<CustomerSealEventLogSave>(customerSealGroup));                        
                    }
                    else
                    {
                        response.ErrorItem += $"{customerSealQuarterId},";
                    }
                }

                if (response.ErrorItem == null)
                {
                    await dbContext.SaveChangesAsync();
                    response.Success();
                    //異動紀錄存檔(審核)
                    foreach (CustomerSealEventLogSave customerSealEventLogSave in customerSealEventLogSaves)
                    {
                        await logReportService.SaveCustomerSealEventLog(
                                                                            customerSealEventLogSave, OperateType.Review,
                                                                            userInfo.UserName,
                                                                            $"{userInfo.FirstName}{userInfo.LastName}"
                                                                        );
                    }
                }
                else
                {
                    response.ErrorItem = response.ErrorItem.Remove(response.ErrorItem.Length - 1, 1);
                    response.CustomerSealNoData();
                }
                logger.LogInformation("StatusChange output {@output} ", response);
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
