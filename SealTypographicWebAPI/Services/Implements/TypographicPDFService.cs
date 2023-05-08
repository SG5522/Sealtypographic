using AutoMapper;
using DBEntities;
using SealTypographicWebAPI.Models.TypographicPDF;
using SealTypographicWebAPI.Models;
using DBEntities.Consts;
using Microsoft.EntityFrameworkCore;
using DJLib.Models;

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
        public TypographicPDFPaginateViewModel GetPaginate(TypographicPDFSearch typographicPDFSearch)
        {

            return new();
        }

        /// <summary>
        /// 建立PDF排版資訊
        /// </summary>
        /// <param name="typographicPDFForm">排版資訊(新增使用)</param>     
        /// <returns></returns>
        public ResponseViewModel New(TypographicPDFForm typographicPDFForm)
        {
            ResponseViewModel response = new();            
            int userId = 0;
            
            UploadFile? pDFInfo = dbContext.UploadFiles.Find(typographicPDFForm.UploadId);
            Customer? customer = dbContext.Customers.Include(x => x.TypographicPDFs)                                                        
                                                    .FirstOrDefault(x => x.Id == typographicPDFForm.CustomerId);

            if (pDFInfo != null && customer != null)
            {
                List<TypographicPage> typographicPages = new();

                //新增PDF排版
                TypographicPDF typographicPDF = new()
                {
                    OriginFileName = pDFInfo.OriginalFileName,
                    FullPath = pDFInfo.FullPath,
                    Quarter = typographicPDFForm.Quarter
                };
                BaseInputTypographicPDF(typographicPDF, true, userId);
                foreach (TypographicPageForm pageInfo in typographicPDFForm.Pages)
                {
                    TypographicPage typographicPage = new();
                    PageSave(typographicPage, pageInfo);
                    typographicPages.Add(typographicPage);                    
                }
                typographicPDF.TypographicPages = typographicPages;
                customer.TypographicPDFs.Add(typographicPDF);                   
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
        /// <param name="typographicPDFSaveForm"></param>
        /// <returns></returns>
        public ResponseViewModel Save(TypographicPDFSaveForm typographicPDFSaveForm)
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

        /// <summary>
        /// 資料新增修改時基本資料輸入
        /// </summary>
        /// <param name="typographicPDF">DB上的排板PDF資料</param>        
        /// <param name="isCreate">確認是否新增還是更新的動作</param>
        /// <param name="userid">使用者ID</param>
        private void BaseInputTypographicPDF(TypographicPDF typographicPDF, bool isCreate, int userid)
        {
            if (isCreate)
            {                
                typographicPDF.CreateUserId = userid;
                typographicPDF.CreateDate = DateTime.Now;
                typographicPDF.DeleteStatus = DeleteStatus.No;
                typographicPDF.ReviewStatus = ReviewStatus.Draft;
            }
            else
            {
                typographicPDF.UpdateUserId = userid;
                typographicPDF.UpdateDate = DateTime.Now;
            }
        }

        /// <summary>
        /// PDF頁次存檔
        /// </summary>
        /// <param name="typographicPage"></param>        
        /// <param name="pageFrom">來源頁次</param>        
        private void PageSave(TypographicPage typographicPage, TypographicPageForm pageFrom)
        {            
            List<CustomerSealLocation> customerSealLocations = new();
            List<AccountantSignLocation> accountantSignLocations = new();
            List<LetterheadImageLocation> letterheadImageLocations = new();
            List<TemporarySealLocation> temporarySealLocations = new();

            typographicPage.PageNumber = pageFrom.PageNumber;
            typographicPage.BlankCheck = pageFrom.BlankCheck;
            typographicPage.DeleteCheck = pageFrom.DeleteCheck;
            typographicPage.IsAccountantCertificate = pageFrom.IsAccountantCertificate;
            
            //客戶印鑑座標
            foreach (CustomerSealLocationForm customerSealLocationForm in pageFrom.CustomerSealLocations)
            {
                CustomerSealLocation customerSealLocation = new()
                {
                    CustomerSealJournal = dbContext.CustomerSealJournals.Single(x => x.Id == customerSealLocationForm.CustomerSealId),
                    Left = customerSealLocationForm.Left,
                    Top = customerSealLocationForm.Top,
                    Height = customerSealLocationForm.Height,
                    Width = customerSealLocationForm.Width
                };
                customerSealLocations.Add(customerSealLocation);
            }
            typographicPage.CustomerSealLocations = customerSealLocations;

            //會計師簽印座標
            foreach (AccountantSingLocationForm accountantSingLocationForm in pageFrom.AccountantSingLocations)
            {
                AccountantSignLocation accountantSignLocation = new()
                {
                    AccountantSignJournal = dbContext.AccountantSignJournals.Single(x => x.Id == accountantSingLocationForm.AccountantSignId),
                    Left = accountantSingLocationForm.Left,
                    Top = accountantSingLocationForm.Top,
                    Height = accountantSingLocationForm.Height,
                    Width = accountantSingLocationForm.Width
                };
                accountantSignLocations.Add(accountantSignLocation);
            }
            typographicPage.AccountantSignLocations = accountantSignLocations;

            //信頭座標
            foreach (LetterheadImageLocationForm letterheadImageLocationForm in pageFrom.LetterheadImageLocations)
            {
                LetterheadImageLocation letterheadImageLocation = new()
                {
                    LetterheadImageJournal = dbContext.LetterheadImageJournals.Single(x => x.Id == letterheadImageLocationForm.LetterheadImageId),
                    Left = letterheadImageLocationForm.Left,
                    Top = letterheadImageLocationForm.Top,
                    Height = letterheadImageLocationForm.Height,
                    Width = letterheadImageLocationForm.Width
                };
                letterheadImageLocations.Add(letterheadImageLocation);
            }
            typographicPage.LetterheadImageLocations = letterheadImageLocations;

            //信頭座標
            foreach (TemporarySealLocationForm temporarySealLocationForm in pageFrom.TemporarySealLocations)
            {
                TemporarySealLocation temporarySealLocation = new()
                {
                    TemporarySealJournal = dbContext.TemporarySealJournals.Single(x => x.Id == temporarySealLocationForm.TemporarySealId),
                    Left = temporarySealLocationForm.Left,
                    Top = temporarySealLocationForm.Top,
                    Height = temporarySealLocationForm.Height,
                    Width = temporarySealLocationForm.Width
                };
                temporarySealLocations.Add(temporarySealLocation);
            }
            typographicPage.TemporarySealLocations = temporarySealLocations;
            //return typographicPages;
        }

    }
}
