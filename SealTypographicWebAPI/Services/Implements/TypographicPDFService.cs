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
using AutoMapper.QueryableExtensions;
using DJSpire.Consts;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.Accountant;

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
        private readonly ILogger<TypographicPDFService> logger;

        /// <summary>
        /// 取得DB與Automapper
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>        
        public TypographicPDFService(SealTypographicDbContext dbContext, IMapper mapper, ILogger<TypographicPDFService> logger)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;            
            configurationProvider = mapper.ConfigurationProvider;
            this.logger = logger;
        }

        ///<inheritdoc />
        public TypographicPDFPaginateViewModel GetPaginate(TypographicPDFSearch typographicPDFSearch, TypographyType typographyType, int userId = 0)
        {
            logger.LogInformation("GetPaginate input {@typographicPDFSearch} typographyType: {@typographyType} userId: {@userId}", typographicPDFSearch, typographyType, userId);

            TypographicPDFPaginateViewModel typographicPDFPaginateViewModel = new();
            int companyId = 1;

            try
            {
                IQueryable<TypographicPDF> typographicPDFs = dbContext.TypographicPDFs
                                                        .Include(x => x.Customer)
                                                        .Include(x => x.QuarterYear)
                                                        .Where
                                                        (
                                                            x => x.Customer.Company.Id == companyId
                                                            && x.DeleteStatus == DeleteStatus.No
                                                            && x.TypographyType == typographyType
                                                        );

                if (!string.IsNullOrEmpty(typographicPDFSearch.CustomerKeyWord))
                {
                    typographicPDFs = typographicPDFs.Where
                                        (
                                            x => x.Customer.Code.ToLower().Contains(typographicPDFSearch.CustomerKeyWord.ToLower())
                                            || x.Customer.Name.ToLower().Contains(typographicPDFSearch.CustomerKeyWord.ToLower())
                                        );
                }

                if (typographicPDFSearch.QuarterYearId != null)
                {
                    typographicPDFs = typographicPDFs.Where(x => x.QuarterYear.Id == typographicPDFSearch.QuarterYearId);
                }

                if (typographicPDFSearch.ReviewStatus != null)
                {
                    typographicPDFs = typographicPDFs.Where(x => x.ReviewStatus == typographicPDFSearch.ReviewStatus);
                }

                typographicPDFs = typographicPDFs.OrderByDescending(x => x.Id);

                if (typographicPDFs.Any())
                {
                    typographicPDFPaginateViewModel.ViewModels = typographicPDFs
                                                                .Skip((typographicPDFSearch.PageNumber - 1) * typographicPDFSearch.PageSize)
                                                                .Take(typographicPDFSearch.PageSize)
                                                                .ProjectTo<TypographicPDFViewModel>(configurationProvider)
                                                                .ToList();

                    PageUtil.SetPageData(typographicPDFPaginateViewModel, typographicPDFSearch.PageNumber, typographicPDFSearch.PageSize, typographicPDFs.Count());
                    typographicPDFPaginateViewModel.Success();
                }
                else
                {
                    typographicPDFPaginateViewModel.DbNoData();
                }
                logger.LogInformation("GetPaginate output {@output} ", typographicPDFPaginateViewModel);
            }
            catch (Exception ex) 
            {
                typographicPDFPaginateViewModel.Error();
                logger.LogError("GetPaginate error {@error}", ex.Message);
            }
            
            return typographicPDFPaginateViewModel;
        }

        ///<inheritdoc />
        public TypographicPagesResponse GetEditPages(int id, int userId = 0)
        {
            logger.LogInformation("GetPaginate input id: {@id} userId: {@userId}", id, userId);

            TypographicPagesResponse? typographicPagesResponse ;

            try
            {
                typographicPagesResponse = dbContext.TypographicPDFs
                                            .Include(x => x.TypographicPages)
                                            .ThenInclude(x => x.TypographicResourceLocations)
                                            .Where(x => x.Id == id)
                                            .ProjectTo<TypographicPagesResponse>(configurationProvider)
                                            .FirstOrDefault();

                if (typographicPagesResponse != null)
                {
                    typographicPagesResponse.Success();
                }
                else
                {
                    typographicPagesResponse = new();
                    typographicPagesResponse.DbNoData();
                }
                logger.LogInformation("GetPaginate output {@output}", typographicPagesResponse);
            }
            catch (Exception ex)
            {
                typographicPagesResponse = new();
                typographicPagesResponse.Error();
                logger.LogError("GetPaginate error {@error}", ex.Message);
            }
            
            return typographicPagesResponse;
        }

        ///<inheritdoc />
        public PDFViewModel GetPDFView(int uploadFileid, int pageNumber, int userId = 0)
        {
            logger.LogInformation("GetPDFView input uploadFileid: {@uploadFileid} pageNumber: {@pageNumber} userId: {@userId}"
                                    , uploadFileid, pageNumber, userId);

            PDFViewModel pDFViewModel = new ();

            try
            {
                string? uploadPath = dbContext.UploadFiles.Where(x => x.Id == uploadFileid).Select(x => x.FullPath).FirstOrDefault();
                if (uploadPath != null)
                {
                    //取得單頁PDF圖檔
                    PDFService pDFService = new()
                    {
                        PDFPath = uploadPath,
                        PageIndex = pageNumber,
                    };
                    PDFImageInfo pDFImageInfo = pDFService.GetPageImageInfo(PDFImageScaleConsts.Default);

                    pDFViewModel.PDFFullPath = uploadPath; //Log使用
                    pDFViewModel.TotalPage = pDFService.GetTotalPage();
                    pDFViewModel.ImageWidth = pDFImageInfo.Width;
                    pDFViewModel.ImageHeight = pDFImageInfo.Height;
                    pDFViewModel.ImageBase64 = pDFImageInfo.ImageBase64;
                    pDFViewModel.Success();
                }
                else
                {
                    pDFViewModel.DbNoData();
                }
                logger.LogInformation("GetPDFView output {@output}", mapper.Map<PDFViewModel>(pDFViewModel));
            }
            catch (Exception ex)
            {
                pDFViewModel.Error();
                logger.LogInformation("GetPDFView error {@error}", ex.Message);
            }
            return pDFViewModel;
        }

        ///<inheritdoc />
        public TypographicPageViewModel GetPageView(TypographicPDFPageSearch typographicPDFPageSearch, int userId = 0)
        {
            logger.LogInformation("GetPageView input {@typographicPDFPageSearch} userId: {@userId}", typographicPDFPageSearch, userId);

            TypographicPageViewModel? typographicPageViewModel = new();

            try
            {
                typographicPageViewModel = dbContext.TypographicPages
                                            .Include(x => x.TypographicPDF)
                                            .ThenInclude(x => x.UploadFile)
                                            .Include(x => x.TypographicResourceLocations)
                                            .ThenInclude(x => x.TypographicResource)
                                            .AsSplitQuery()
                                            .Where(x => x.TypographicPDF.Id == typographicPDFPageSearch.Id && x.PageNumber == typographicPDFPageSearch.PageNumber)
                                            .ProjectTo<TypographicPageViewModel>(configurationProvider)
                                            .FirstOrDefault();

                if (typographicPageViewModel != null)
                {
                    //取得單頁PDF圖檔
                    PDFService pDFService = new()
                    {
                        PDFPath = typographicPageViewModel.PDFFullPath,
                        PageIndex = typographicPDFPageSearch.PageNumber,
                    };
                    PDFImageInfo pDFImageInfo = pDFService.GetPageImageInfo(PDFImageScaleConsts.Default);

                    typographicPageViewModel.PDFImageWidth = pDFImageInfo.Width;
                    typographicPageViewModel.PDFImageHeight = pDFImageInfo.Height;
                    typographicPageViewModel.PDFImageBase64 = pDFImageInfo.ImageBase64;
                    typographicPageViewModel.Success();
                }
                else
                {
                    typographicPageViewModel = new();
                    typographicPageViewModel.DbNoData();
                }
                logger.LogInformation("GetPageView output {@output}", mapper.Map<TypographicPageViewModel>(typographicPageViewModel));
            }
            catch (Exception ex)
            {
                typographicPageViewModel = new();
                typographicPageViewModel.Error();
                logger.LogInformation("GetPageView error {@error}", ex.Message);
            }            
            return typographicPageViewModel;
        }

        ///<inheritdoc />
        public TypographicPDFSettingViewModel GetTypographicPDFSummary(int typographicPDFId, int userId = 0)
        {
            logger.LogInformation("GetTypographicPDFSummary typographicPDFId: {@typographicPDFId} userId: {@userId}}"
                                , typographicPDFId, userId);

            TypographicPDFSettingViewModel? typographicPDFSettingViewModel;

            try
            {
                typographicPDFSettingViewModel = dbContext.TypographicPDFs
                                            .Include(x => x.UploadFile)
                                            .Include(x => x.QuarterYear)
                                            .Include(x => x.TypographicPages)
                                            .Where(x => x.Id == typographicPDFId)
                                            .ProjectTo<TypographicPDFSettingViewModel>(configurationProvider)
                                            .FirstOrDefault();

                if (typographicPDFSettingViewModel != null)
                {
                    typographicPDFSettingViewModel.Success();
                }
                else
                {
                    typographicPDFSettingViewModel = new();
                    typographicPDFSettingViewModel.DbNoData();
                }
                logger.LogInformation("GetTypographicPDFSummary output {@output}", typographicPDFSettingViewModel);
            }
            catch (Exception ex)
            {
                typographicPDFSettingViewModel = new();
                typographicPDFSettingViewModel.Error();
                logger.LogInformation("GetTypographicPDFSummary error {@error}", ex.Message);
            }            
            return typographicPDFSettingViewModel;
        }

        ///<inheritdoc />
        public TypographicPDFEditViewResponse GetEditPDFView(int typographicPDFId, int userId = 0)
        {
            logger.LogInformation("GetEditPDFView input typographicPDFId: {@typographicPDFId} userId: {@userId}", typographicPDFId, userId);

            TypographicPDFEditViewResponse typographicPDFEditViewResponse = new();

            try
            {
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
                logger.LogInformation("GetEditPDFView output {@output}", mapper.Map<TypographicPDFEditViewResponse>(typographicPDFEditViewResponse));
            }
            catch (Exception ex)
            {
                typographicPDFEditViewResponse.Error();
                logger.LogError("GetEditPDFView error {@error}", ex.Message);
            }
            return typographicPDFEditViewResponse;
        }

        ///<inheritdoc />
        public TypographicPDFMakeResponse MakeTyporaphicPDF(TypographicPDFMakeSetting typographicPDFMakeSetting, int userId = 0)
        {
            logger.LogInformation("MakeTyporaphicPDF input {@typographicPDFMakeSetting} userId: {@userId}", typographicPDFMakeSetting, userId);

            TypographicPDFMakeResponse typographicPagePDFResponse = new ();  
            
            try
            {
                //取得排版的頁面印鑑與座標
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
                    typographicPagePDFResponse.DbNoData();
                }
                logger.LogInformation("MakeTyporaphicPDF output {@output}", mapper.Map<TypographicPDFMakeResponse>(typographicPagePDFResponse));
            }
            catch (Exception ex)
            {
                typographicPagePDFResponse.Error();
                logger.LogError("MakeTyporaphicPDF error {@error}", ex.Message);
            }            
            return typographicPagePDFResponse;
        }

        ///<inheritdoc />
        public TypographicPDFNewResronse New(TypographicPDFForm typographicPDFForm, TypographyType typographyType, int userId = 0)
        {
            logger.LogInformation("New input {@Input} typographyType: {@typographyType} userId: {@userId}", typographicPDFForm, typographyType, userId);

            TypographicPDFNewResronse typographicPDFNewResronse = new();            
            
            try
            {
                UploadFile? pDFInfo = dbContext.UploadFiles.Find(typographicPDFForm.UploadId);
                Customer? customer = dbContext.Customers.Find(typographicPDFForm.CustomerId);
                if (pDFInfo != null && customer != null)
                {
                    List<TypographicPage> typographicPages = new();

                    //之後輸入要從前端提供Id
                    QuarterYear quarter = dbContext.QuarterYears.Single(x => x.Id == typographicPDFForm.QuarterYearId);

                    //新增PDF排版
                    TypographicPDF typographicPDF = new()
                    {
                        Customer = customer,
                        UploadFile = pDFInfo,
                        OriginFileName = pDFInfo.OriginalFileName,
                        FullPath = pDFInfo.FullPath,
                        QuarterYear = quarter,
                        TypographyType = typographyType,
                        TypographicPages = new List<TypographicPage>()
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
                logger.LogInformation("New output {@output}", typographicPDFNewResronse);
            }
            catch (Exception ex)
            {
                typographicPDFNewResronse.Error();
                logger.LogError("New error {@error}", ex.Message);
            }            
            return typographicPDFNewResronse;
        }

        ///<inheritdoc />
        public ResponseViewModel Save(TypographicPDFSaveForm typographicPDFSaveForm, int userId = 0)
        {
            logger.LogInformation("New input {@typographicPDFSaveForm} userId: {@userId}", typographicPDFSaveForm, userId);

            ResponseViewModel response = new();
            
            try
            {
                TypographicPDF? typographicPDF = dbContext.TypographicPDFs
                                                .Include(x => x.TypographicPages)
                                                .FirstOrDefault(x => x.Id == typographicPDFSaveForm.TypographicPDFId);

                if (typographicPDF != null)
                {
                    typographicPDF.Customer = dbContext.Customers.Single(x => x.Id == typographicPDFSaveForm.CustomerId);
                    typographicPDF.UploadFile = dbContext.UploadFiles.Single(x => x.Id == typographicPDFSaveForm.UploadId);
                    //之後輸入要從前端提供Id
                    typographicPDF.QuarterYear = dbContext.QuarterYears
                                            .Single(x => x.Id == typographicPDFSaveForm.QuarterYearId);

                    typographicPDF.ReviewStatus = ReviewStatus.Draft;
                    BaseInputTypographicPDF(typographicPDF, false, userId);
                    List<TypographicPage> newPages = new();

                    foreach (TypographicPageForm typographicPageForm in typographicPDFSaveForm.Pages)
                    {
                        newPages.Add(PageSave(typographicPageForm));
                    }
                    //由於Include(Pages)所以更換成newPages後會將舊的資料刪除
                    typographicPDF.TypographicPages = newPages;
                    dbContext.SaveChanges();
                    response.Success();
                }
                else
                {
                    response.DbNoData();
                }
                logger.LogInformation("New output {@output}", response);
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogError("New error {@error}", ex.Message);
            }
            return response;
        }

        ///<inheritdoc />
        public ResponseViewModel ChangeReviewStatus(int typographicPDFId, ReviewStatus reviewStatus, int userId = 0)
        {
            logger.LogInformation("ChangeReviewStatus input typographicPDFId: {@typographicPDFId} reviewStatus: {@reviewStatus} userId: {@userId}"
                , typographicPDFId, reviewStatus, userId);

            ResponseViewModel response = new();

            try
            {
                TypographicPDF? typographicPDF = dbContext.TypographicPDFs.Find(typographicPDFId);
                if (typographicPDF != null)
                {
                    typographicPDF.ReviewStatus = reviewStatus;
                    BaseInputTypographicPDF(typographicPDF, false, userId);
                    dbContext.SaveChanges();
                    response.Success();
                }
                else
                {
                    response.DbNoData();
                }
                logger.LogInformation("ChangeReviewStatus output {@output}", response);
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogError("ChangeReviewStatus error {@error}", ex.Message);
            }
            return response;
        }

        ///<inheritdoc />
        public ResponseViewModel Delete(int typographicPDFId, int userId = 0)
        {
            logger.LogInformation("Delete input typographicPDFId: {@typographicPDFId} userId: {@userId}"
                , typographicPDFId, userId);

            ResponseViewModel response = new();

            try
            {
                TypographicPDF? typographicPDF = dbContext.TypographicPDFs.Find(typographicPDFId);

                if (typographicPDF != null)
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
                logger.LogInformation("Delete output {@output}", response);                
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogError("Delete error {@error}", ex.Message);
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

            if(pageFrom.AccountantCertificateId != 0)
            {                
                typographicPage.UploadFile = dbContext.UploadFiles.Find(pageFrom.AccountantCertificateId);
            }
            
            //客戶印鑑座標
            foreach (CustomerSealLocationForm customerSealLocationForm in pageFrom.CustomerSealLocations)
            {
                TypographicResourceLocation typographicResourceLocation = mapper.Map<TypographicResourceLocation>(customerSealLocationForm);
                typographicResourceLocation.TypographicResource = GetTypographicResource(customerSealLocationForm.Id);
                typographicResourceLocations.Add(typographicResourceLocation);
            }

            //會計師簽印座標
            foreach (AccountantSignLocationForm accountantSignLocationForm in pageFrom.AccountantSignLocations)
            {
                TypographicResourceLocation typographicResourceLocation = mapper.Map<TypographicResourceLocation>(accountantSignLocationForm);
                typographicResourceLocation.TypographicResource = GetTypographicResource(accountantSignLocationForm.Id);
                typographicResourceLocations.Add(typographicResourceLocation);
            }

            //信頭座標
            foreach (LetterheadImageLocationForm letterheadImageLocationForm in pageFrom.LetterheadImageLocations)
            {
                TypographicResourceLocation typographicResourceLocation = mapper.Map<TypographicResourceLocation>(letterheadImageLocationForm);
                typographicResourceLocation.TypographicResource = GetTypographicResource(letterheadImageLocationForm.Id);
                typographicResourceLocations.Add(typographicResourceLocation);
            }

            //信頭座標
            foreach (TemporarySealLocationForm temporarySealLocationForm in pageFrom.TemporarySealLocations)
            {
                TypographicResourceLocation typographicResourceLocation = mapper.Map<TypographicResourceLocation>(temporarySealLocationForm);
                typographicResourceLocation.TypographicResource = GetTypographicResource(temporarySealLocationForm.Id);
                typographicResourceLocations.Add(typographicResourceLocation);
            }
            typographicPage.TypographicResourceLocations = typographicResourceLocations;

            return typographicPage;
        }

        /// <summary>
        /// 取得typographicResource實體
        /// </summary>
        /// <param name="typographicResourceId"></param>
        /// <returns></returns>
        private TypographicResource GetTypographicResource(int typographicResourceId)
        {
            return dbContext.TypographicResources.Single(x => x.Id == typographicResourceId);
        }
    }
}
