using AutoMapper;
using SealTypographicWebAPI.Models.TypographicPDF;
using DBEntities;
using DJSpire.Models;
using DJLib.Models;
using DBEntities.Consts;
using SealTypographicWebAPI.Utils;
using DJLib;

namespace SealTypographicWebAPI.Config
{
    /// <summary>
    /// AutoMapper用的LIST
    /// </summary>
    public class TypographicPDFServiceMapperProfile : Profile
    {        
        /// <summary>
        /// 建置
        /// </summary>
        public TypographicPDFServiceMapperProfile()
        {
            //PDF排版資訊
            CreateMap<TypographicPDFForm, TypographicPDF>()
                .ForMember(x => x.FullPath, y => y.Ignore());

            CreateMap<TypographicPageForm, TypographicPage>();

            CreateMap<PDFViewModel, PDFViewModel>()
                    .ForMember(x => x.ImageBase64, y => y.Ignore());

            CreateMap<TypographicPage, TypographicPageForm>()
                    .ForMember(dst => dst.CustomerSealLocations, opt =>
                        opt.MapFrom(src => src.TypographicResourceLocations.Where(x => x.TypographicResource.SealType == SealType.Customer)))
                    .ForMember(dst => dst.AccountantSignLocations, opt =>
                        opt.MapFrom(src => src.TypographicResourceLocations.Where(x => x.TypographicResource.SealType == SealType.Accountant)))
                    .ForMember(dst => dst.LetterheadImageLocations, opt =>
                        opt.MapFrom(src => src.TypographicResourceLocations.Where(x => x.TypographicResource.SealType == SealType.Letterhead)))
                    .ForMember(dst => dst.TemporarySealLocations, opt =>
                        opt.MapFrom(src => src.TypographicResourceLocations.Where(x => x.TypographicResource.SealType == SealType.TemporarySeal)));

            CreateMap<TypographicResourceLocation, CustomerSealLocationForm>();
            CreateMap<TypographicResourceLocation, AccountantSignLocationForm>();
            CreateMap<TypographicResourceLocation, LetterheadImageLocationForm>();
            CreateMap<TypographicResourceLocation, TemporarySealLocationForm>();             


            CreateMap<CustomerSealLocationForm, TypographicResourceLocation>()
                    .ForMember(x => x.Id, y => y.Ignore()); //此ID非為ResourceLocation的ID而是關聯用的ID                    
            CreateMap<AccountantSignLocationForm, TypographicResourceLocation>()
                    .ForMember(x => x.Id, y => y.Ignore()); //此ID非為ResourceLocation的ID而是關聯用的ID                    
            CreateMap<LetterheadImageLocationForm, TypographicResourceLocation>()
                    .ForMember(x => x.Id, y => y.Ignore()); //此ID非為ResourceLocation的ID而是關聯用的ID                    
            CreateMap<TemporarySealLocationForm, TypographicResourceLocation>()
                    .ForMember(x => x.Id, y => y.Ignore()); //此ID非為ResourceLocation的ID而是關聯用的ID
                    

            CreateMap<TypographicPage, TypographicPageViewModel>()
                    .ForMember(x => x.CustomerSealLocationViewModels, y => y.MapFrom(o => (o.TypographicResourceLocations.Where(x => x.TypographicResource.SealType == SealType.Customer))))
                    .ForMember(x => x.AccountantSignLocationViewModels, y => y.MapFrom(o => (o.TypographicResourceLocations.Where(x => x.TypographicResource.SealType == SealType.Accountant))))
                    .ForMember(x => x.LetterheadImageLocationViewModels, y => y.MapFrom(o => (o.TypographicResourceLocations.Where(x => x.TypographicResource.SealType == SealType.Letterhead))))
                    .ForMember(x => x.TemporarySealLocationViewModels, y => y.MapFrom(o => (o.TypographicResourceLocations.Where(x => x.TypographicResource.SealType == SealType.TemporarySeal))));

            CreateMap<TypographicResourceLocation, CustomerSealLocationViewModel>()
                    .ForMember(x => x.Sequence, y => y.MapFrom(o => o.TypographicResource.Sequence))
                    .ForMember(x => x.CustomerSealType, y => y.MapFrom(o => SealMappingConfigUtil.GetCustomerSealType(o.TypographicResource.SubSealType)))
                    .ForMember(x => x.ImageBase64, y => y.MapFrom(o => ImageSharpUtil.PathImageFileToBase64(o.TypographicResource.ImageFullPath)));
            CreateMap<TypographicResourceLocation, AccountantSignLocationViewModel>()
                    .ForMember(x => x.AccountantSignType, y => y.MapFrom(o => SealMappingConfigUtil.GetAccountantSignType(o.TypographicResource.SubSealType)))
                    .ForMember(x => x.ImageBase64, y => y.MapFrom(o => ImageSharpUtil.PathImageFileToBase64(o.TypographicResource.ImageFullPath)));
            CreateMap<TypographicResourceLocation, LetterheadImageLocationViewModel>()
                    .ForMember(x => x.ImageBase64, y => y.MapFrom(o => ImageSharpUtil.PathImageFileToBase64(o.TypographicResource.ImageFullPath)));
            CreateMap<TypographicResourceLocation, TemporarySealLocationViewModel>()
                    .ForMember(x => x.Sequence, y => y.MapFrom(o => o.TypographicResource.Sequence))
                    .ForMember(x => x.ImageBase64, y => y.MapFrom(o => ImageSharpUtil.PathImageFileToBase64(o.TypographicResource.ImageFullPath)));                      

            //印鑑與簽印複製使用
            CreateMap<TypographicResource, TypographicResource>()
                    .ForMember(x => x.CustomerSealGroup, y => y.Ignore())
                    .ForMember(x => x.AccountantSignGroup, y => y.Ignore())
                    .ForMember(x => x.Letterhead, y => y.Ignore())
                    .ForMember(x => x.TemporarySealGroup, y => y.Ignore())
                    .ForMember(x => x.Id, y => y.Ignore());

            //讀取PDF概要內容的Map
            CreateMap<TypographicPDF, TypographicPDFSettingViewModel>()
                 .ForMember(x => x.OriginalFileName, y => y.MapFrom(o => o.OriginFileName))
                 .ForMember(x => x.Quarter, y => y.MapFrom(o => new string($"{o.Quarter.GregorianYear}{o.Quarter.Period}")))
                 .ForMember(x => x.EditPageCount, y => y.MapFrom(o => (o.TypographicPages.Count())))
                 .ForMember(x => x.BlankPageCount, y => y.MapFrom(o => (o.TypographicPages.Where(x => x.BlankCheck == true).Count())));

            //PDF該頁的編輯內容的Map
            CreateMap<TypographicPage, EditPage>()
                 .ForMember(dst => dst.AccountantCertificatePath, opt => opt.MapFrom(src => src.UploadFile != null ? src.UploadFile.FullPath : string.Empty))
                 .ForMember(dst => dst.EditImages, y => y.MapFrom(o => o.TypographicResourceLocations));
            // PDF排版圖像與位置的Map
            CreateMap<TypographicResourceLocation, EditImage>()
                .ForMember(x => x.ImageBase64, y => y.MapFrom(o => new string(ImageInfo.FromPath(o.TypographicResource.ImageFullPath).ImageToBase64())));

        }
    }
}
