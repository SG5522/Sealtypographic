using AutoMapper;
using DBEntitiesExtension;
using DBEntitiesExtension.Consts;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.CustomerSealReview;
using SealTypographicWebAPI.Utils;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 客戶印鑑審核管理
    /// </summary>
    public class CustomerSealReviewServiceExtension : ICustomerSealReviewService
    {
        private readonly SealTypographicExtensionDbContext dbContext;
        private readonly ImageService imageService;
        private readonly IMapper mapper;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="imageService"></param>
        /// <param name="mapper"></param>        
        public CustomerSealReviewServiceExtension(SealTypographicExtensionDbContext dbContext, ImageService imageService, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.imageService = imageService;
            this.mapper = mapper;
        }

        ///<inheritdoc />
        public CustomerSealQuarterReviewPaginate GetReviewList(CustomerSealSearchReview customerSealSearchReview)
        {
            CustomerSealQuarterReviewPaginate customerSealQuarterResponse = new ();           

            IQueryable<CustomerSealGroup> customerSealQuarterQuery = dbContext.CustomerSealGroups
                                                                    .Include(custoomerSealGroup => custoomerSealGroup.Customer)
                                                                    .Where
                                                                    (
                                                                        custoomerSealGroup => custoomerSealGroup.DeleteStatus == DeleteStatus.No
                                                                        && custoomerSealGroup.ReviewStatus < ReviewStatus.Disabled
                                                                    );

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
                customerSealQuarterQuery = customerSealQuarterQuery.Where(customerSealJournal => customerSealJournal.ReviewStatus == (ReviewStatus)customerSealSearchReview.ReviewStatus);
            }

            if (customerSealQuarterQuery.Any())
            {
                //取得該頁            
                List<CustomerSealGroup> thisPageCustomerSealGroup = customerSealQuarterQuery
                                                                    .Include(x => x.TypographicResources)
                                                                    .Include(x => x.Quarter)
                                                                    .Skip((customerSealSearchReview.PageNumber - 1) * customerSealSearchReview.PageSize)
                                                                    .Take(customerSealSearchReview.PageSize)                                                                    
                                                                    .ToList();


                foreach (CustomerSealGroup customerSealGroup in thisPageCustomerSealGroup)
                {
                    CustomerSealQuarterReviewViewModel customerSealQuarterReviewViewModel = mapper.Map<CustomerSealQuarterReviewViewModel>(customerSealGroup);
                    customerSealQuarterReviewViewModel.Quarter = QuarterUtil.GetTaiwanYearQuarter(customerSealGroup.Quarter);

                    TypographicResourceUtil.FilterDeleteResource(customerSealGroup.TypographicResources);

                    foreach (TypographicResource typographyResource in customerSealGroup.TypographicResources)
                    {
                        SealImageInfo sealImageInfo = new()
                        {
                            SealMappingConfigId = (DBEntities.Consts.CustomerSealType)SealMappingConfigUtil.GetCustomerSealType(typographyResource.SubSealType),
                            Sequence = typographyResource.Sequence,                            
                        };
                        if(typographyResource.ThumbnailFullPath != null)
                        {
                            sealImageInfo.ThumbnailBase64 = imageService.GetPathToBase64(typographyResource.ThumbnailFullPath);
                        }

                        customerSealQuarterReviewViewModel.SealImageInfos.Add(sealImageInfo);
                    }                    
                    customerSealQuarterResponse.ViewModels.Add(customerSealQuarterReviewViewModel);
                }
                
                customerSealQuarterResponse.PageNumber = customerSealSearchReview.PageNumber;
                customerSealQuarterResponse.PageSize = customerSealSearchReview.PageSize;
                //計算總頁數
                customerSealQuarterResponse.TotalPage = TotalPageUtil.GetTotalPage(customerSealQuarterQuery.Count(), customerSealSearchReview.PageSize);
                customerSealQuarterResponse.TotalCount = customerSealQuarterQuery.Count();
            }
            customerSealQuarterResponse.Success();

            return customerSealQuarterResponse;
        }
        
        ///<inheritdoc />
        public CustomerSealQuarterDetailReviewResponse GetReviewDetail(int customerSealQuarterId)
        {
            CustomerSealQuarterDetailReviewResponse customerSealReviewDetailResponse = new();
                        
            CustomerSealGroup? customerSealGroupQuery = dbContext.CustomerSealGroups
                                                        .Include(x => x.Customer)
                                                        .Include(x => x.Quarter)
                                                        .Include(x => x.TypographicResources)
                                                        .FirstOrDefault(x => x.Id == customerSealQuarterId);
            if (customerSealGroupQuery != null)
            {
                customerSealReviewDetailResponse.ViewModel = mapper.Map<CustomerSealQuarterDetailReviewViewModel>(customerSealGroupQuery.Customer);

                customerSealReviewDetailResponse.ViewModel.Id = customerSealQuarterId;
                customerSealReviewDetailResponse.ViewModel.Quarter = QuarterUtil.GetTaiwanYearQuarter(customerSealGroupQuery.Quarter);

                TypographicResourceUtil.FilterDeleteResource(customerSealGroupQuery.TypographicResources);

                foreach (TypographicResource typographicResource in FilterCustomerSeals(customerSealGroupQuery.TypographicResources))
                {
                    CustomerSealViewModel customerSealViewModel = mapper.Map<CustomerSealViewModel>(typographicResource);
                    customerSealViewModel.ImageBase64 = imageService.GetPathToBase64(typographicResource.ImageFullPath); //資料庫取得圖檔路徑轉BASE64                                       
                    customerSealViewModel.SealMappingConfigId = (DBEntities.Consts.CustomerSealType)SealMappingConfigUtil.GetCustomerSealType(typographicResource.SubSealType);
                    customerSealReviewDetailResponse.ViewModel.Seals.Add(customerSealViewModel);
                }                
            }
            customerSealReviewDetailResponse.Success();
        
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
        /// 過濾客戶印鑑
        /// 過濾標記刪除
        /// 以及排序簽印
        /// </summary>
        /// <param name="typographicResource"></param>
        private List<TypographicResource> FilterCustomerSeals(List<TypographicResource> typographicResource)
        {
            List<TypographicResource> typographicQuery = typographicResource
                                                    .Where(x => x.DeleteStatus == DeleteStatus.No)
                                                    .OrderBy(x => x.SubSealType)
                                                    .ThenBy(x => x.Sequence)
                                                    .ToList();
            return typographicQuery;
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
