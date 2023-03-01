using AutoMapper;
using Microsoft.EntityFrameworkCore;
using DBEntities.Consts;
using DBEntities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.CustomerSealReview;
using SealTypographicWebAPI.Utils;
using System.Linq;

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

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="imageService"></param>
        /// <param name="mapper"></param>        
        public CustomerSealReviewService(SealTypographicDbContext dbContext, ImageService imageService, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.imageService = imageService;
            this.mapper = mapper;
        }

        ///<inheritdoc />
        public CustomerSealQuarterReviewPaginate GetReviewList(CustomerSealSearchReview customerSealSearchReview)
        {
            CustomerSealQuarterReviewPaginate customerSealQuarterResponse = new ();           

            IQueryable<CustomerSealQuarterJournal> customerSealQuarterQuery = dbContext.CustomerSealQuarterJournals
                                                                            .Include(customerSealQuarterJournal => customerSealQuarterJournal.Customer)
                                                                            .Where
                                                                            (
                                                                                customerSealJournal => customerSealJournal.DeleteStatus == DeleteStatus.No
                                                                                && customerSealJournal.ReviewStatus < ReviewStatus.Disabled
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
                customerSealQuarterQuery = customerSealQuarterQuery.Where(customerSealJournal => customerSealJournal.ReviewStatus == customerSealSearchReview.ReviewStatus);
            }

            if (customerSealQuarterQuery.Any())
            {
                //取得該頁            
                List<CustomerSealQuarterJournal> thisPageCustomerSealQuarter = customerSealQuarterQuery
                                                    .Include(x => x.CustomerSealJournals)
                                                    .Skip((customerSealSearchReview.PageNumber - 1) * customerSealSearchReview.PageSize)
                                                    .Take(customerSealSearchReview.PageSize)
                                                    .ToList();

                foreach (CustomerSealQuarterJournal customerSealQuarterJournal in thisPageCustomerSealQuarter)
                {
                    CustomerSealQuarterReviewViewModel customerSealQuarterReviewViewModel = mapper.Map<CustomerSealQuarterReviewViewModel>(customerSealQuarterJournal);

                    foreach (CustomerSealJournal customerSeal in FilterCustomerSeals(customerSealQuarterJournal.CustomerSealJournals))
                    {
                        SealImageInfo sealImageInfo = new()
                        {
                            SealMappingConfigId = customerSeal.ConfigType,
                            Sequence = customerSeal.Sequence,
                            ThumbnailBase64 = imageService.GetPathToBase64(customerSeal.ThumbnailFullPath)
                        };
                        customerSealQuarterReviewViewModel.SealImageInfos.Add(sealImageInfo);
                    }                    
                    customerSealQuarterResponse.ViewModels.Add(customerSealQuarterReviewViewModel);
                }
                
                customerSealQuarterResponse.PageNumber = customerSealSearchReview.PageNumber;
                customerSealQuarterResponse.PageSize = customerSealSearchReview.PageSize;
                //計算總頁數
                customerSealQuarterResponse.TotalPage = TotalPageUtil.GetTotalPage(customerSealQuarterQuery.Count(), customerSealSearchReview.PageSize);
                customerSealQuarterResponse.TotalCount = customerSealQuarterQuery.Count();
                customerSealQuarterResponse.Success();
            }
            else
            {
                customerSealQuarterResponse.CustomerSealNoData();
            }

            return customerSealQuarterResponse;
        }
        
        ///<inheritdoc />
        public CustomerSealQuarterDetailReviewResponse GetReviewDetail(int customerSealQuarterId)
        {
            CustomerSealQuarterDetailReviewResponse customerSealReviewDetailResponse = new();
                        
            CustomerSealQuarterJournal? customerSealQuarterQuery = dbContext.CustomerSealQuarterJournals
                                                                   .Include(x => x.Customer)
                                                                   .Include(x => x.CustomerSealJournals)
                                                                   .FirstOrDefault(x => x.Id == customerSealQuarterId);
            if (customerSealQuarterQuery != null)
            {
                customerSealReviewDetailResponse.ViewModel = mapper.Map<CustomerSealQuarterDetailReviewViewModel>(customerSealQuarterQuery.Customer);

                customerSealReviewDetailResponse.ViewModel.Id = customerSealQuarterId;
                customerSealReviewDetailResponse.ViewModel.Quarter = customerSealQuarterQuery.Quarter;

                foreach (CustomerSealJournal customerSealJournal in FilterCustomerSeals(customerSealQuarterQuery.CustomerSealJournals))
                {
                    CustomerSealViewModel customerSealViewModel = mapper.Map<CustomerSealViewModel>(customerSealJournal);
                    customerSealViewModel.ImageBase64 = imageService.GetPathToBase64(customerSealJournal.ImageFullPath); //資料庫取得圖檔路徑轉BASE64                                       
                    customerSealViewModel.SealMappingConfigId = customerSealJournal.ConfigType;
                    customerSealReviewDetailResponse.ViewModel.Seals.Add(customerSealViewModel);
                }
                customerSealReviewDetailResponse.Success();
            }
            else
            {
                customerSealReviewDetailResponse.CustomerSealNoData();
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
        /// 過濾客戶印鑑
        /// 過濾標記刪除
        /// 以及排序簽印
        /// </summary>
        /// <param name="customerSealJournal"></param>
        private List<CustomerSealJournal> FilterCustomerSeals(List<CustomerSealJournal> customerSealJournal)
        {
            List<CustomerSealJournal> customerSeals = customerSealJournal
                                                    .Where(x => x.DeleteStatus == DeleteStatus.No)
                                                    .OrderBy(x => x.ConfigType)
                                                    .ThenBy(x => x.Sequence)
                                                    .ToList();
            return customerSeals;
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
                CustomerSealQuarterJournal? customerSealQuarterJournal = dbContext.CustomerSealQuarterJournals.Find(customerSealQuarterId);
                if (customerSealQuarterJournal != null)
                {
                    customerSealQuarterJournal.ReviewUserId = userId;
                    customerSealQuarterJournal.ReviewStatus = reviewStatus;
                    customerSealQuarterJournal.ReviewDate = DateTime.Now;
                    if(reviewStatus == ReviewStatus.Approval)
                    {
                        customerSealQuarterJournal.StartDate = DateTime.Now;
                        customerSealQuarterJournal.EndDate = DateTime.Parse("9999/12/31");
                    }
                    if(reviewStatus == ReviewStatus.Refuse)
                    {
                        customerSealQuarterJournal.DeleteStatus = DeleteStatus.Yes;
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
