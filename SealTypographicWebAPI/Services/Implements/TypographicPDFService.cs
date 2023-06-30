using AutoMapper;
using SealTypographicWebAPI.Models.TypographicPDF;
using SealTypographicWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using DBEntities;
using DBEntities.Consts;
using SealTypographicWebAPI.Utils;
using DJSpire.Services;
using DJSpire.Models;
using Serilog;
using DJLib;
using AutoMapper.QueryableExtensions;
using DJSpire.Consts;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 管理PDF排版資訊
    /// </summary>
    public class TypographicPDFService : ITypographicPDFService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;

        /// <summary>
        /// 取得DB與Automapper
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>        
        public TypographicPDFService(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;            
            configurationProvider = mapper.ConfigurationProvider;
        }

        /// <summary>
        /// 取得PDF排版資訊
        /// </summary>
        /// <param name="typographicPDFSearch">搜尋條件</param>
        /// <returns></returns>
        public TypographicPDFPaginateViewModel GetPaginate(TypographicPDFSearch typographicPDFSearch)
        {
            TypographicPDFPaginateViewModel typographicPDFPaginateViewModel = new();
            int companyId = 1;
            IQueryable<TypographicPDF> typographicPDFs = dbContext.TypographicPDFs
                                                        .Include(x => x.Customer)
                                                        .Include(x => x.Quarter)
                                                        .Where
                                                        (
                                                            x => x.Customer.Company.Id == companyId
                                                            && x.DeleteStatus == DeleteStatus.No                                                            
                                                        );

            if (!string.IsNullOrEmpty(typographicPDFSearch.CustomerKeyWord))
            {
                typographicPDFs = typographicPDFs.Where
                                (
                                    x => x.Customer.Code.ToLower().Contains(typographicPDFSearch.CustomerKeyWord.ToLower())
                                    || x.Customer.Name.Contains(typographicPDFSearch.CustomerKeyWord)
                                );
            }

            if(!string.IsNullOrEmpty(typographicPDFSearch.Quarter))
            {                
                typographicPDFs = typographicPDFs.Where
                                (
                                    x => x.Quarter.TaiwanYear.Contains(typographicPDFSearch.Quarter.Substring(0, 3))
                                    && x.Quarter.Period == typographicPDFSearch.Quarter.Substring(3)
                                );
            }

            if(typographicPDFSearch.ReviewStatus != null)
            {
                typographicPDFs = typographicPDFs.Where
                                (
                                    x => x.ReviewStatus == typographicPDFSearch.ReviewStatus
                                );
            }

            typographicPDFs = typographicPDFs.OrderByDescending(x => x.Id);

            if(typographicPDFs.Any())
            {
                typographicPDFPaginateViewModel.ViewModels = mapper.ProjectTo<TypographicPDFViewModel>
                                                            (
                                                                    typographicPDFs
                                                                    .Skip((typographicPDFSearch.PageNumber - 1) * typographicPDFSearch.PageSize)
                                                                    .Take(typographicPDFSearch.PageSize)
                                                            ).ToList();

                int totalCount = typographicPDFs.Count();
                typographicPDFPaginateViewModel.PageNumber = typographicPDFSearch.PageNumber;
                typographicPDFPaginateViewModel.PageSize = typographicPDFSearch.PageSize;
                typographicPDFPaginateViewModel.TotalCount = totalCount;
                typographicPDFPaginateViewModel.TotalPage = TotalPageUtil.GetTotalPage(totalCount, typographicPDFSearch.PageSize);
                typographicPDFPaginateViewModel.Success();
            }
            else
            {
                typographicPDFPaginateViewModel.DbNoData();
            }
            return typographicPDFPaginateViewModel;
        }

        /// <summary>
        /// 取得已編輯PDF頁次資訊
        /// </summary>
        /// <param name="id">TypographicPDFId</param>
        /// <returns></returns>
        public TypographicPagesResponse GetEditPages(int id)
        {
            TypographicPagesResponse? typographicPagesResponse = mapper.ProjectTo<TypographicPagesResponse>
                                                                (
                                                                    dbContext.TypographicPDFs
                                                                    .Include(x => x.TypographicPages)
                                                                    .ThenInclude(x => x.TypographicResourceLocations)
                                                                    .AsSplitQuery()
                                                                ).FirstOrDefault(x => x.Id == id);

            if (typographicPagesResponse != null)
            {
                typographicPagesResponse.Success();
            }
            else
            {
                typographicPagesResponse = new();
                typographicPagesResponse.DbNoData();
            }
            return typographicPagesResponse;
        }

        /// <summary>
        /// 取得PDF資訊
        /// <param name="uploadFileid">上傳檔案Id</param>
        /// <param name="pageNumber">pdf頁次</param>  
        /// </summary>
        /// <returns></returns>
        public PDFViewModel GetPDFView(int uploadFileid, int pageNumber)
        {
            PDFViewModel pDFViewModel = new ();            
            string? uploadPath = dbContext.UploadFiles.Where(x => x.Id == uploadFileid).Select(x => x.FullPath).FirstOrDefault();
            if (uploadPath != null) 
            {
                //取得單頁PDF圖檔
                PDFService pDFService = new()
                {
                    PDFPath = uploadPath,
                    PageIndex = pageNumber,
                };
                pDFViewModel.PDFFullPath = uploadPath; //Log使用
                pDFViewModel.TotalPage = pDFService.GetTotalPage();
                pDFViewModel.ImageBase64 = pDFService.GetPageImageBase64();                
                pDFViewModel.Success();                
            }            
            else
            {
                pDFViewModel.DbNoData();
            }
            
            Log.Information("TypographicPDF PDFView output {@Output}", mapper.Map<PDFViewModel>(pDFViewModel));
            return pDFViewModel;
        }

        /// <summary>
        /// 取得單頁PDF圖像與排版編輯資訊
        /// </summary>
        /// <param name="typographicPDFPageSearch">排板PDFPage搜尋</param>
        /// <returns></returns>
        public TypographicPageViewModel GetPageView(TypographicPDFPageSearch typographicPDFPageSearch)
        {
            TypographicPageViewModel? typographicPageViewModel = new();

            string? pdfFullPath = dbContext.TypographicPDFs.Include(x => x.UploadFile)
                                 .Where(x => x.Id == typographicPDFPageSearch.Id)
                                 .Select(x => x.FullPath)
                                 .FirstOrDefault();

            typographicPageViewModel = mapper.ProjectTo<TypographicPageViewModel>
                                    (
                                        dbContext.TypographicPages
                                        .Include(x => x.TypographicResourceLocations)
                                        .ThenInclude(x => x.TypographicResource)
                                        .AsSplitQuery()
                                    ).FirstOrDefault(x => x.Id == typographicPDFPageSearch.Id && x.PageNumber == typographicPDFPageSearch.PageNumber);

            if (pdfFullPath != null && typographicPageViewModel != null)
            {
                //取得單頁PDF圖檔
                PDFService pDFService = new()
                {
                    PDFPath = pdfFullPath,
                    PageIndex = typographicPDFPageSearch.PageNumber,
                };

                typographicPageViewModel.PDFImageBase64 = pDFService.GetPageImageBase64();
                typographicPageViewModel.Success();                
            }            
            else
            {
                typographicPageViewModel = new();
                typographicPageViewModel.DbNoData();
            }
            return typographicPageViewModel;
        }

        /// <summary>
        /// 讀取排版PDF的概要
        /// </summary>
        /// <param name="typographicPDFId">PDFID</param>
        /// <returns></returns>
        public TypographicPDFSettingViewModel GetTypographicPDFSummary(int typographicPDFId)
        {
            TypographicPDFSettingViewModel? typographicPDFSettingViewModel = mapper.ProjectTo<TypographicPDFSettingViewModel>
                                                                            (
                                                                                dbContext.TypographicPDFs.Include(x => x.UploadFile)
                                                                                                        .Include(x => x.Quarter)
                                                                                                        .Include(x => x.TypographicPages)
                                                                                                        .Where(x => x.Id == typographicPDFId)
                                                                            ).FirstOrDefault();
            if(typographicPDFSettingViewModel != null)
            {
                typographicPDFSettingViewModel.Success();
            }
            else
            {
                typographicPDFSettingViewModel = new();
                typographicPDFSettingViewModel.DbNoData();
            }

            return typographicPDFSettingViewModel;
        }

        /// <summary>
        /// 取得排版後的PDFBase64
        /// </summary>
        /// <param name="typographicPDFId">PDF排版ID</param>
        /// <returns></returns>
        public TypographicPDFEditViewResponse GetEditPDFView(int typographicPDFId)
        {
            TypographicPDFEditViewResponse typographicPDFEditViewResponse = new();

            List<EditPage> editPages = dbContext.TypographicPages
                                        .Include(x => x.TypographicResourceLocations)
                                        .ThenInclude(x => x.TypographicResource)
                                        .Where(x => x.TypographicPDF.Id == typographicPDFId)
                                        .ProjectTo<EditPage>(configurationProvider).ToList();

            if (editPages != null)
            {
                EditPDF editPDF = new()
                {
                    PDFColor = PDFColor.Original,
                    IsBlank = false,
                    EditPages = editPages
                };
                string? pdfPath = dbContext.UploadFiles
                                .Where(x => x.TypographicPDFs.Any(x => x.Id == typographicPDFId))
                                .Select(x => x.FullPath)
                                .FirstOrDefault();
                
                PDFService pDFService = new() { PDFPath = pdfPath };
                typographicPDFEditViewResponse.PDFBase64 = pDFService.GetEditPDFBase64(editPDF);
                typographicPDFEditViewResponse.Success();
            }
            else
            {
                typographicPDFEditViewResponse.DbNoData();
            }

            return typographicPDFEditViewResponse;
        }

        /// <summary>
        /// 建立排版後的PDF
        /// </summary>
        /// <param name="typographicPDFMakeSetting">輸出PDF檔案時的設定</param>
        /// <returns></returns>
        public TypographicPDFMakeResponse MakeTyporaphicPDF(TypographicPDFMakeSetting typographicPDFMakeSetting)
        {
            TypographicPDFMakeResponse typographicPagePDFResponse = new ();            

            List<EditPage> editPages = dbContext.TypographicPages
                                        .Include(x => x.TypographicResourceLocations)
                                        .ThenInclude(x => x.TypographicResource)
                                        .Where(x => x.TypographicPDF.Id == typographicPDFMakeSetting.TypographicPDFId)
                                        .ProjectTo<EditPage>(configurationProvider).ToList();

            if (editPages != null)
            {
           
                EditPDF editPDF = new()
                {
                    PDFColor = typographicPDFMakeSetting.PDFColor,
                    IsBlank = typographicPDFMakeSetting.IsBlank,
                    EditPages = editPages
                };
                string? pdfPath = dbContext.UploadFiles
                                .Where(x => x.TypographicPDFs.Any(x => x.Id == typographicPDFMakeSetting.TypographicPDFId))
                                .Select(x => x.FullPath)
                                .FirstOrDefault();                

                PDFService pDFService = new() { PDFPath = pdfPath };                
                typographicPagePDFResponse.PDFBase64 = pDFService.GetEditPDFBase64(editPDF);
                typographicPagePDFResponse.Success();
            }
            else
            {
                typographicPagePDFResponse.Error();
            }
            Log.Information("TypographicPDF makePDF output {@Output}", typographicPagePDFResponse.Message);
            return typographicPagePDFResponse;
        }

        /// <summary>
        /// 建立PDF排版資訊
        /// </summary>
        /// <param name="typographicPDFForm">排版資訊(新增使用)</param>     
        /// <returns></returns>
        public TypographicPDFNewResronse New(TypographicPDFForm typographicPDFForm)
        {
            TypographicPDFNewResronse typographicPDFNewResronse = new();            
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
                
                dbContext.TypographicPDFs.Add(typographicPDF);                   
                dbContext.SaveChanges();
                typographicPDFNewResronse.TypographicPDFId = typographicPDF.Id;
                typographicPDFNewResronse.Success();
            }
            else
            {
                typographicPDFNewResronse.DbNoData();
            }
            return typographicPDFNewResronse;
        }

        /// <summary>
        /// 儲存PDF排版資訊(更新資料)
        /// </summary>
        /// <param name="typographicPDFSaveForm">排版資訊</param>
        /// <returns></returns>
        public ResponseViewModel Save(TypographicPDFSaveForm typographicPDFSaveForm)
        {
            ResponseViewModel response = new();
            int userId = 1;
            TypographicPDF? typographicPDF = dbContext.TypographicPDFs
                                            .Include(x => x.TypographicPages)
                                            .FirstOrDefault(x => x.Id == typographicPDFSaveForm.TypographicPDFId);                                            

            if (typographicPDF != null)
            {
                typographicPDF.Customer = dbContext.Customers.Single(x => x.Id == typographicPDFSaveForm.CustomerId);
                typographicPDF.UploadFile = dbContext.UploadFiles.Single(x => x.Id == typographicPDFSaveForm.UploadId);
                //之後輸入要從前端提供Id
                typographicPDF.Quarter = dbContext.Quarters
                                        .Single
                                        (
                                            x => x.TaiwanYear == typographicPDFSaveForm.Quarter.Substring(0, 3)
                                            && x.Period == typographicPDFSaveForm.Quarter.Substring(3)
                                        );
                typographicPDF.ReviewStatus = ReviewStatus.Approval;
                BaseInputTypographicPDF(typographicPDF, false, userId);
                List<TypographicPage> newPages = new ();                

                foreach (TypographicPageForm typographicPageForm in typographicPDFSaveForm.Pages)
                {
                    newPages.Add(PageSave(typographicPageForm));                    
                }
                //由於Include(Pages)所以更換成newPages後會將舊的資料刪除
                typographicPDF.TypographicPages = newPages;
                dbContext.SaveChanges();
                response.Success();
            }            
            return response;
        }

        /// <summary>
        /// 變更PDF排版建檔狀態(未來會變更為審核狀態)
        /// </summary>
        /// <param name="typographicPDFId">PDF排版ID</param>
        /// <returns></returns>
        public ResponseViewModel Approval(int typographicPDFId)
        {            
            return ChangeReviewStatus(typographicPDFId, ReviewStatus.Approval);
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="typographicPDFId">排版PDF ID</param>
        /// <returns></returns>
        public ResponseViewModel Delete(int typographicPDFId)
        {
            ResponseViewModel response = new();
            TypographicPDF? typographicPDF = dbContext.TypographicPDFs.Find(typographicPDFId);
            int userId = 1;
            if(typographicPDF != null)
            {
                typographicPDF.DeleteStatus = DeleteStatus.Yes;
                typographicPDF.ReviewStatus = ReviewStatus.Disabled;
                BaseInputTypographicPDF(typographicPDF, false, userId);
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
        /// 變更PDF狀態
        /// </summary>
        /// <param name="typographicPDFId">PDFID</param>
        /// <param name="reviewStatus">狀態</param>
        /// <returns></returns>
        private ResponseViewModel ChangeReviewStatus(int typographicPDFId, ReviewStatus reviewStatus)
        {
            ResponseViewModel response = new();
            int userId = 1;
            TypographicPDF? typographicPDF = dbContext.TypographicPDFs.Find(typographicPDFId);
            if (typographicPDF != null)
            {
                typographicPDF.ReviewStatus = reviewStatus;
                BaseInputTypographicPDF(typographicPDF, false, userId);                
                dbContext.SaveChanges();
                response.Success();
            }
            return response;
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

            if(pageFrom.AccountantCertificateId != 0)
            {                
                typographicPage.UploadFile = dbContext.UploadFiles.Find(pageFrom.AccountantCertificateId);
            }
            
            //客戶印鑑座標
            foreach (CustomerSealLocationForm customerSealLocationForm in pageFrom.CustomerSealLocations)
            {
                TypographicResourceLocation typographicResourceLocation = mapper.Map<TypographicResourceLocation>(customerSealLocationForm);
                typographicResourceLocation.TypographicResource = dbContext.TypographicResources.Single(x => x.Id == customerSealLocationForm.Id);
                typographicResourceLocations.Add(typographicResourceLocation);
            }

            //會計師簽印座標
            foreach (AccountantSignLocationForm accountantSignLocationForm in pageFrom.AccountantSignLocations)
            {
                TypographicResourceLocation typographicResourceLocation = mapper.Map<TypographicResourceLocation>(accountantSignLocationForm);
                typographicResourceLocation.TypographicResource = dbContext.TypographicResources.Single(x => x.Id == accountantSignLocationForm.Id);
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
