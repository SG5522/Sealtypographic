using AutoMapper;
using DBEntities;
using DBEntities.Consts;
using SealTypographicWebAPI.Utils;
using SealTypographicWebAPI.Models.ImageRangeSetting;

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
            CreateMap<CustomerSealRangeSetting, ImageRangeSetting>()                 
                    .ForMember(dst => dst.ImageRangeLocations, opt => opt.MapFrom(src => src.CustomerSealRangeLocations));

            //客戶印鑑截取範圍設定
            CreateMap<CustomerSealRangeLocation, ImageRangeLocation>()
                    .ForMember(dst => dst.SealType, opt => opt.MapFrom(src => SealType.Customer))
                    .ForMember(dst => dst.SubSealType, opt => opt.MapFrom(src => SealMappingConfigUtil.GetSubSealTypeWithCustomer(src.CustomerSealType)));


            //客戶印鑑截取設定
            CreateMap<ImageRangeSetting, CustomerSealRangeSetting>()                    
                    .ForMember(dst => dst.CustomerSealRangeLocations, opt => opt.MapFrom(src => src.ImageRangeLocations));

            //客戶印鑑截取範圍設定
            CreateMap<ImageRangeLocation, CustomerSealRangeLocation>()
                    .ForMember(dst => dst.CustomerSealType, opt => opt.MapFrom(src => SealMappingConfigUtil.GetCustomerSealType(src.SubSealType)));


            //會計師簽印截取設定
            CreateMap<AccountantSignRangeSetting, ImageRangeSetting>()
                    .ForMember(dst => dst.ImageRangeLocations, opt => opt.MapFrom(src => src.AccountantSignRangeLocations));

            //會計師簽印截取範圍設定
            CreateMap<AccountantSignRangeLocation, ImageRangeLocation>()
                    .ForMember(dst => dst.SealType, opt => opt.MapFrom(src => SealType.Accountant))
                    .ForMember(dst => dst.SubSealType, opt => opt.MapFrom(src => SealMappingConfigUtil.GetSubSealTypeWithAccountant(src.AccountantSignType)));


            //會計師簽印截取設定
            CreateMap<ImageRangeSetting, AccountantSignRangeSetting>()
                    .ForMember(dst => dst.AccountantSignRangeLocations, opt => opt.MapFrom(src => src.ImageRangeLocations));

            //會計師簽印截取範圍設定
            CreateMap<ImageRangeLocation, AccountantSignRangeLocation>()
                    .ForMember(dst => dst.AccountantSignType, opt => opt.MapFrom(src => SealMappingConfigUtil.GetAccountantSignType(src.SubSealType)));
        }
    }
}
