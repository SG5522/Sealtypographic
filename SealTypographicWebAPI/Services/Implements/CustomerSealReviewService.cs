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
        private readonly IMapper mapper;

        /// <summary>
        /// 注入DB、ResponseService
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>        
        public CustomerSealReviewService(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <summary>
        /// 待審清單
        /// </summary>
        /// <returns></returns>
        public CustomerSealQuarterViewModelResponse GetCustomerSealReviewViewModel(CustomerSealSearchReview customerSealReviewSearch)
        {
            CustomerSealQuarterViewModelResponse customerSealReviewViewModelResponse = new ();
            List<CustomerSealQuarterViewModel> customerSealReviewViewModels = new ();

            IQueryable<CustomerSealQuarterJournal> customerSealQuarterJournalQuery = dbContext.CustomerSealQuarterJournals
                                                                .Include(customerSealQuarterJournal => customerSealQuarterJournal.Customer)
                                                                .Where
                                                                (
                                                                    customerSealJournal => customerSealJournal.DeleteStatus == DeleteStatus.No
                                                                    && customerSealJournal.ReviewStatus == customerSealReviewSearch.ReviewStatus
                                                                );
            if (customerSealQuarterJournalQuery.Any())
            {
                //取得該頁            
                List<CustomerSealQuarterJournal> thisPageCustomerSealQuarter = customerSealQuarterJournalQuery
                                                    .Skip((customerSealReviewSearch.PageNumber - 1) * customerSealReviewSearch.PageSize)
                                                    .Take(customerSealReviewSearch.PageSize)
                                                    .ToList();
                foreach (CustomerSealQuarterJournal customerSealQuarterJournal in thisPageCustomerSealQuarter)
                {
                    CustomerSealQuarterViewModel customerSealReviewViewModel = mapper.Map<CustomerSealQuarterViewModel>(customerSealQuarterJournal);
                    //customerSealReviewViewModel.ReviewStatus = ReviewStatusUtil.Get(customerSealJournal.ReviewStatus);
                    customerSealReviewViewModels.Add(customerSealReviewViewModel);
                }

                customerSealReviewViewModelResponse.CustomerSealReviewViewModels = customerSealReviewViewModels;
                customerSealReviewViewModelResponse.PageNumber = customerSealReviewSearch.PageNumber;
                customerSealReviewViewModelResponse.PageSize = customerSealReviewSearch.PageSize;
                //計算總頁數
                customerSealReviewViewModelResponse.TotalPage = TotalPageUtil.GetTotalPage(customerSealQuarterJournalQuery.Count(), customerSealReviewSearch.PageSize);
                customerSealReviewViewModelResponse.TotalCount = customerSealQuarterJournalQuery.Count();
                customerSealReviewViewModelResponse.Success();
            }
            else
            {
                customerSealReviewViewModelResponse.CustomerSealNoData();
            }

            return customerSealReviewViewModelResponse;
        }
        /// <summary>
        /// 依照客戶ID與季度列出詳細資料與印鑑
        /// </summary>
        /// <param name="customerSealReviewSearch">搜尋條件</param>
        /// <returns></returns>

        public CustomerSealReviewDetailResponse GetCustomerSealReviewDetail(CustomerSealSearchReview customerSealReviewSearch)
        {
            CustomerSealReviewDetailResponse customerSealReviewDetailResponse = new();
            List<CustomerSealViewModel> customerSealViewModels = new();
            
            //Customer? customerQuery = dbContext.Customers.Find(customerSealReviewSearch.CustomerId);
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
