using AutoMapper;
using DJLib;
using DBEntities;
using DBEntities.Consts;
using SealTypographicWebAPI.Utils;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.CustomerSeal;

namespace SealTypographicWebAPI.Config
{
    /// <summary>
    /// AutoMapper用的LIST
    /// </summary>
    public class CustomerMapperProfile : Profile
    {        
        /// <summary>
        /// 建置
        /// </summary>
        public CustomerMapperProfile()
        {            
            //客戶基本資料
            CreateMap<Customer, CustomerDetail>();
            CreateMap<Customer, CustomerViewModel>();
            CreateMap<Customer, CustomerSummary>();

            CreateMap<CustomerForm, Customer>();
            CreateMap<CustomerUpdateForm, Customer>();

            //客戶印鑑
            CreateMap<CustomerSealGroup, CustomerSealViewModels>()
                    .ForMember(dst => dst.CustomerSealQuarterId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Quarter, opt => opt.MapFrom(src => QuarterUtil.GetTaiwanYearQuarter(src.QuarterYear)))
                    .ForMember(dst => dst.ReviewStatus, opt => opt.MapFrom(src => src.ReviewStatus))
                    .ForMember(dst => dst.SealViewModels, opt => opt.MapFrom(src => src.TypographicResources.Where(x => x.DeleteStatus == DeleteStatus.No)));

            CreateMap<CustomerSealGroup, CustomerSealGroupResponse>()
                .ForMember(dst => dst.CustomerSealGroupId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Quarter, opt => opt.MapFrom(src => QuarterUtil.GetTaiwanYearQuarter(src.QuarterYear)));
            //CustomerSealQuarterResponse

            CreateMap<TypographicResource, CustomerSealViewModel>()
                    .ForMember(dst => dst.SealMappingConfigId, opt => opt.MapFrom(src => SealMappingConfigUtil.GetCustomerSealType(src.SubSealType)))
                    .ForMember(dst => dst.ImageBase64, opt => opt.MapFrom(src => ImageSharpUtil.PathImageFileToBase64(src.ImageFullPath)));
                    
            CreateMap<CustomerSeal, TypographicResource>()
                    .ForMember(dst => dst.ImageFullPath, opt => opt.Ignore()) // <---ImagePath要額外處理所以要忽略                    
                    .ReverseMap();

            CreateMap<CustomerSealViewModel, TypographicResource>()
                    .ForMember(dst => dst.ImageFullPath, opt => opt.Ignore()); // <---ImagePath要額外處理所以要忽略  


            CreateMap<CustomerSealUpdateForm, TypographicResource>()
                    .ForMember(dst => dst.ImageFullPath, opt => opt.Ignore()) // <---ImagePath要額外處理所以要忽略
                    .ReverseMap();
            
        }
    }
}
