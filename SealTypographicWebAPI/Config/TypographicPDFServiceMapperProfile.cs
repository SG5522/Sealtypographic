using AutoMapper;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Models.TypographicPDF;
using SealTypographicWebAPI.Models.AccountantGroup;
using SealTypographicWebAPI.Models.CustomerSealReview;
using SealTypographicWebAPI.Models.AccountantSignReview;
using SealTypographicWebAPI.Models.TemporarySeal;
using SealTypographicWebAPI.Models.CustomerSealTemplate;
using SealTypographicWebAPI.Models.AccountantSignTemplate;
using SealTypographicWebAPI.Models.LetterheadImageTemplate;
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

            CreateMap<CustomerSealLocationForm, TypographicResourceLocation>()
                    .ForMember(x => x.Id, y => y.Ignore()) //此ID非為ResourceLocation的ID而是關聯用的ID
                    .ReverseMap();
            CreateMap<AccountantSignLocationForm, TypographicResourceLocation>()
                    .ForMember(x => x.Id, y => y.Ignore()) //此ID非為ResourceLocation的ID而是關聯用的ID
                    .ReverseMap();
            CreateMap<LetterheadImageLocationForm, TypographicResourceLocation>()
                    .ForMember(x => x.Id, y => y.Ignore()) //此ID非為ResourceLocation的ID而是關聯用的ID
                    .ReverseMap();
            CreateMap<TemporarySealLocationForm, TypographicResourceLocation>()
                    .ForMember(x => x.Id, y => y.Ignore()) //此ID非為ResourceLocation的ID而是關聯用的ID
                    .ReverseMap();

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
                 .ForMember(x => x.EditImages, y => y.MapFrom(o => o.TypographicResourceLocations));
            // PDF排版圖像與位置的Map
            CreateMap<TypographicResourceLocation, EditImage>()
                .ForMember(x => x.ImageBase64, y => y.MapFrom(o => new string(ImageInfo.FromPath(o.TypographicResource.ImageFullPath).ImageToBase64())));

        }
    }
}
