using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
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

        /// <summary>
        /// 待審清單
        /// </summary>
        /// <returns></returns>
        public CustomerSealQuarterResponse GetReviewQuarterSeals(CustomerSealSearchReview customerSealSearchReview)
        {
            CustomerSealQuarterResponse customerSealQuarterResponse = new ();           

            IQueryable<CustomerSealQuarterJournal> customerSealQuarterJournalQuery = dbContext.CustomerSealQuarterJournals
                                                                .Include(customerSealQuarterJournal => customerSealQuarterJournal.Customer)
                                                                .Where
                                                                (
                                                                    customerSealJournal => customerSealJournal.DeleteStatus == DeleteStatus.No
                                                                    && customerSealJournal.ReviewStatus == customerSealSearchReview.ReviewStatus
                                                                );
            if (customerSealQuarterJournalQuery.Any())
            {
                //取得該頁            
                List<CustomerSealQuarterJournal> thisPageCustomerSealQuarter = customerSealQuarterJournalQuery
                                                    .Skip((customerSealSearchReview.PageNumber - 1) * customerSealSearchReview.PageSize)
                                                    .Take(customerSealSearchReview.PageSize)
                                                    .ToList();
                foreach (CustomerSealQuarterJournal customerSealQuarterJournal in thisPageCustomerSealQuarter)
                {
                    CustomerSealQuarterViewModel customerSealQuarterViewModel = mapper.Map<CustomerSealQuarterViewModel>(customerSealQuarterJournal);
                    List<CustomerSealJournal> customerSeals = dbContext.CustomerSealJournals.Where
                                                                (
                                                                    x => x.CustomerSealQuarterJournal.Id == customerSealQuarterJournal.Id
                                                                    && x.DeleteStatus == DeleteStatus.No                                                                    
                                                                )
                                                                .OrderBy(x => x.ConfigType)
                                                                .ThenBy(x => x.Sequence)
                                                                .ToList();
                    foreach (CustomerSealJournal customerSeal in customerSeals)
                    {
                        SealImageInfo sealImageInfo = new()
                        {
                            CustomerSealType = customerSeal.ConfigType,
                            Sequence = customerSeal.Sequence,
                            ThumbnailBase64 = imageService.GetPathToBase64(customerSeal.ThumbnailFullPath)
                        };
                        customerSealQuarterViewModel.SealImageInfos.Add(sealImageInfo);
                    }
                    
                    customerSealQuarterResponse.ViewModels.Add(customerSealQuarterViewModel);
                }
                
                customerSealQuarterResponse.PageNumber = customerSealSearchReview.PageNumber;
                customerSealQuarterResponse.PageSize = customerSealSearchReview.PageSize;
                //計算總頁數
                customerSealQuarterResponse.TotalPage = TotalPageUtil.GetTotalPage(customerSealQuarterJournalQuery.Count(), customerSealSearchReview.PageSize);
                customerSealQuarterResponse.TotalCount = customerSealQuarterJournalQuery.Count();
                customerSealQuarterResponse.Success();
            }
            else
            {
                customerSealQuarterResponse.CustomerSealNoData();
            }

            return customerSealQuarterResponse;
        }
        
        ///<inheritdoc />
        public CustomerSealReviewDetailResponse GetCustomerSealReviewDetail(int CustomerSealQuarterId)
        {
            CustomerSealReviewDetailResponse customerSealReviewDetailResponse = new();
            
            
            CustomerSealQuarterJournal? customerQuery = dbContext.CustomerSealQuarterJournals.Find(CustomerSealQuarterId);
            //if(customerQuery != null) 
            //{
            //    IQueryable<CustomerSealJournal> customerSealJournalQuery = dbContext.CustomerSealJournals                                                                            
            //                                                                .Where
            //                                                                (
            //                                                                    customerSealJournal => customerSealJournal.CustomerId == customerSealReviewSearch.CustomerId
            //                                                                    //&& customerSealJournal.Quarter == customerSealQuarter.Quarter
            //                                                                )
            //                                                                .OrderBy(customerSealJournal => customerSealJournal.ConfigType)
            //                                                                .ThenBy(customerSealJournal => customerSealJournal.SealReviewJournal.Sequence);
            //    if(customerSealJournalQuery.Any())
            //    {                    
            //        foreach (CustomerSealJournal customerSealJournal in customerSealJournalQuery)
            //        {
            //            CustomerSealViewModel customerSealViewModel = mapper.Map<CustomerSealViewModel>(customerSealJournal);
            //            customerSealViewModel.ImageBase64 = customerSealJournal.ImagePath;
            //            customerSealViewModels.Add(customerSealViewModel);
            //        }
            //        customerSealReviewDetailResponse.CustomerSealReviewDetail = mapper.Map<CustomerSealReviewDetail>(customerQuery);
            //        customerSealReviewDetailResponse.CustomerSealReviewDetail.CustomerSealViewModels = customerSealViewModels;                    
            //        customerSealReviewDetailResponse.CustomerSealReviewDetail.Quarter = customerSealReviewSearch.Quarter;
            //        customerSealReviewDetailResponse.Success();
            //    }
            //    else
            //    {
            //        customerSealReviewDetailResponse.CustomerSealNoData();
            //    }
            //}
            //else
            //{
            //    customerSealReviewDetailResponse.CustomeNoData();
            //}            
            return customerSealReviewDetailResponse;
        }

        /// <summary>
        /// 審核通過
        /// </summary>
        /// <param name="customerSealIds"></param>
        public ResponseViewModel ReviewApproval(List<int> customerSealIds)
        {              
            return ReviewStatusChange(customerSealIds, ReviewStatus.Approval);
        }

        /// <summary>
        /// 審核退件
        /// </summary>
        /// <param name="customerSealIds"></param>
        public ResponseViewModel ReviewReject(List<int> customerSealIds)
        {
            return ReviewStatusChange(customerSealIds, ReviewStatus.Reject);
        }

        /// <summary>
        /// 更換審核狀態
        /// </summary>
        /// <param name="customerSealIds">審核印鑑(Id)</param>
        /// <param name="reviewStatus">審核狀態</param>
        /// <returns></returns>

        private ResponseViewModel ReviewStatusChange(List<int> customerSealIds, ReviewStatus reviewStatus)
        {
            ResponseViewModel response = new();
            foreach (int customerSealJournalId in customerSealIds)
            {
                CustomerSealJournal? customerSealJournal = dbContext.CustomerSealJournals.Find(customerSealJournalId);
                if (customerSealJournal != null)
                {
                    //customerSealJournal.ReviewUserId = 1; //之後要調整從驗證帳號中取得ID
                    //customerSealJournal.ReviewStatus = reviewStatus;
                    //customerSealJournal.ReviewDate = DateTime.Now;
                }
                else
                {
                    response.CustomerSealNoData();
                    return response;
                }
            }
            dbContext.SaveChanges();
            response.Success();
            return response;
        }
    }
}
