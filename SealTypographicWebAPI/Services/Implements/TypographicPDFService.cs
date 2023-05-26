using AutoMapper;
using SealTypographicWebAPI.Models.TypographicPDF;
using SealTypographicWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using DJSpireNet6;
using DBEntities;
using DBEntities.Consts;
using SealTypographicWebAPI.Utils;
using DJLib.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
        /// 取得PDF資訊
        /// </summary>
        /// <returns></returns>
        public PDFViewModel GetPDFView(int uploadFileid)
        {
            PDFViewModel pDFViewModel = new ();
            UploadFile? uploadFile = dbContext.UploadFiles.Find(uploadFileid);
            if (uploadFile != null) 
            {
                PdfView pdfView = new(uploadFile.FullPath);
                pDFViewModel.TotalPage = pdfView.GetTotalPage();
                pDFViewModel.PDFBase64 = pdfView.GetBase64ToWebApi();
                pDFViewModel.Success();
            }            
            else
            {
                pDFViewModel.DbNoData();
            }
            return pDFViewModel;
        }

        /// <summary>
        /// 建立PDF排版資訊
        /// </summary>
        /// <param name="typographicPDFForm">排版資訊(新增使用)</param>     
        /// <returns></returns>
        public TypographicPDFNewResronse New(TypographicPDFForm typographicPDFForm)
        {
            TypographicPDFNewResronse response = new();            
            int userId = 0;
            
            UploadFile? pDFInfo = dbContext.UploadFiles.Find(typographicPDFForm.UploadId);
            Customer? customer = dbContext.Customers.Find(typographicPDFForm.CustomerId);

            if (pDFInfo != null && customer != null)
            {
                List<TypographicPage> typographicPages = new();

                //之後輸入要從前端提供Id
                Quarter quarter = dbContext.Quarters
                                .Single
                                (
                                    x => x.TaiwanYear == typographicPDFForm.Quarter.Substring(0, 3)
                                    && x.Period == typographicPDFForm.Quarter.Substring(3)
                                );                

                //新增PDF排版
                TypographicPDF typographicPDF = new()
                {
                    Customer = customer,
                    UploadFile = pDFInfo,
                    OriginFileName = pDFInfo.OriginalFileName,
                    FullPath = pDFInfo.FullPath,
                    Quarter = quarter,
                    TypographicPages = new ()
                };
                BaseInputTypographicPDF(typographicPDF, true, userId);
                foreach (TypographicPageForm pageInfo in typographicPDFForm.Pages)
                {
                    typographicPDF.TypographicPages.Add(PageSave(pageInfo));
                }
                //typographicPDF.TypographicPages = typographicPages;
                dbContext.TypographicPDFs.Add(typographicPDF);                   
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
            TypographicPDF? typographicPDF = dbContext.TypographicPDFs
                                            .Include(x => x.TypographicPages)
                                            .ThenInclude(x => x.TypographicResourceLocations)
                                            .FirstOrDefault(x => x.Id == typographicPDFSaveForm.TypographicPDFId);

            if (typographicPDF != null)
            {
                // 移除現有的 TypographicResourceLocations
                //foreach (TypographicPage typographicPage in typographicPDF.TypographicPages)
                //{
                //    dbContext.RemoveRange(typographicPage.TypographicResourceLocations);
                //}

                //// 移除現有的 TypographicPages
                //dbContext.RemoveRange(typographicPDF.TypographicPages);
                typographicPDF.TypographicPages = new();
;                               
                foreach (TypographicPageForm typographicPageForm in typographicPDFSaveForm.Pages)
                {
                    typographicPDF.TypographicPages.Add(PageSave(typographicPageForm));
                }                
                try
                {
                    dbContext.SaveChanges();
                    response.Success();
                }
                catch (DbUpdateConcurrencyException ex) 
                {
                    // 處理樂觀併發例外
                    foreach (EntityEntry entry in ex.Entries)
                    {
                        entry.Reload();
                    }

                    // 重新執行更新操作
                    // ...

                    dbContext.SaveChanges();
                    response.Success();
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
        /// <param name="pageFrom">來源頁次</param>        
        private TypographicPage PageSave(TypographicPageForm pageFrom)
        {
            TypographicPage typographicPage = new();
            List<TypographicResourceLocation> typographicResourceLocations = new();

            typographicPage.PageNumber = pageFrom.PageNumber;
            typographicPage.BlankCheck = pageFrom.BlankCheck;
            typographicPage.DeleteCheck = pageFrom.DeleteCheck;
            typographicPage.IsAccountantCertificate = pageFrom.IsAccountantCertificate;

            //客戶印鑑座標
            foreach (CustomerSealLocationForm customerSealLocationForm in pageFrom.CustomerSealLocations)
            {
                TypographicResourceLocation typographicResourceLocation = mapper.Map<TypographicResourceLocation>(customerSealLocationForm);
                typographicResourceLocation.TypographicResource = dbContext.TypographicResources.Single(x => x.Id == customerSealLocationForm.Id);
                typographicResourceLocations.Add(typographicResourceLocation);
            }

            //會計師簽印座標
            foreach (AccountantSingLocationForm accountantSingLocationForm in pageFrom.AccountantSingLocations)
            {
                TypographicResourceLocation typographicResourceLocation = mapper.Map<TypographicResourceLocation>(accountantSingLocationForm);
                typographicResourceLocation.TypographicResource = dbContext.TypographicResources.Single(x => x.Id == accountantSingLocationForm.Id);
                typographicResourceLocations.Add(typographicResourceLocation);
            }

            //信頭座標
            foreach (LetterheadImageLocationForm letterheadImageLocationForm in pageFrom.LetterheadImageLocations)
            {
                TypographicResourceLocation typographicResourceLocation = mapper.Map<TypographicResourceLocation>(letterheadImageLocationForm);
                typographicResourceLocation.TypographicResource = dbContext.TypographicResources.Single(x => x.Id == letterheadImageLocationForm.Id);
                typographicResourceLocations.Add(typographicResourceLocation);
            }

            //信頭座標
            foreach (TemporarySealLocationForm temporarySealLocationForm in pageFrom.TemporarySealLocations)
            {
                TypographicResourceLocation typographicResourceLocation = mapper.Map<TypographicResourceLocation>(temporarySealLocationForm);
                typographicResourceLocation.TypographicResource = dbContext.TypographicResources.Single(x => x.Id == temporarySealLocationForm.Id);
                typographicResourceLocations.Add(typographicResourceLocation);
            }
            typographicPage.TypographicResourceLocations = typographicResourceLocations;

            return typographicPage;
        }

    }
}
