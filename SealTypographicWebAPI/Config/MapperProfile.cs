using AutoMapper;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Models.TypographicPDF;
using SealTypographicWebAPI.Models.AccountantGroup;
using SealTypographicWebAPI.Models.SealMappingConfig;
using SealTypographicWebAPI.Models.AccountantGroupMember;
using SealTypographicWebAPI.Models.CustomerSealReview;
using AutoMapper.Internal;

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
            //客戶基本資料
            CreateMap<Customer, CustomerDetail>();
            CreateMap<Customer, CustomerViewModel>();
            CreateMap<CustomerForm, Customer>();
            CreateMap<CustomerFormUpdate, Customer>();

            //客戶印鑑
            CreateMap<CustomerSealJournal, CustomerSealViewModel>()
                    .ForMember(x => x.SealMappingConfigId, y => y.MapFrom(o => o.SealMappingConfig.Id))
                    .ForMember(x => x.SealMappingConfigName, y => y.MapFrom(o => o.SealMappingConfig.Name))
                    .ForMember(x => x.SealMappingConfigSubId, y => y.MapFrom(o => o.SealMappingConfig.SubId))
                    .ForMember(x => x.ImageBase64, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();

            CreateMap<CustomerSealForm, CustomerSealJournal>()
                    .ForMember(x => x.ImagePath, y => y.Ignore()) // <---ImagePath要額外處理所以要忽略
                    .ReverseMap();

            CreateMap<CustomerSealFormUpdate, CustomerSealJournal>()
                    .ForMember(x => x.ImagePath, y => y.Ignore()) // <---ImagePath要額外處理所以要忽略
                    .ReverseMap();

            //客戶印鑑序號確認用
            CreateMap<CustomerSealForm, CustomerSealSequenceCheck>();
            CreateMap<CustomerSealJournal, CustomerSealSequenceCheck>();

            //客戶印鑑審核
            CreateMap<CustomerSealJournal, CustomerSealReviewViewModel>()
                    .ForMember(x => x.Id, y => y.MapFrom(o => o.Customer.Id))
                    .ForMember(x => x.Name, y => y.MapFrom(o => o.Customer.Name))
                    .ForMember(x => x.CustomerNumber, y => y.MapFrom(o => o.Customer.CustomerNumber))
                    .ForMember(x => x.BAN, y => y.MapFrom(o => o.Customer.BAN))
                    .ReverseMap();

            CreateMap<Customer, CustomerSealReviewDetail>();


            //會計師基本資料
            CreateMap<Accountant, AccountantViewModel>()                    
                    .ForMember(x => x.AccountantGroupName, y => y.MapFrom(o => o.AccountantGroup.Name))
                    .ReverseMap();

            CreateMap<Accountant, AccountantViewModelWithCreateDate>()
                    .ForMember(x => x.AccountantGroupName, y => y.MapFrom(o => o.AccountantGroup.Name))
                    .ReverseMap();

            CreateMap<Accountant, AccountantDetailViewModel>()
                    //.ForMember(x => x.AccountantGroupName, y => y.MapFrom(o => o.AccountantGroup.Name))
                    //.ForMember(x => x.AccountantGroupId, y => y.MapFrom(o => o.AccountantGroup.Id))
                    //.ForMember(x => x.AccountantNumber, y => y.MapFrom(o => o.AccountantGroup.Id))
                    .ReverseMap();

            CreateMap<AccountantForm, Accountant>();
            CreateMap<AccountantFormUpdate, Accountant>();

            //會計師群組
            CreateMap<AccountantGroup, AccountantGroupViewModel>();
            CreateMap<AccountantGroupForm, AccountantGroup>();
            CreateMap<AccountantGroupFormUpdate,AccountantGroup>();            

            //會計師印鑑
            CreateMap<AccountantSignJournal, AccountantSignViewModel>()
                    .ForMember(x => x.SealMappingConfigName, y => y.MapFrom(o => o.SealMappingConfig.Name))
                    .ForMember(x => x.ImageBase64, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();
            CreateMap<AccountantSignForm, AccountantSignJournal>()
                    .ForMember(x => x.ImagePath, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();
            
            CreateMap<AccountantSignFormUpdate, AccountantSignJournal>()
                    .ForMember(x => x.ImagePath, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();
            //會計師簽名重複確認用
            CreateMap<AccountantSignForm, AccountantSignCheck>();
            CreateMap<AccountantSignFormUpdate, AccountantSignCheck>();

            //信頭基本資料
            CreateMap<Letterhead, LetterheadViewModel>();            
            CreateMap<LetterheadForm, Letterhead>();
            CreateMap<LetterheadFormUpdate, Letterhead>();

            //信頭圖片
            CreateMap<LetterheadImageJournal, LetterheadImageViewModel>()
                    .ForMember(x => x.SealMappingConfigName, y => y.MapFrom(o => o.SealMappingConfig.Name))
                    .ForMember(x => x.ImageBase64, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();

            CreateMap<LetterheadImageForm, LetterheadImageJournal>()
                    .ForMember(x => x.ImagePath, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();
            CreateMap<LetterheadImageFormUpdate, LetterheadImageJournal>()
                    .ForMember(x => x.ImagePath, y => y.Ignore()) // <---ImagePath要額外處理所以要忽略
                    .ReverseMap();

            //印鑑種類資料
            CreateMap<SealMappingConfig, SealMappingConfigViewModel>();
            CreateMap<SealMappingConfigViewModel, SealMappingConfig>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ReverseMap();

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
