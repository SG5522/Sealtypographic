using AutoMapper;
using SealTypographicWebAPI.Models.TypographicPDF;
using SealTypographicWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using DBEntities.Consts;
using SealTypographicWebAPI.Utils;
using AutoMapper.QueryableExtensions;
using SealTypographicWebAPI.Consts;
using DBEntities.Entities;
using DBEntities;
using DBEntities.Entities.TypographicModels;
using DBEntities.Entities.CustomerModels;
using SealTypographicWebAPI.Models.EditPdf;
using DJSpire.Utils;
using DJSpire.Models;
using SealTypographicWebAPI.Models.TypographicPDF.EditViewModels;
using SealTypographicWebAPI.Config;
using Microsoft.Extensions.Options;
using DJImageLib.Models;
using CommonLib.Utils;
using DJImageLib.Extensions;
using DBEntities.Utils;
using CommonLib.Extensions;
using Microsoft.OpenApi.Extensions;

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
        private TypographyEditImagePathOptions typographyEditImagePathOptions;

        /// <summary>
        /// 取得DB與Automapper
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="typographyEditImagePathOptionsMonitor"></param>        
        public TypographicPDFService(SealTypographicDbContext dbContext, ILogger<TypographicPDFService> logger, IMapper mapper,
            IOptionsMonitor<TypographyEditImagePathOptions> typographyEditImagePathOptionsMonitor)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;            
            configurationProvider = mapper.ConfigurationProvider;
            typographyEditImagePathOptions = typographyEditImagePathOptionsMonitor.CurrentValue;

            typographyEditImagePathOptionsMonitor.OnChange(options =>
            {
                typographyEditImagePathOptions = options;
            });
        }

        ///<inheritdoc />
        public async Task<TypographicPDFPaginateViewModel> GetPaginate(TypographicPDFSearch typographicPDFSearch, TypographyType typographyType, int userId = 1)
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
                    typographicPDFPaginateViewModel.ViewModels = await PageUtil.SetPaginateViewModelAsync<TypographicPDF, TypographicPDFViewModel>(
                                                                    typographicPDFs,
                                                                    configurationProvider,
                                                                    typographicPDFSearch.PageNumber,
                                                                    typographicPDFSearch.PageSize
                                                                );

                    PageUtil.SetPaginate(typographicPDFPaginateViewModel, typographicPDFSearch.PageNumber, typographicPDFSearch.PageSize, typographicPDFs.Count());
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
        public async Task<TypographicPagesResponse> GetEditPages(int id, int userId = 1)
        {
            logger.LogInformation("GetPaginate input id: {@id} userId: {@userId}", id, userId);

            TypographicPagesResponse? typographicPagesResponse ;

            try
            {
                typographicPagesResponse = await dbContext.TypographicPDFs
                                            .Include(x => x.TypographicPages)
                                            .ThenInclude(x => x.TypographicResourceLocations)
                                            .Where(x => x.Id == id)
                                            .ProjectTo<TypographicPagesResponse>(configurationProvider)
                                            .FirstOrDefaultAsync();

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
        public async Task<PDFViewModel> GetPDFView(int uploadFileid, int pageNumber, int userId = 1)
        {
            logger.LogInformation("GetPDFView input uploadFileid: {@uploadFileid} pageNumber: {@pageNumber} userId: {@userId}"
                                    , uploadFileid, pageNumber, userId);

            PDFViewModel pDFViewModel = new ();

            try
            {
                string? uploadPath = await dbContext.UploadFiles.Where(x => x.Id == uploadFileid).Select(x => x.FullPath).FirstOrDefaultAsync();
                if (uploadPath != null)
                {
                    //取得單頁PDF圖檔資訊
                    PdfPageImageInfo pdfPageImageInfo = PdfImageUtil.GetPdfPageImageInfo(uploadPath, pageNumber);

                    pDFViewModel.PDFFullPath = uploadPath; //Log使用
                    pDFViewModel.TotalPage = pdfPageImageInfo.TotalPage;
                    pDFViewModel.ImageWidth = pdfPageImageInfo.Width;
                    pDFViewModel.ImageHeight = pdfPageImageInfo.Height;
                    pDFViewModel.ImageBase64 = pdfPageImageInfo.ImageDataUrl;
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
        public async Task<TypographicPageViewModel> GetPageView(TypographicPDFPageSearch typographicPDFPageSearch, int userId = 1)
        {
            logger.LogInformation("GetPageView input {@typographicPDFPageSearch} userId: {@userId}", typographicPDFPageSearch, userId);

            TypographicPageViewModel? typographicPageViewModel = new();

            try
            {
                typographicPageViewModel = await dbContext.TypographicPages
                                            .Include(x => x.TypographicPDF)
                                            .ThenInclude(x => x.UploadFile)
                                            .Include(x => x.TypographicResourceLocations)
                                            .ThenInclude(x => x.TypographicResource)                                            
                                            .Where(x => x.TypographicPDF.Id == typographicPDFPageSearch.Id && x.PageNumber == typographicPDFPageSearch.PageNumber)
                                            .ProjectTo<TypographicPageViewModel>(configurationProvider)
                                            .FirstOrDefaultAsync();

                if (typographicPageViewModel != null)
                {
                    //取得單頁PDF圖檔資訊                    
                    PdfPageImageInfo pdfPageImageInfo = PdfImageUtil.GetPdfPageImageInfo(typographicPageViewModel.PDFFullPath, typographicPDFPageSearch.PageNumber);

                    typographicPageViewModel.PDFImageWidth = pdfPageImageInfo.Width;
                    typographicPageViewModel.PDFImageHeight = pdfPageImageInfo.Height;
                    typographicPageViewModel.PDFImageBase64 = pdfPageImageInfo.ImageDataUrl;
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
        public async Task<TypographicPDFSettingViewModel> GetTypographicPDFSummary(int typographicPDFId, int userId = 1)
        {
            logger.LogInformation("GetTypographicPDFSummary typographicPDFId: {@typographicPDFId} userId: {@userId}}"
                                , typographicPDFId, userId);

            TypographicPDFSettingViewModel? typographicPDFSettingViewModel;

            try
            {
                typographicPDFSettingViewModel = await dbContext.TypographicPDFs
                                                .Include(x => x.UploadFile)
                                                .Include(x => x.QuarterYear)
                                                .Include(x => x.TypographicPages)
                                                .Where(x => x.Id == typographicPDFId)
                                                .ProjectTo<TypographicPDFSettingViewModel>(configurationProvider)
                                                .FirstOrDefaultAsync();

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
        public async Task<TypographicPDFEditViewResponse> GetEditPDFView(int typographicPDFId, int userId = 1)
        {
            logger.LogInformation("GetEditPDFView input typographicPDFId: {@typographicPDFId} userId: {@userId}", typographicPDFId, userId);

            TypographicPDFEditViewResponse typographicPDFEditViewResponse = new();

            try
            {
                EditPDF? editPDF = await dbContext.TypographicPDFs
                                    .Include(x => x.TypographicPages)
                                    .ThenInclude(x => x.TypographicResourceLocations)
                                    .ThenInclude(x => x.TypographicResource)
                                    .Where(x => x.Id == typographicPDFId)
                                    .ProjectTo<EditPDF>(configurationProvider)
                                    .FirstOrDefaultAsync();

                if (editPDF != null)
                {
                    editPDF.PDFColor = PDFColor.Original;
                    editPDF.IsBlank = false;

                    typographicPDFEditViewResponse.PDFBase64 = EditPdfUitl.ToDataURL(editPDF, 300f);                    
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
        public async Task<TypographicPDFMakeResponse> MakeTyporaphicPDF(TypographicPDFMakeSetting typographicPDFMakeSetting, int userId = 1)
        {
            logger.LogInformation("MakeTyporaphicPDF input {@typographicPDFMakeSetting} userId: {@userId}", typographicPDFMakeSetting, userId);

            TypographicPDFMakeResponse typographicPagePDFResponse = new ();  
            
            try
            {
                //取得排版的頁面印鑑與座標
                EditPDF? editPDF = await dbContext.TypographicPDFs
                                    .Include(x => x.TypographicPages)
                                    .ThenInclude(x => x.TypographicResourceLocations)
                                    .ThenInclude(x => x.TypographicResource)
                                    .Where(x => x.Id == typographicPDFMakeSetting.TypographicPDFId)
                                    .ProjectTo<EditPDF>(configurationProvider)
                                    .FirstOrDefaultAsync();

                if (editPDF != null)
                {
                    editPDF.PDFColor = typographicPDFMakeSetting.PDFColor;
                    editPDF.IsBlank = typographicPDFMakeSetting.IsBlank;     
                    
                    typographicPagePDFResponse.PDFBase64 = EditPdfUitl.ToDataURL(editPDF, 300f);
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
        public PdfEditStepResponse GetPdfEditStep()
        {
            PdfEditStepResponse result = new ();

            try
            {
                foreach (PdfEditStep pdfEditStep in (PdfEditStep[])Enum.GetValues(typeof(PdfEditStep)))
                {
                    PdfEditStepViewModel pdfEditStepViewModel = new()
                    {
                        Id = (int)pdfEditStep,
                        Name = pdfEditStep.GetDisplayName(),
                        Description = pdfEditStep.GetDescription()
                    };
                    result.ViewModels.Add(pdfEditStepViewModel);
                }
            }
            catch (Exception ex) 
            {
                result.Error();
                logger.LogError("GetPdfEditStep error {@error}", ex.ToString());
            }
            return result;
        }

        ///<inheritdoc />
        public async Task<TypographicPDFNewResronse> New(TypographicPDFForm typographicPDFForm, TypographyType typographyType, int userId = 1)
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
                        PdfEditStep = PdfEditStep.CustomerSeal,
                        TypographicPages = new List<TypographicPage>()
                    };                    
                    InputUtil.SetWithReview(typographicPDF, true, userId);
                    foreach (TypographicPageForm pageInfo in typographicPDFForm.Pages)
                    {
                        typographicPDF.TypographicPages.Add(await PageSave(pageInfo));
                    }

                    dbContext.TypographicPDFs.Add(typographicPDF);
                    await dbContext.SaveChangesAsync();
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
        public async Task<ResponseViewModel> Save(TypographicPDFSaveForm typographicPDFSaveForm, int userId = 1)
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
                    //紀錄pdf現在的步驟
                    typographicPDF.PdfEditStep = typographicPDFSaveForm.PdfEditStep;

                    InputUtil.SetWithReview(typographicPDF, false, userId);

                    List<TypographicPage> newPages = new();

                    foreach (TypographicPageForm typographicPageForm in typographicPDFSaveForm.Pages)
                    {
                        newPages.Add(await PageSave(typographicPageForm));
                    }
                    //由於Include(Pages)所以更換成newPages後會將舊的資料刪除
                    typographicPDF.TypographicPages = newPages;
                    await dbContext.SaveChangesAsync();
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
        public async Task<ResponseViewModel> ChangeReviewStatus(int typographicPDFId, ReviewStatus reviewStatus, int userId = 1)
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
                    InputUtil.SetWithReview(typographicPDF, false, userId);
                    await dbContext.SaveChangesAsync();
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
        public async Task<ResponseViewModel> Delete(int typographicPDFId, int userId = 1)
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
                    InputUtil.SetWithReview(typographicPDF, false, userId);
                    await dbContext.SaveChangesAsync();
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
        /// PDF頁次存檔
        /// </summary>             
        /// <param name="pageFrom">來源頁次</param>        
        private async Task<TypographicPage> PageSave(TypographicPageForm pageFrom)
        {
            TypographicPage typographicPage = new();
            List<TypographicResourceLocation> typographicResourceLocations = new();

            typographicPage.PageNumber = pageFrom.PageNumber;
            typographicPage.BlankCheck = pageFrom.BlankCheck;
            typographicPage.DeleteCheck = pageFrom.DeleteCheck;

            if(pageFrom.AccountantCertificateId != 0)
            {                
                typographicPage.AccountantCertificateFile = dbContext.UploadFiles.Find(pageFrom.AccountantCertificateId);
            }
            
            //客戶印鑑座標
            foreach (CustomerSealLocationForm customerSealLocationForm in pageFrom.CustomerSealLocations)
            {
                await AddTypographicResourceLocation(typographicResourceLocations, customerSealLocationForm);
            }

            //會計師簽印座標
            foreach (AccountantSignLocationForm accountantSignLocationForm in pageFrom.AccountantSignLocations)
            {               
                await AddTypographicResourceLocation(typographicResourceLocations, accountantSignLocationForm);
            }

            //信頭座標
            foreach (LetterheadImageLocationForm letterheadImageLocationForm in pageFrom.LetterheadImageLocations)
            {
                await AddTypographicResourceLocation(typographicResourceLocations, letterheadImageLocationForm);
            }

            //信頭座標
            foreach (TemporarySealLocationForm temporarySealLocationForm in pageFrom.TemporarySealLocations)
            {
                await AddTypographicResourceLocation(typographicResourceLocations, temporarySealLocationForm);
            }
            typographicPage.TypographicResourceLocations = typographicResourceLocations;

            return typographicPage;
        }

        private async Task AddTypographicResourceLocation<T>(List<TypographicResourceLocation> typographicResourceLocations, T locationData)  where T : TypographicPDFBaseLocation
        {
            TypographicResourceLocation typographicResourceLocation = mapper.Map<TypographicResourceLocation>(locationData);
            typographicResourceLocation.TypographicResource = dbContext.TypographicResources.Single(x => x.Id == locationData.Id);
            if (!string.IsNullOrWhiteSpace(locationData.EditPdfImageBase64))
            {
                ImageModel imageModel = new() { DataUrl = locationData.EditPdfImageBase64 };
                string originalFileName = $"{DateTime.Now:yyyyMMddHHmmssffff}.{imageModel.ImageFormat!.Name.ToLower()}";
                //存到指定位置
                string savePath = await FileUtil.SaveFileReturnPath(imageModel.Base64!.ToBytes(), $"{typographyEditImagePathOptions.RootPath}{originalFileName}");
                typographicResourceLocation.EditImageFullPath = savePath;
            }
            typographicResourceLocations.Add(typographicResourceLocation);
        }
    }
}
