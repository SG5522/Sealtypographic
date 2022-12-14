using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models.Accountant;
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
                                                                    && customerSealJournal.ReviewStatus == ReviewStatus.Pending
                                                                    && customerSealJournal.Customer.DeleteStatus == DeleteStatus.NO
                                                                )
                                                                .OrderBy(customerSealJournal => customerSealJournal.Customer.CustomerNumber)
                                                                .GroupBy(customerSealJournal => new { customerSealJournal.CustomerId , customerSealJournal.Quarter })
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
                customerSealReviewViewModelResponse.DbNoData();
            }

            return customerSealReviewViewModelResponse;
        }
    }
}
