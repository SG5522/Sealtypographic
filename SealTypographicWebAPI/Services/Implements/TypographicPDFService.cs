using AutoMapper;
using SealTypographicWebAPI.Models.TypographicPDF;
using SealTypographicWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using DJSpire;
using DBEntities;
using DBEntities.Consts;
using SealTypographicWebAPI.Utils;
using DJSpire.Services;
using Serilog;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 管理PDF排版資訊
    /// </summary>
    public class TypographicPDFService : ITypographicPDFService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ImageService imageService;

        /// <summary>
        /// 取得DB與Automapper
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="imageService"></param>
        public TypographicPDFService(SealTypographicDbContext dbContext, IMapper mapper, ImageService imageService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.imageService = imageService;
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
                IQueryable<TypographicPDF> thisPageTypographicPDFs = typographicPDFs
                                                                    .Skip((typographicPDFSearch.PageNumber - 1) * typographicPDFSearch.PageSize)
                                                                    .Take(typographicPDFSearch.PageSize);

                foreach(TypographicPDF typographicPDF in thisPageTypographicPDFs)
                {
                    TypographicPDFViewModel typographicPDFViewModel = new()
                    {
                        Id = typographicPDF.Id,
                        CustomerCode = typographicPDF.Customer.Code,
                        CustomerName = typographicPDF.Customer.Name,
                        OriginFileName = typographicPDF.UploadFile.OriginalFileName,
                        Quarter = $"{typographicPDF.Quarter.TaiwanYear}{typographicPDF.Quarter.Period}",
                        ReviewStatus = typographicPDF.ReviewStatus
                    };
                    typographicPDFPaginateViewModel.ViewModels.Add(typographicPDFViewModel);
                }
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
            TypographicPagesResponse typographicPagesResponse = new();
            IQueryable<TypographicPage> typographicPages = dbContext.TypographicPages
                                                        .Include(x => x.TypographicResourceLocations)
                                                        .ThenInclude(x => x.TypographicResource)
                                                        .Where(x => x.TypographicPDF.Id == id);

            if(typographicPages != null)
            {
                typographicPagesResponse.Id = id;
                foreach(TypographicPage typographicPage in typographicPages)
                {
                    TypographicPageForm pageForm = new()
                    {
                        PageNumber = typographicPage.PageNumber,
                        DeleteCheck = typographicPage.DeleteCheck,
                        BlankCheck = typographicPage.BlankCheck,
                        IsAccountantCertificate = typographicPage.IsAccountantCertificate
                    };
                    foreach(TypographicResourceLocation typographicResourceLocation in typographicPage.TypographicResourceLocations)
                    {                        
                        switch (typographicResourceLocation.TypographicResource.SealType)
                        {
                            case SealType.Customer:
                                CustomerSealLocationForm customerSealLocationForm = mapper.Map<CustomerSealLocationForm>(typographicResourceLocation);
                                customerSealLocationForm.Id = id;
                                pageForm.CustomerSealLocations.Add(customerSealLocationForm);
                                break;
                            case SealType.Accountant:
                                AccountantSignLocationForm accountantSignLocationForm = mapper.Map<AccountantSignLocationForm>(typographicResourceLocation);
                                accountantSignLocationForm.Id = id;
                                pageForm.AccountantSignLocations.Add(accountantSignLocationForm);
                                break;
                            case SealType.Letterhead:
                                LetterheadImageLocationForm letterheadImageLocationForm = mapper.Map<LetterheadImageLocationForm>(typographicResourceLocation);
                                letterheadImageLocationForm.Id = id;
                                pageForm.LetterheadImageLocations.Add(letterheadImageLocationForm);
                                break;
                            case SealType.TemporarySeal:
                                TemporarySealLocationForm temporarySealLocationForm = mapper.Map<TemporarySealLocationForm>(typographicResourceLocation);
                                temporarySealLocationForm.Id = id;
                                pageForm.TemporarySealLocations.Add(temporarySealLocationForm);
                                break;
                        }
                    }
                    typographicPagesResponse.Pages.Add(pageForm);
                }
                typographicPagesResponse.Success();
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
            UploadFile? uploadFile = dbContext.UploadFiles.Find(uploadFileid);
            if (uploadFile != null) 
            {
                //取得單頁PDF圖檔
                PDFService pDFService = new()
                {
                    Path = uploadFile.FullPath,
                    PageIndex = pageNumber,
                };
                pDFViewModel.PDFFullPath = uploadFile.FullPath; //Log使用
                pDFViewModel.TotalPage = pDFService.GetTotalPage();
                pDFViewModel.PDFBase64 = pDFService.GetPDFPageBase64();                
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
            TypographicPageViewModel typographicPageViewModel = new();
            TypographicPDF? typographicPDF = dbContext.TypographicPDFs
                                            .Include(x => x.UploadFile)
                                            .Select
                                            (
                                                x => new TypographicPDF()
                                                {
                                                    Id = x.Id,
                                                    UploadFile = new UploadFile()
                                                    {
                                                        Id = x.UploadFile.Id,
                                                        FullPath = x.UploadFile.FullPath,
                                                    }
                                                }
                                            )
                                            .FirstOrDefault
                                            (
                                                x => x.Id == typographicPDFPageSearch.Id                                                
                                            );
                                                

            if (typographicPDF != null)
            {
                //取得單頁PDF圖檔
                PDFService pDFService = new()
                {
                    Path = typographicPDF.UploadFile.FullPath,
                    PageIndex = typographicPDFPageSearch.PageNumber,
                };
                
                typographicPageViewModel.PageNumber = typographicPDFPageSearch.PageNumber;
                typographicPageViewModel.PDFImageBase64 = pDFService.GetPageImageBase64();                

                TypographicPage? typographicPages = dbContext.TypographicPages
                                                    .Include(x => x.TypographicPDF)
                                                    .Include(x => x.TypographicResourceLocations)
                                                    .ThenInclude(x => x.TypographicResource)
                                                    .FirstOrDefault(
                                                                        x => x.TypographicPDF.Id == typographicPDFPageSearch.Id
                                                                        && x.PageNumber == typographicPDFPageSearch.PageNumber
                                                                    );
                

                //如有該頁有編輯頁面資訊才進行查詢
                if (typographicPages != null)
                {
                    typographicPageViewModel.Id = typographicPages.Id;
                    #region 顯示已編輯頁次內裡面所有的印鑑擺放位置與顯示名稱
                    foreach (TypographicResourceLocation typographicResourceLocation in typographicPages.TypographicResourceLocations) 
                    {
                        switch (typographicResourceLocation.TypographicResource.SealType)
                        {
                            case SealType.Customer:                                
                                CustomerSealLocationViewModel customerSealLocationViewModel = mapper.Map<CustomerSealLocationViewModel>(typographicResourceLocation);
                                customerSealLocationViewModel.Sequence = typographicResourceLocation.TypographicResource.Sequence;
                                customerSealLocationViewModel.CustomerSealType = SealMappingConfigUtil.GetCustomerSealType(typographicResourceLocation.TypographicResource.SubSealType);
                                customerSealLocationViewModel.ImageBase64 = imageService.GetPathToBase64(typographicResourceLocation.TypographicResource.ImageFullPath);
                                typographicPageViewModel.CustomerSealLocationViewModels.Add(customerSealLocationViewModel);
                                break;
                            case SealType.Accountant:
                                string accountantName = dbContext.Accountants
                                                        .Single
                                                        (
                                                            x => x.AccountantSignGroups.Any
                                                            (
                                                                x => x.TypographicResources.Any
                                                                (
                                                                    x => x.Id == typographicResourceLocation.TypographicResource.Id
                                                                )
                                                            )
                                                        ).Name;                                                        
                                AccountantSignLocationViewModel accountantSignLocationViewModel = mapper.Map<AccountantSignLocationViewModel>(typographicResourceLocation);
                                accountantSignLocationViewModel.AccountantSignType = SealMappingConfigUtil.GetAccountantSignType(typographicResourceLocation.TypographicResource.SubSealType);
                                accountantSignLocationViewModel.AccountantName = accountantName;
                                accountantSignLocationViewModel.ImageBase64 = imageService.GetPathToBase64(typographicResourceLocation.TypographicResource.ImageFullPath);
                                typographicPageViewModel.AccountantSignLocationViewModels.Add(accountantSignLocationViewModel);
                                break;
                            case SealType.Letterhead:
                                string letterheadName = dbContext.Letterheads
                                                        .Single
                                                        (
                                                            x => x.TypographicResources.Any
                                                            (
                                                                x => x.Id == typographicResourceLocation.TypographicResource.Id
                                                            )
                                                        ).Name;
                                LetterheadImageLocationViewModel letterheadImageLocationForm = mapper.Map<LetterheadImageLocationViewModel>(typographicResourceLocation);
                                letterheadImageLocationForm.LetterheadName = letterheadName;
                                letterheadImageLocationForm.ImageBase64 = imageService.GetPathToBase64(typographicResourceLocation.TypographicResource.ImageFullPath);
                                typographicPageViewModel.LetterheadImageLocationViewModels.Add(letterheadImageLocationForm);
                                break;
                            case SealType.TemporarySeal:
                                TemporarySealLocationViewModel temporarySealLocationViewModel = mapper.Map<TemporarySealLocationViewModel>(typographicResourceLocation);
                                temporarySealLocationViewModel.Sequence = typographicResourceLocation.TypographicResource.Sequence;
                                temporarySealLocationViewModel.ImageBase64 = imageService.GetPathToBase64(typographicResourceLocation.TypographicResource.ImageFullPath);
                                typographicPageViewModel.TemporarySealLocationViewModels.Add(temporarySealLocationViewModel);
                                break;
                        }
                    }
                    #endregion                    
                }
                typographicPageViewModel.Success();
            }            

            return typographicPageViewModel;
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
            typographicPage.IsAccountantCertificate = pageFrom.IsAccountantCertificate;

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
