using AutoMapper;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models.TypographicPDF;
using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 管理PDF排版資訊
    /// </summary>
    public class TypographicPDFService : ITypographicPDFService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly IMapper mapper;

        /// <summary>
        /// 取得DB與Automapper
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public TypographicPDFService(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得PDF排版資訊
        /// </summary>
        /// <param name="typographicPDFSearch">搜尋條件</param>
        /// <returns></returns>
        public TypographicPDFViewModel GetTypographicPDFViewModel(TypographicPDFSearch typographicPDFSearch)
        {

            return new();
        }

        /// <summary>
        /// 建立PDF排版資訊
        /// </summary>
        /// <param name="typographicPDFForm"></param>
        /// <returns></returns>
        public ResponseViewModel CreateTypographicForm(TypographicPDFForm typographicPDFForm)
        {
            ResponseViewModel response = new();
            IQueryable<TypographicPDF> typographicPDFQuery = dbContext.TypographicPDFs
                                    .Where(typographicPDF => 
                                           typographicPDF.CustomerId == typographicPDFForm.CustomerId
                                           && typographicPDF.Quarter == typographicPDFForm.Quarter);

            if (!typographicPDFQuery.Any())
            {
                TypographicPDF dbtypographicPDF = new();
                List<TypographicPage> typographicPage = new();
                List<CustomerSealLocation> customerSealLocation = new();
                List<AccountantSignLocation> accountantSingLocation = new();
                List<LetterheadImageLocation> letterheadImageLocation = new();
                foreach (TypographicPageForm typographicPageForm in typographicPDFForm.TypographicPagesForm)
                {
                    
                }
                


                dbContext.TypographicPDFs.Add(dbtypographicPDF);
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.DbNoData();
            }

            return response;
        }

        /// <summary>
        /// 建立PDF排版資訊
        /// </summary>
        /// <param name="typographicPDFForm"></param>
        /// <returns></returns>
        public ResponseViewModel UpTypographicForm(TypographicPDFForm typographicPDFForm)
        {
            return new();
        }

        private List<TypographicPage> GetTypographicPages(List<TypographicPageForm> typographicPageForms)
        {
            List<TypographicPage> typographicPages = new();
            foreach (TypographicPageForm typographicPageForm in typographicPageForms)
            {
                typographicPages.Add(mapper.Map<TypographicPage>(typographicPageForm));
            }
            return typographicPages;
        }

        private List<CustomerSealLocation> GetCusTomerSealLocations(List<CustomerSealLocationForm> customerSealLocationForms)
        {
            List<CustomerSealLocation> customerSealLocations = new();
            foreach (CustomerSealLocationForm customerSealLocationForm in customerSealLocationForms)
            {
                customerSealLocations.Add(mapper.Map<CustomerSealLocation>(customerSealLocationForm));
            }
            return customerSealLocations;
        }

    }
}
