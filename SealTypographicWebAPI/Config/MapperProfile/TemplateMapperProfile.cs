using AutoMapper;
using SealTypographicWebAPI.Models.CustomerSealTemplate;
using SealTypographicWebAPI.Models.AccountantSignTemplate;
using SealTypographicWebAPI.Models.LetterheadImageTemplate;
using SealTypographicWebAPI.Utils;
using DJLib;
using DBEntities.Entities.TemplateModels;

namespace SealTypographicWebAPI.Config.MapperProfile
{
    /// <summary>
    /// AutoMapper用的LIST
    /// </summary>
    public class TemplateMapperProfile : Profile
    {
        /// <summary>
        /// 建置
        /// </summary>
        public TemplateMapperProfile()
        {
            //客戶印鑑樣板使用
            CreateMap<CustomerSealTemplateForm, Template>();
            CreateMap<CustomerSealTemplateLocationForm, TemplateLocation>();

            //客戶印鑑樣板異動使用
            CreateMap<CustomerSealTemplateUpdateForm, Template>()
                ;
            CreateMap<CustomerSealTemplateLocationUpdateForm, TemplateLocation>();

            //客戶印鑑樣板單筆查詢使用
            CreateMap<Template, CustomerSealTemplateDetailViewModel>()
                    .ForMember(dst => dst.LocaltionViewModels, opt => opt.MapFrom(src => src.TemplateLocations));
            CreateMap<TemplateLocation, CustomerSealTemplateLocationViewModel>()
                    .ForMember(dst => dst.CustomerSealType, opt => opt.MapFrom(src => SealMappingConfigUtil.GetCustomerSealType(src.SubSealType)));

            //客戶印鑑樣版分頁查詢使用
            CreateMap<Template, CustomerSealTemplateViewModel>()
                    .ForMember(dst => dst.ThumbnailBase64, opt => opt.MapFrom(src => ImageSharpUtil.PathImageFileToBase64(src.ThumbnailFullPath)));

            //Log使用
            CreateMap<CustomerSealTemplateViewModel, CustomerSealTemplateLogModel>();
            CreateMap<CustomerSealTemplatePaginate, CustomerSealTemplatePaginateLog>();


            //會計師簽印樣板使用
            CreateMap<AccountantSignTemplateForm, Template>();
            CreateMap<AccountantSignTemplateLocationForm, TemplateLocation>();

            //會計師簽印樣板異動使用
            CreateMap<AccountantSignTemplateUpdateForm, Template>();
            CreateMap<AccountantSignTemplateLocationUpdateForm, TemplateLocation>();

            //會計師簽印樣板單筆查詢使用
            CreateMap<Template, AccountantSignTemplateDetailViewModel>()
                    .ForMember(dst => dst.LocaltionViewModels, opt => opt.MapFrom(src => src.TemplateLocations));
            CreateMap<TemplateLocation, AccountantSignTemplateLocationViewModel>()
                    .ForMember(dst => dst.AccountantSignType, opt => opt.MapFrom(src => SealMappingConfigUtil.GetAccountantSignType(src.SubSealType)));

            //會計師簽印樣版分頁查詢使用
            CreateMap<Template, AccountantSignTemplateViewModel>()
                    .ForMember(dst => dst.ThumbnailBase64, opt => opt.MapFrom(src => ImageSharpUtil.PathImageFileToBase64(src.ThumbnailFullPath)));

            CreateMap<TemplateLocation, TemplateLocation>();
            //Log使用
            CreateMap<AccountantSignTemplateViewModel, AccountantSignTemplateLogModel>();
            CreateMap<AccountantSignTemplatePaginate, AccountantSignTemplatePaginateLog>();


            //信頭樣板使用
            CreateMap<LetterheadImageTemplateForm, Template>();
            CreateMap<LetterheadImageTemplateLocationForm, TemplateLocation>();
            //信頭樣板異動使用
            CreateMap<LetterheadImageTemplateUpdateForm, Template>();
            CreateMap<LetterheadImageTemplateLocationUpdateForm, TemplateLocation>();
            //信頭樣板單筆查詢使用
            CreateMap<Template, LetterheadImageTemplateDetailViewModel>()
                    .ForMember(dst => dst.LocaltionViewModels, opt => opt.MapFrom(src => src.TemplateLocations));
            CreateMap<TemplateLocation, LetterheadImageTemplateLocationViewModel>();


            CreateMap<Template, LetterheadImageTemplateViewModel>()
                    .ForMember(dst => dst.ImageFullPath, opt => opt.MapFrom(src => src.ThumbnailFullPath))
                    .ForMember(dst => dst.ThumbnailBase64, opt => opt.MapFrom(src => ImageSharpUtil.PathImageFileToBase64(src.ThumbnailFullPath)));

            //Log使用
            CreateMap<LetterheadImageTemplateViewModel, LetterheadImageTemplateLogModel>();
            CreateMap<LetterheadImageTemplatePaginate, LetterheadImageTemplatePaginateLog>();
        }
    }
}
