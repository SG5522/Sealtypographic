using AutoMapper;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.DbModels;

namespace SealTypographicWebAPI.Config
{
    /// <summary>
    /// AutoMapper用的LIST
    /// </summary>
    public class MapperProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public MapperProfile()
        {
            //顧客基本資料
            CreateMap<Customer, CustomerData>();
            CreateMap<Customer, CustomerViewModel>();
            CreateMap<CustomerData, Customer>();

            //顧客印鑑
            CreateMap<CustomerSealJournal, CustomerSealViewModel>()
                    .ForMember(x => x.SealMappingConfigName, y => y.MapFrom(o => o.SealMappingConfig.Name))
                    .ForMember(x => x.ImageBase64, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();
            CreateMap<CustomerSealJournal, CustomerSeal>()
                    .ForMember(x => x.ImageBase64, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();
        }
    }
}
