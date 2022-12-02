using AutoMapper;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Models.TypographicPDF;

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
            CreateMap<Customer, CustomerForm>();
            CreateMap<Customer, CustomerPaginateViewModel>();
            CreateMap<CustomerForm, Customer>();

            //顧客印鑑
            CreateMap<CustomerSealJournal, CustomerSealViewModel>()
                    .ForMember(x => x.SealMappingConfigName, y => y.MapFrom(o => o.SealMappingConfig.Name))
                    .ForMember(x => x.ImageBase64, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();
            CreateMap<CustomerSealJournal, CustomerSealForm>()
                    .ForMember(x => x.ImageBase64, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();

            //會計師基本資料
            CreateMap<Accountant, AccountantViewModel>()                    
                    .ForMember(x => x.AccountantGroupName, y => y.MapFrom(o => o.AccountantGroup.Name))
                    .ReverseMap();
            CreateMap<Accountant, AccountantPaginateViewModel>()
                    .ForMember(x => x.AccountantGroupName, y => y.MapFrom(o => o.AccountantGroup.Name))
                    .ReverseMap();
            CreateMap<AccountantForm, Accountant>();


            //會計師印鑑
            CreateMap<AccountantSignJournal, AccountantSignViewModel>()
                    .ForMember(x => x.SealMappingConfigName, y => y.MapFrom(o => o.SealMappingConfig.Name))
                    .ForMember(x => x.ImageBase64, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();
            CreateMap<AccountantSignJournal, AccountantSignForm>()
                    .ForMember(x => x.ImageBase64, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();
            //這個可能用不到
            CreateMap<AccountantSignJournal, AccountantSignUpdate>()
                    .ForMember(x => x.ImageBase64, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();

            //信頭基本資料
            CreateMap<Letterhead, LetterheadViewModel>();            
            CreateMap<LetterheadForm, Letterhead>();


            //PDF排版資訊
            CreateMap<TypographicPDFForm, TypographicPDF>()
                .ForMember(x => x.FullPath, y => y.Ignore());

            CreateMap<TypographicPageForm, TypographicPage>();
            CreateMap<CustomerSealLocationForm, CustomerSealLocation>();
            CreateMap<AccountantSingLocationForm, AccountantSingLocation>();
            CreateMap<LetterheadImageLocationForm, LetterheadImageLocation>();



        }
    }
}
