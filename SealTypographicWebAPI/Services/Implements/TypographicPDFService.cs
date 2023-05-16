using AutoMapper;
using DBEntities;
using SealTypographicWebAPI.Models.TypographicPDF;
using SealTypographicWebAPI.Models;
using DBEntities.Consts;
using Microsoft.EntityFrameworkCore;
using DJLib.Models;
using System.Linq;

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
                                                    .ThenInclude(x => x.TypographicPages)
                                                    .ThenInclude(x => x.TypographicSealLocations)
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
            ResponseViewModel response = new();
            TypographicPDF? typographicPDF = dbContext.TypographicPDFs.Include(x => x.TypographicPages)      
                                                                      .ThenInclude(x => x.TypographicSealLocations)
                                                                      .FirstOrDefault(x => x.Id == typographicPDFSaveForm.TypographicPDFId);

            if(typographicPDF != null)
            {
                typographicPDF.TypographicPages = new();
                foreach(TypographicPageForm typographicPageForm in typographicPDFSaveForm.Pages)
                {

                }
            }

            return response;
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
            List<TypographicSealLocation> typographicSealLocations = new();

            typographicPage.PageNumber = pageFrom.PageNumber;
            typographicPage.BlankCheck = pageFrom.BlankCheck;
            typographicPage.DeleteCheck = pageFrom.DeleteCheck;
            typographicPage.IsAccountantCertificate = pageFrom.IsAccountantCertificate;
            
            //客戶印鑑座標
            foreach (CustomerSealLocationForm customerSealLocationForm in pageFrom.CustomerSealLocations)
            {
                TypographicSealLocation typographicSealLocation = mapper.Map<TypographicSealLocation>(customerSealLocationForm);
                typographicSealLocation.CustomerSealJournal = dbContext.CustomerSealJournals.Single(x => x.Id == customerSealLocationForm.Id);
                typographicSealLocations.Add(typographicSealLocation);
            }
            
            ////會計師簽印座標
            //foreach (AccountantSingLocationForm accountantSingLocationForm in pageFrom.AccountantSingLocations)
            //{
            //    TypographicSealLocation typographicSealLocation = mapper.Map<TypographicSealLocation>(accountantSingLocationForm);
            //    typographicSealLocation.AccountantSignJournal = dbContext.AccountantSignJournals.Single(x => x.Id == accountantSingLocationForm.Id);
            //    typographicSealLocations.Add(typographicSealLocation);
            //}            

            ////信頭座標
            //foreach (LetterheadImageLocationForm letterheadImageLocationForm in pageFrom.LetterheadImageLocations)
            //{
            //    TypographicSealLocation typographicSealLocation = mapper.Map<TypographicSealLocation>(letterheadImageLocationForm);
            //    typographicSealLocation.LetterheadImageJournal = dbContext.LetterheadImageJournals.Single(x => x.Id == letterheadImageLocationForm.Id);
            //    typographicSealLocations.Add(typographicSealLocation);
            //}            

            ////信頭座標
            //foreach (TemporarySealLocationForm temporarySealLocationForm in pageFrom.TemporarySealLocations)
            //{
            //    TypographicSealLocation typographicSealLocation = mapper.Map<TypographicSealLocation>(temporarySealLocationForm);
            //    typographicSealLocation.TemporarySealJournal = dbContext.TemporarySealJournals.Single(x => x.Id == temporarySealLocationForm.Id);
            //    typographicSealLocations.Add(typographicSealLocation);
            //}
            typographicPage.TypographicSealLocations = typographicSealLocations;            
        }

    }
}
