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
        public CustomerSealReviewViewModelResponse GetCustomerSealReviewViewModel(CustomerSealReviewSearch customerSealReviewSearch)
        {
            CustomerSealReviewViewModelResponse customerSealReviewViewModelResponse = new ();
            List<CustomerSealReviewViewModel> customerSealReviewViewModels = new ();

            List<CustomerSealJournal> customerSealJournalQuery = dbContext.CustomerSealJournals
                                                                .Include(customerSealJournal => customerSealJournal.Customer)
                                                                .Where
                                                                (
                                                                    customerSealJournal => customerSealJournal.DeleteStatus == DeleteStatus.NO                                                                    
                                                                    && customerSealJournal.Customer.DeleteStatus == DeleteStatus.NO
                                                                    && customerSealJournal.ReviewStatus == ReviewStatus.Pending
                                                                )
                                                                .OrderBy(customerSealJournal => customerSealJournal.Customer.CustomerNumber)
                                                                .GroupBy(customerSealJournal => new { 
                                                                                                        customerSealJournal.CustomerId,
                                                                                                        customerSealJournal.Quarter,                                                                                                        
                                                                                                    })
                                                                .Select(customerSealJournal => customerSealJournal.First())
                                                                .ToList();
            if (customerSealJournalQuery.Any())
            {
                //取得該頁            
                List<CustomerSealJournal> thisPageCustomerSealJournals = customerSealJournalQuery
                                                    .Skip((customerSealReviewSearch.PageNumber - 1) * customerSealReviewSearch.PageSize)
                                                    .Take(customerSealReviewSearch.PageSize)
                                                    .ToList();
                foreach (CustomerSealJournal customerSealJournal in thisPageCustomerSealJournals)
                {
                    CustomerSealReviewViewModel customerSealReviewViewModel = mapper.Map<CustomerSealReviewViewModel>(customerSealJournal);
                    customerSealReviewViewModel.ReviewStatus = ReviewStatusUtil.Get(customerSealJournal.ReviewStatus);
                    customerSealReviewViewModels.Add(customerSealReviewViewModel);
                }

                customerSealReviewViewModelResponse.CustomerSealReviewViewModels = customerSealReviewViewModels;
                customerSealReviewViewModelResponse.PageNumber = customerSealReviewSearch.PageNumber;
                customerSealReviewViewModelResponse.PageSize = customerSealReviewSearch.PageSize;
                //計算總頁數
                customerSealReviewViewModelResponse.TotalPage = TotalPageUtil.GetTotalPage(customerSealJournalQuery.Count(), customerSealReviewSearch.PageSize);
                customerSealReviewViewModelResponse.TotalCount = customerSealJournalQuery.Count();
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
        /// <param name="customerSealReviewQuarterSearch">搜尋條件</param>
        /// <returns></returns>

        public CustomerSealReviewDetailResponse GetCustomerSealReviewDetail(CustomerSealReviewQuarterSearch customerSealReviewQuarterSearch)
        {
            CustomerSealReviewDetailResponse customerSealReviewDetailResponse = new();
            List<CustomerSealViewModel> customerSealViewModels = new();
            
            Customer? customerQuery = dbContext.Customers.Find(customerSealReviewQuarterSearch.CustomerId);
            if(customerQuery != null) 
            {
                IQueryable<CustomerSealJournal> customerSealJournalQuery = dbContext.CustomerSealJournals
                                                                            .Include(customerSealJournal => customerSealJournal.SealMappingConfig)
                                                                            .Where
                                                                            (
                                                                                customerSealJournal => customerSealJournal.CustomerId == customerSealReviewQuarterSearch.CustomerId
                                                                                && customerSealJournal.Quarter == customerSealReviewQuarterSearch.Quarter
                                                                            )
                                                                            .OrderBy(customerSealJournal => customerSealJournal.SealMappingConfigId)
                                                                            .ThenBy(customerSealJournal => customerSealJournal.Sequence);
                if(customerSealJournalQuery.Any())
                {                    
                    foreach (CustomerSealJournal customerSealJournal in customerSealJournalQuery)
                    {
                        CustomerSealViewModel customerSealViewModel = mapper.Map<CustomerSealViewModel>(customerSealJournal);
                        customerSealViewModel.ImageBase64 = customerSealJournal.ImagePath;
                        customerSealViewModels.Add(customerSealViewModel);
                    }
                    customerSealReviewDetailResponse.CustomerSealReviewDetail = mapper.Map<CustomerSealReviewDetail>(customerQuery);
                    customerSealReviewDetailResponse.CustomerSealReviewDetail.CustomerSealViewModels = customerSealViewModels;                    
                    customerSealReviewDetailResponse.CustomerSealReviewDetail.Quarter = customerSealReviewQuarterSearch.Quarter;
                    customerSealReviewDetailResponse.Success();
                }
                else
                {
                    customerSealReviewDetailResponse.CustomerSealNoData();
                }
            }
            else
            {
                customerSealReviewDetailResponse.CustomeNoData();
            }            
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
                    customerSealJournal.ReviewUserId = 1; //之後要調整從驗證帳號中取得ID
                    customerSealJournal.ReviewStatus = reviewStatus;
                    customerSealJournal.ReviewDate = DateTime.Now;
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
