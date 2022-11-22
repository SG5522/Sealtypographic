using AutoMapper;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.DbModels;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// AutoMapper用的LIST
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public AutoMapperProfile()
        {
            //顧客基本資料
            CreateMap<DbModels.Customer, CustomerData>();
            CreateMap<DbModels.Customer, CustomerViewModel>();
            CreateMap<CustomerData, DbModels.Customer>();
            
            //顧客印鑑
            CreateMap<CustomerSealJournal, CustomerSealViewModel>()
                    .ForMember(x => x.ImageGroupName, y => y.MapFrom(o => o.ImageGroup.Name))
                    .ForMember(x => x.ImageBase64, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();
            CreateMap<CustomerSealJournal,CustomerSeal>();
        }
    }
}
