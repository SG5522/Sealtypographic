using AutoMapper;
using SealTypographicWebAPI.Models.TypographicPDF;
using DBEntities.Consts;
using SealTypographicWebAPI.Utils;
using DBEntities.Entities.TypographicModels;
using SealTypographicWebAPI.Models.EditPdf;
using SealTypographicWebAPI.Models.TypographicPDF.EditViewModels;

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
                .ForMember(dst => dst.PdfEditStep, opt => opt.MapFrom(src => src.PdfEditStep))
                .ForMember(dst => dst.Pages, opt => opt.MapFrom(src => src.TypographicPages));

            CreateMap<TypographicResourceLocation, CustomerSealEditLocation>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.TypographicResource.Id));
            CreateMap<TypographicResourceLocation, AccountantSignEditLocation>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.TypographicResource.Id));
            CreateMap<TypographicResourceLocation, LetterheadImageEditLocation>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.TypographicResource.Id));
            CreateMap<TypographicResourceLocation, TemporarySealEditLocation>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.TypographicResource.Id));

            CreateMap<TypographicPage, TypographicEditPage>()
                    .ForMember(dst => dst.AccountantCertificateId, opt => opt.MapFrom(src => src.AccountantCertificateFile != null ? src.AccountantCertificateFile.Id : 0))
                    .ForMember(dst => dst.CustomerSealLocations, opt =>
                        opt.MapFrom(src => src.TypographicResourceLocations.Where(x => x.TypographicResource.SealType == SealType.Customer)))
                    .ForMember(dst => dst.AccountantSignLocations, opt =>
                        opt.MapFrom(src => src.TypographicResourceLocations.Where(x => x.TypographicResource.SealType == SealType.Accountant)))
                    .ForMember(dst => dst.LetterheadImageLocations, opt =>
                        opt.MapFrom(src => src.TypographicResourceLocations.Where(x => x.TypographicResource.SealType == SealType.Letterhead)))
                    .ForMember(dst => dst.TemporarySealLocations, opt =>
                        opt.MapFrom(src => src.TypographicResourceLocations.Where(x => x.TypographicResource.SealType == SealType.TemporarySeal)));

            CreateMap<TypographicPage, TypographicPageForm>()
                    .ForMember(dst => dst.AccountantCertificateId, opt => opt.MapFrom(src => src.AccountantCertificateFile != null ? src.AccountantCertificateFile.Id : 0))
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

            //log使用
            CreateMap<CustomerSealLocationForm, CustomerSealLocationForm>()
                    .ForMember(dst => dst.EditPdfImageBase64, opt => opt.Ignore());
            CreateMap<AccountantSignLocationForm, AccountantSignLocationForm>()
                    .ForMember(dst => dst.EditPdfImageBase64, opt => opt.Ignore());
            CreateMap<LetterheadImageLocationForm, LetterheadImageLocationForm>()
                    .ForMember(dst => dst.EditPdfImageBase64, opt => opt.Ignore());
            CreateMap<TemporarySealLocationForm, TemporarySealLocationForm>()
                    .ForMember(dst => dst.EditPdfImageBase64, opt => opt.Ignore());

            //單頁詳細資料(PDFPage)
            CreateMap<TypographicPage, TypographicPageViewModel>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.TypographicPDF.Id))
                    .ForMember(dst => dst.PDFFullPath, opt => opt.MapFrom(src => src.TypographicPDF.UploadFile.FullPath))
                    .ForMember(dst => dst.EncryptKey, opt => opt.MapFrom(src => src.TypographicPDF.UploadFile.EncryptKey))
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
                    .ForMember(dst => dst.Sequence, opt => opt.MapFrom(src => src.TypographicResource.Sequence))
                    .ForMember(dst => dst.CustomerSealType, opt => opt.MapFrom(src => SealMappingConfigUtil.GetCustomerSealType(src.TypographicResource.SubSealType)))
                    .ForMember(dst => dst.ImagePath, opt => opt.MapFrom(src => TypographicResourceLocationUtil.GetImagePath(src)))
                    .ForMember(dst => dst.EncryptKey, opt => opt.MapFrom(src => TypographicResourceLocationUtil.GetEncryptKey(src)));

            //客戶印鑑(log)
            CreateMap<CustomerSealLocationViewModel, CustomerSealLocationViewModel>()
                    .ForMember(dst => dst.ImageBase64, opt => opt.Ignore())
                    .ForMember(dst => dst.EncryptKey, opt => opt.Ignore());

            //會計師簽印
            CreateMap<TypographicResourceLocation, AccountantSignLocationViewModel>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.TypographicResource.Id))
                    .ForMember(dst => dst.AccountantName, opt => opt.MapFrom
                    (
                        src => src.TypographicResource.AccountantSignGroup != null ?
                        src.TypographicResource.AccountantSignGroup.Accountant.Name : null
                    ))
                    .ForMember(dst => dst.AccountantSignType, opt => opt.MapFrom(src => SealMappingConfigUtil.GetAccountantSignType(src.TypographicResource.SubSealType)))
                    .ForMember(dst => dst.ImagePath, opt => opt.MapFrom(src => TypographicResourceLocationUtil.GetImagePath(src)))
                    .ForMember(dst => dst.EncryptKey, opt => opt.MapFrom(src => TypographicResourceLocationUtil.GetEncryptKey(src)));

            //會計師簽印(log)
            CreateMap<AccountantSignLocationViewModel, AccountantSignLocationViewModel>()
                    .ForMember(dst => dst.ImageBase64, opt => opt.Ignore())
                    .ForMember(dst => dst.EncryptKey, opt => opt.Ignore());

            //信頭圖
            CreateMap<TypographicResourceLocation, LetterheadImageLocationViewModel>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.TypographicResource.Id))
                    .ForMember(dst => dst.LetterheadName, opt => opt.MapFrom(src =>
                        src.TypographicResource.Letterhead != null ? src.TypographicResource.Letterhead.Name : null))
                    .ForMember(dst => dst.ImagePath, opt => opt.MapFrom(src => TypographicResourceLocationUtil.GetImagePath(src)))
                    .ForMember(dst => dst.ImageBase64, opt => opt.MapFrom(src => TypographicResourceLocationUtil.GetImageBase64(src)));

            //信頭圖(log)
            CreateMap<LetterheadImageLocationViewModel, LetterheadImageLocationViewModel>()
                    .ForMember(dst => dst.ImageBase64, opt => opt.Ignore())
                    .ForMember(dst => dst.EncryptKey, opt => opt.Ignore());

            //臨時章
            CreateMap<TypographicResourceLocation, TemporarySealLocationViewModel>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.TypographicResource.Id))
                    .ForMember(dst => dst.Sequence, opt => opt.MapFrom(src => src.TypographicResource.Sequence))
                    .ForMember(dst => dst.ImagePath, opt => opt.MapFrom(src => TypographicResourceLocationUtil.GetImagePath(src)))
                    .ForMember(dst => dst.EncryptKey, opt => opt.MapFrom(src => TypographicResourceLocationUtil.GetEncryptKey(src)));

            //臨時章(log)
            CreateMap<TemporarySealLocationViewModel, TemporarySealLocationViewModel>()
                    .ForMember(dst => dst.ImageBase64, opt => opt.Ignore())
                    .ForMember(dst => dst.EncryptKey, opt => opt.Ignore());

            //印鑑與簽印複製使用
            CreateMap<TypographicResource, TypographicResource>()
                    .ForMember(dst => dst.CustomerSealGroup, y => y.Ignore())
                    .ForMember(dst => dst.AccountantSignGroup, y => y.Ignore())
                    .ForMember(dst => dst.Letterhead, y => y.Ignore())
                    .ForMember(dst => dst.TemporarySealGroup, y => y.Ignore())
                    .ForMember(dst => dst.Id, y => y.Ignore());

            //讀取PDF概要內容的Map
            CreateMap<TypographicPDF, TypographicPDFSettingViewModel>()
                 .ForMember(dst => dst.OriginalFileName, opt => opt.MapFrom(o => o.OriginFileName))
                 .ForMember(dst => dst.QuarterYearId, opt => opt.MapFrom(src => src.QuarterYear.Id))
                 .ForMember(dst => dst.EditPageCount, opt => opt.MapFrom(o => o.TypographicPages.Where(src => src.TypographicResourceLocations.Any()).Count()))
                 .ForMember(dst => dst.BlankPageCount, opt => opt.MapFrom(o => o.TypographicPages.Where(src => src.BlankCheck == true).Count()))
                 .ForMember(dst => dst.DefaultPdfFileName, opt => opt.MapFrom(src => new string($"{src.Customer.Code}{QuarterUtil.GetTaiwanYearQuarter(src.QuarterYear)}")));

            //TypographicPDF的Map
            CreateMap<TypographicPDF, EditPDF>()
                .ForMember(dst => dst.PdfPath, opt => opt.MapFrom(o => o.UploadFile.FullPath))
                .ForMember(dst => dst.EditPages, opt => opt.MapFrom(src => src.TypographicPages));

            //PDF該頁的編輯內容的Map
            CreateMap<TypographicPage, EditPage>()
                 .ForMember(dst => dst.AccountantCertificatePath, opt => opt.MapFrom(src => src.AccountantCertificateFile != null ? src.AccountantCertificateFile.FullPath : string.Empty))
                 .ForMember(dst => dst.EditImages, opt => opt.MapFrom(src => src.TypographicResourceLocations));

            // PDF排版圖像與位置的Map
            CreateMap<TypographicResourceLocation, EditImage>()
                //.ForMember(dst => dst.ImageBase64, opt => opt.MapFrom(src => TypographicResourceLocationUtil.GetImageBase64(src)));
                .ForMember(dst => dst.ImageBase64, opt => opt.Ignore())
                .ForMember(dst => dst.ImageFullPath, opt => opt.MapFrom(src => src.EditImageFullPath ?? src.TypographicResource.ImageFullPath))
                .ForMember(dst => dst.ImageEncryptKey, opt => opt.MapFrom(src => src.EditImageEncryptKey ?? src.TypographicResource.ImageEncryptKey));

            // PDF排版圖像紀錄處理
            CreateMap<TypographicPDFEditViewResponse, TypographicPDFEditViewResponse>()
                .ForMember(dst => dst.PDFBase64, opt => opt.Ignore());

            // PDF排版圖像紀錄處理
            CreateMap<TypographicPDFMakeResponse, TypographicPDFMakeResponse>()
                .ForMember(dst => dst.PDFBase64, opt => opt.Ignore());
        }
    }
}
