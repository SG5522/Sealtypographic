using AutoMapper;
using DBEntities.Consts;
using SealTypographicWebAPI.Utils;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.CustomerSeal;
using DBEntities.Entities.TypographicModels;
using DBEntities.Entities.CustomerModels;
using DJImageLib.Utils;

namespace SealTypographicWebAPI.Config.MapperProfile
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

            CreateMap<Customer, CustomerSummary>();

            CreateMap<CustomerForm, Customer>();
            CreateMap<CustomerUpdateForm, Customer>();

            //客戶印鑑
            CreateMap<CustomerSealGroup, CustomerSealViewModels>()
                    .ForMember(dst => dst.CustomerSealQuarterId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.QuarterYearId, opt => opt.MapFrom(src => src.QuarterYear.Id))
                    .ForMember(dst => dst.ReviewStatus, opt => opt.MapFrom(src => src.ReviewStatus))
                    .ForMember(dst => dst.SealViewModels, opt => opt.MapFrom(src => src.TypographicResources.Where(x => x.DeleteStatus == DeleteStatus.No)))
                    //log save
                    .ForMember(dst => dst.GregorainQuarterYear, opt => opt.MapFrom(src => 
                        src.QuarterYear.Type == TypographyType.FinancialReport ?
                        QuarterUtil.GetGregorainQuarter(src.QuarterYear) : QuarterUtil.GetGregorainYear(src.QuarterYear)
                    ))
                    .ForMember(dst => dst.DisplayQuarterYear, opt => opt.MapFrom(src =>
                        src.QuarterYear.Type == TypographyType.FinancialReport ?
                        QuarterUtil.GetTaiwanYearQuarter(src.QuarterYear) : QuarterUtil.GetTaiwanYear(src.QuarterYear)
                    ));                     

            //客戶印鑑(Log)
            CreateMap<CustomerSealViewModels, CustomerSealViewModels>();

            CreateMap<CustomerSealForm, CustomerSealForm>();

            CreateMap<CustomerSealUpdate, CustomerSealUpdate>();

            //Log紀錄使用
            CreateMap<CustomerSealUpdateForm, CustomerSealUpdateForm>()
                    .ForMember(dst => dst.ImageBase64, opt => opt.MapFrom(src => src.ImageBase64 != null ? "Image/base64..." : null));

            CreateMap<CustomerSeal, CustomerSeal>()
                    .ForMember(dst => dst.ImageBase64, opt => opt.MapFrom(src => src.ImageBase64 != null ? "Image/base64..." : null));

            CreateMap<CustomerSealViewModel, CustomerSealViewModel>()
                    .ForMember(dst => dst.ImageBase64, opt => opt.MapFrom(src => src.ImageBase64 != null ? "Image/base64..." : null));

            CreateMap<CustomerSealGroup, CustomerSealGroupResponse>()
                    .ForMember(dst => dst.CustomerSealGroupId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.QuarterYearId, opt => opt.MapFrom(src => src.QuarterYear.Id));            

            CreateMap<TypographicResource, CustomerSealViewModel>()
                    .ForMember(dst => dst.SealMappingConfigId, opt => opt.MapFrom(src => SealMappingConfigUtil.GetCustomerSealType(src.SubSealType)))
                    .ForMember(dst => dst.ImageBase64, opt => opt.MapFrom(src => ImageUtil.ToDataUrlFromFilePath(src.ImageFullPath)));

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
