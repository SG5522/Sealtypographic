using AutoMapper;
using SealTypographicWebAPI.Models.TypographicPDF;
using DJSpire.Models;
using DBEntities.Consts;
using SealTypographicWebAPI.Utils;
using DJLib;
using DBEntities.Entities.TypographicModels;

namespace SealTypographicWebAPI.Config.MapperProfile
{
    /// <summary>
    /// AutoMapper用的LIST
    /// </summary>
    public class TypographicPDFMapperProfile : Profile
    {
        /// <summary>
        /// 建置
        /// </summary>
        public TypographicPDFMapperProfile()
        {
            //分頁處理的Map
            CreateMap<TypographicPDF, TypographicPDFViewModel>()
                 .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dst => dst.CustomerCode, opt => opt.MapFrom(src => src.Customer.Code))
                 .ForMember(dst => dst.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
                 .ForMember(dst => dst.OriginFileName, opt => opt.MapFrom(src => src.UploadFile.OriginalFileName))
                 .ForMember(dst => dst.QuarterYearId, opt => opt.MapFrom(src => src.QuarterYear.Id))
                 .ForMember(dst => dst.ReviewStatus, opt => opt.MapFrom(src => src.ReviewStatus));

            //PDF排版資訊
            CreateMap<TypographicPDFForm, TypographicPDF>()
                .ForMember(dst => dst.FullPath, opt => opt.Ignore());

            CreateMap<TypographicPageForm, TypographicPage>();

            CreateMap<PDFViewModel, PDFViewModel>()
                    .ForMember(dst => dst.ImageBase64, opt => opt.Ignore());

            CreateMap<TypographicPDF, TypographicPagesResponse>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.UploadId, opt => opt.MapFrom(src => src.UploadFile.Id))
                    .ForMember(dst => dst.CustomerId, opt => opt.MapFrom(src => src.Customer.Id))
                    .ForMember(dst => dst.QuarterYearId, opt => opt.MapFrom(src => src.QuarterYear.Id))
                    .ForMember(dst => dst.Pages, opt => opt.MapFrom(src => src.TypographicPages));

            CreateMap<TypographicPage, TypographicPageForm>()
                    .ForMember(dst => dst.AccountantCertificateId, opt => opt.MapFrom(src => src.UploadFile != null ? src.UploadFile.Id : 0))
                    .ForMember(dst => dst.CustomerSealLocations, opt =>
                        opt.MapFrom(src => src.TypographicResourceLocations.Where(x => x.TypographicResource.SealType == SealType.Customer)))
                    .ForMember(dst => dst.AccountantSignLocations, opt =>
                        opt.MapFrom(src => src.TypographicResourceLocations.Where(x => x.TypographicResource.SealType == SealType.Accountant)))
                    .ForMember(dst => dst.LetterheadImageLocations, opt =>
                        opt.MapFrom(src => src.TypographicResourceLocations.Where(x => x.TypographicResource.SealType == SealType.Letterhead)))
                    .ForMember(dst => dst.TemporarySealLocations, opt =>
                        opt.MapFrom(src => src.TypographicResourceLocations.Where(x => x.TypographicResource.SealType == SealType.TemporarySeal)));

