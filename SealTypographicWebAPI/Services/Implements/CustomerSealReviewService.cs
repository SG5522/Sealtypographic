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


            List<CustomerSealJournal> CustomerSealJournalQuery = dbContext.CustomerSealJournals
                                                                .Include(customerSealJournal => customerSealJournal.Customer)
                                                                .Where
                                                                (
                                                                    customerSealJournal => customerSealJournal.DeleteStatus == DeleteStatus.NO
                                                                    && customerSealJournal.ReviewStatus == ReviewStatus.Pending
                                                                )
                                                                .OrderBy(customerSealJournal => customerSealJournal.Customer.CustomerNumber)
                                                                .Distinct()
                                                                .ToList();

            //var customerSealJournals = CustomerSealJournalQuery.DistinctBy(customerSealJournal => customerSealJournal.Quarter);


            //if (CustomerSealJournalQuery.Any())
            //{
            //    //取得該頁            
            //    List<CustomerSealJournal> thisPageCustomerSealJournals = CustomerSealJournalQuery                                          
            //                                        .Skip((customerSealReviewSearch.PageNumber - 1) * customerSealReviewSearch.PageSize)
            //                                        .Take(customerSealReviewSearch.PageSize)
            //                                        .ToList();
            //    foreach (CustomerSealJournal customerSealJournal in thisPageCustomerSealJournals)
            //    {
            //        CustomerSealReviewViewModel customerSealReviewViewModel = mapper.Map<CustomerSealReviewViewModel>(customerSealJournal);
            //        customerSealReviewViewModels.Add(customerSealReviewViewModel);
            //    }

            //    customerSealReviewViewModelResponse.CustomerSealReviewViewModels = customerSealReviewViewModels;
            //    customerSealReviewViewModelResponse.PageNumber = customerSealReviewSearch.PageNumber;
            //    customerSealReviewViewModelResponse.PageSize = customerSealReviewSearch.PageSize;
            //    //計算總頁數
            //    customerSealReviewViewModelResponse.TotalPage = TotalPageUtil.GetTotalPage(CustomerSealJournalQuery.Count(), customerSealReviewSearch.PageSize);
            //    customerSealReviewViewModelResponse.TotalCount = CustomerSealJournalQuery.Count();
            //    customerSealReviewViewModelResponse.Success();
            //}            
            //else
            //{
            //    customerSealReviewViewModelResponse.DbNoData();
            //}

            return customerSealReviewViewModelResponse;
        }
    }
}
