using AutoMapper;
using DBEntities;
using DBEntities.Consts;
using SealTypographicWebAPI.Utils;
using SealTypographicWebAPI.Models.SealCaptureRange;

namespace SealTypographicWebAPI.Config.MapperProfile
{
    /// <summary>
    /// AutoMapper用的LIST
    /// </summary>
    public class SettingMapperProfile : Profile
    {
        /// <summary>
        /// 建置
        /// </summary>
        public SettingMapperProfile()
        {
            //客戶印鑑截取設定
            CreateMap<CustomerSealCaptureSetting, ImageCaptureSetting>()                 
                    .ForMember(dst => dst.ImageCaptureLocations, opt => opt.MapFrom(src => src.CustomerSealCaptureLocations));

            //客戶印鑑截取範圍設定
            CreateMap<CustomerSealCaptureLocation, ImageCaptureLocation>()
                    .ForMember(dst => dst.SealType, opt => opt.MapFrom(src => SealType.Customer))
                    .ForMember(dst => dst.SubSealType, opt => opt.MapFrom(src => SealMappingConfigUtil.GetSubSealTypeWithCustomer(src.CustomerSealType)));


            //客戶印鑑截取設定
            CreateMap<ImageCaptureSetting, CustomerSealCaptureSetting>()                    
                    .ForMember(dst => dst.CustomerSealCaptureLocations, opt => opt.MapFrom(src => src.ImageCaptureLocations));

            //客戶印鑑截取範圍設定
            CreateMap<ImageCaptureLocation, CustomerSealCaptureLocation>()
                    .ForMember(dst => dst.CustomerSealType, opt => opt.MapFrom(src => SealMappingConfigUtil.GetCustomerSealType(src.SubSealType)));


            //會計師簽印截取設定
            CreateMap<AccountantSignCaptureSetting, ImageCaptureSetting>()
                    .ForMember(dst => dst.ImageCaptureLocations, opt => opt.MapFrom(src => src.AccountantSignCaptureLocations));

            //會計師簽印截取範圍設定
            CreateMap<AccountantSignCaptureLocation, ImageCaptureLocation>()
                    .ForMember(dst => dst.SealType, opt => opt.MapFrom(src => SealType.Accountant))
                    .ForMember(dst => dst.SubSealType, opt => opt.MapFrom(src => SealMappingConfigUtil.GetSubSealTypeWithAccountant(src.AccountantSignType)));


            //會計師簽印截取設定
            CreateMap<ImageCaptureSetting, AccountantSignCaptureSetting>()
                    .ForMember(dst => dst.AccountantSignCaptureLocations, opt => opt.MapFrom(src => src.ImageCaptureLocations));

            //會計師簽印截取範圍設定
            CreateMap<ImageCaptureLocation, AccountantSignCaptureLocation>()
                    .ForMember(dst => dst.AccountantSignType, opt => opt.MapFrom(src => SealMappingConfigUtil.GetAccountantSignType(src.SubSealType)));
        }
    }
}