            CreateMap<TypographicResourceLocation, CustomerSealLocationForm>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.TypographicResource.Id));
            CreateMap<TypographicResourceLocation, AccountantSignLocationForm>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.TypographicResource.Id));
            CreateMap<TypographicResourceLocation, LetterheadImageLocationForm>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.TypographicResource.Id));
            CreateMap<TypographicResourceLocation, TemporarySealLocationForm>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.TypographicResource.Id));

            CreateMap<CustomerSealLocationForm, TypographicResourceLocation>()
                    .ForMember(dst => dst.Id, opt => opt.Ignore());
            CreateMap<AccountantSignLocationForm, TypographicResourceLocation>()
                    .ForMember(dst => dst.Id, opt => opt.Ignore()); //此ID非為ResourceLocation的ID而是關聯用的ID                    
            CreateMap<LetterheadImageLocationForm, TypographicResourceLocation>()
                    .ForMember(dst => dst.Id, opt => opt.Ignore()); //此ID非為ResourceLocation的ID而是關聯用的ID                    
            CreateMap<TemporarySealLocationForm, TypographicResourceLocation>()
                    .ForMember(dst => dst.Id, opt => opt.Ignore()); //此ID非為ResourceLocation的ID而是關聯用的ID

            //單頁詳細資料(PDFPage)
            CreateMap<TypographicPage, TypographicPageViewModel>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.TypographicPDF.Id))
                    .ForMember(dst => dst.PDFFullPath, opt => opt.MapFrom(src => src.TypographicPDF.UploadFile.FullPath))
                    .ForMember(dst => dst.CustomerSealLocationViewModels, opt =>
                        opt.MapFrom(src => src.TypographicResourceLocations.Where(x => x.TypographicResource.SealType == SealType.Customer)))
                    .ForMember(dst => dst.AccountantSignLocationViewModels, opt =>
                        opt.MapFrom(src => src.TypographicResourceLocations.Where(x => x.TypographicResource.SealType == SealType.Accountant)))
                    .ForMember(dst => dst.LetterheadImageLocationViewModels, opt =>
                        opt.MapFrom(src => src.TypographicResourceLocations.Where(x => x.TypographicResource.SealType == SealType.Letterhead)))
                    .ForMember(dst => dst.TemporarySealLocationViewModels, opt =>
                        opt.MapFrom(src => src.TypographicResourceLocations.Where(x => x.TypographicResource.SealType == SealType.TemporarySeal)));

            //單頁詳細資料(Log)
            CreateMap<TypographicPageViewModel, TypographicPageViewModel>()
                    .ForMember(dst => dst.PDFImageBase64, opt => opt.Ignore());

            //各印鑑基本資料
            //客戶印鑑
            CreateMap<TypographicResourceLocation, CustomerSealLocationViewModel>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.TypographicResource.Id))
                    .ForMember(dst => dst.Sequence, y => y.MapFrom(o => o.TypographicResource.Sequence))
                    .ForMember(dst => dst.CustomerSealType, y => y.MapFrom(o => SealMappingConfigUtil.GetCustomerSealType(o.TypographicResource.SubSealType)))
                    .ForMember(dst => dst.ImageBase64, y => y.MapFrom(o => ImageSharpUtil.TransparentToImageBase64(o.TypographicResource.ImageFullPath)));

            //客戶印鑑(log)
            CreateMap<CustomerSealLocationViewModel, CustomerSealLocationViewModel>()
                    .ForMember(dst => dst.ImageBase64, opt => opt.Ignore());

            //會計師簽印
            CreateMap<TypographicResourceLocation, AccountantSignLocationViewModel>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.TypographicResource.Id))
                    .ForMember(dst => dst.AccountantName, opt => opt.MapFrom
                    (
                        src => src.TypographicResource.AccountantSignGroup != null ?
                        src.TypographicResource.AccountantSignGroup.Accountant.Name : null
                    ))
                    .ForMember(dst => dst.AccountantSignType, y => y.MapFrom(o => SealMappingConfigUtil.GetAccountantSignType(o.TypographicResource.SubSealType)))
                    .ForMember(dst => dst.ImageBase64, y => y.MapFrom(o => ImageSharpUtil.TransparentToImageBase64(o.TypographicResource.ImageFullPath)));

            //會計師簽印(log)
            CreateMap<AccountantSignLocationViewModel, AccountantSignLocationViewModel>()
                    .ForMember(dst => dst.ImageBase64, opt => opt.Ignore());

            //信頭圖
            CreateMap<TypographicResourceLocation, LetterheadImageLocationViewModel>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.TypographicResource.Id))
                    .ForMember(dst => dst.ImageBase64, opt => opt.MapFrom(src => ImageSharpUtil.TransparentToImageBase64(src.TypographicResource.ImageFullPath)))
                    .ForMember(dst => dst.LetterheadName, opt => opt.MapFrom
                    (
                        src => src.TypographicResource.Letterhead != null ?
                        src.TypographicResource.Letterhead.Name : null
                    ));

            //信頭圖(log)
            CreateMap<LetterheadImageLocationViewModel, LetterheadImageLocationViewModel>()
                    .ForMember(dst => dst.ImageBase64, opt => opt.Ignore());

            //臨時章
            CreateMap<TypographicResourceLocation, TemporarySealLocationViewModel>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.TypographicResource.Id))
                    .ForMember(dst => dst.Sequence, opt => opt.MapFrom(src => src.TypographicResource.Sequence))
                    .ForMember(dst => dst.ImageBase64, opt => opt.MapFrom(src => ImageSharpUtil.TransparentToImageBase64(src.TypographicResource.ImageFullPath)));

            //臨時章(log)
            CreateMap<TemporarySealLocationViewModel, TemporarySealLocationViewModel>()
                    .ForMember(dst => dst.ImageBase64, opt => opt.Ignore());

            //印鑑與簽印複製使用
            CreateMap<TypographicResource, TypographicResource>()
                    .ForMember(dst => dst.CustomerSealGroup, y => y.Ignore())
                    .ForMember(dst => dst.AccountantSignGroup, y => y.Ignore())
                    .ForMember(dst => dst.Letterhead, y => y.Ignore())
                    .ForMember(dst => dst.TemporarySealGroup, y => y.Ignore())
                    .ForMember(dst => dst.Id, y => y.Ignore());

            //讀取PDF概要內容的Map
            CreateMap<TypographicPDF, TypographicPDFSettingViewModel>()
                 .ForMember(dst => dst.OriginalFileName, y => y.MapFrom(o => o.OriginFileName))
                 .ForMember(dst => dst.QuarterYearId, opt => opt.MapFrom(src => src.QuarterYear.Id))
                 .ForMember(dst => dst.EditPageCount, opt => opt.MapFrom(o => o.TypographicPages.Where(src => src.TypographicResourceLocations.Any()).Count()))
                 .ForMember(dst => dst.BlankPageCount, opt => opt.MapFrom(o => o.TypographicPages.Where(src => src.BlankCheck == true).Count()))
                 .ForMember(dst => dst.DefaultPdfFileName, opt => opt.MapFrom(src => new string(PdfOutputUtil.GetName(src.Customer.Code, src.QuarterYear))));

            //PDF該頁的編輯內容的Map
            CreateMap<TypographicPage, EditPage>()
                 .ForMember(dst => dst.AccountantCertificatePath, opt => opt.MapFrom(src => src.UploadFile != null ? src.UploadFile.FullPath : string.Empty))
                 .ForMember(dst => dst.EditImages, y => y.MapFrom(o => o.TypographicResourceLocations));
            // PDF排版圖像與位置的Map
            CreateMap<TypographicResourceLocation, EditImage>()
                //透通處理                
                .ForMember(dst => dst.ImageBase64, opt => opt.MapFrom(src => Convert.ToBase64String(OpenCvUtil.TransparentToBytes(src.TypographicResource.ImageFullPath, 160))))
                //確認圖像種類決定縮放大小
                .ForMember(dst => dst.ImageScale, opt => opt.MapFrom(src => PdfOutputUtil.GetImageScale(src.TypographicResource.SubSealType)));

            // PDF排版圖像紀錄處理
            CreateMap<TypographicPDFEditViewResponse, TypographicPDFEditViewResponse>()                
                .ForMember(dst => dst.PDFBase64, opt => opt.Ignore());
            
            // PDF排版圖像紀錄處理
            CreateMap<TypographicPDFMakeResponse, TypographicPDFMakeResponse>()
                .ForMember(dst => dst.PDFBase64, opt => opt.Ignore());
        }
    }
}
