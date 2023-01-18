using AutoMapper;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Models.TypographicPDF;
using SealTypographicWebAPI.Models.AccountantGroup;
using SealTypographicWebAPI.Models.CustomerSealReview;


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
            CreateMap<CustomerUpdateForm, Customer>();

            //客戶印鑑
            CreateMap<CustomerSealJournal, CustomerSealViewModel>()
                    .ForMember(x => x.ImageBase64, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();

            CreateMap<AccountantSignCreateDateJournal, CustomerSealViewModel>()
                    .ForMember(x => x.ImageBase64, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();

            CreateMap<CustomerSeal, CustomerSealJournal>()
                    .ForMember(x => x.ImagePath, y => y.Ignore()) // <---ImagePath要額外處理所以要忽略
                    .ReverseMap();

            CreateMap<CustomerSeal, AccountantSignCreateDateJournal>();                    

            CreateMap<CustomerSealUpdateForm, CustomerSealJournal>()
                    .ForMember(x => x.ImagePath, y => y.Ignore()) // <---ImagePath要額外處理所以要忽略
                    .ReverseMap();

            //客戶印鑑序號確認用
            CreateMap<CustomerSeal, CustomerSealSequenceCheck>();            
            CreateMap<CustomerSealJournal, CustomerSealSequenceCheck>();
                             

            //客戶印鑑審核
            CreateMap<CustomerSealQuarterJournal, CustomerSealQuarterViewModel>()
                    .ForMember(x => x.Id, y => y.MapFrom(o => o.Customer.Id))
                    .ForMember(x => x.Name, y => y.MapFrom(o => o.Customer.Name))
                    .ForMember(x => x.Code, y => y.MapFrom(o => o.Customer.Code))
                    .ForMember(x => x.Quarter, y => y.MapFrom(o => o.Quarter))
                    .ForMember(x => x.ReviewStatus, y => y.MapFrom(o => o.ReviewStatus))
                    .ReverseMap();

            CreateMap<Customer, CustomerSealReviewDetail>();


            //會計師基本資料
            CreateMap<Accountant, AccountantViewModel>()                    
                    .ForMember(x => x.AccountantGroupName, y => y.MapFrom(o => o.AccountantGroup.Name))
                    .ReverseMap();

            CreateMap<Accountant, AccountantViewModelWithCreateDate>()
                    .ForMember(x => x.AccountantGroupName, y => y.MapFrom(o => o.AccountantGroup.Name))
                    .ForMember(x => x.AccountantNumber, y => y.MapFrom(o => o.Code))
                    .ReverseMap();

            CreateMap<Accountant, AccountantDetailViewModel>()
                    .ForMember(x => x.AccountantNumber, y => y.MapFrom(o => o.Code))
                    .ReverseMap();

            CreateMap<AccountantForm, Accountant>()
                     .ForMember(x => x.Code, y => y.MapFrom(o => o.AccountantNumber))
                     .ReverseMap();
            CreateMap<AccountantFormUpdate, Accountant>();

            //會計師群組
            CreateMap<AccountantGroup, AccountantGroupViewModel>();            
            CreateMap<AccountantGroupForm, AccountantGroup>();
            CreateMap<AccountantGroupFormUpdate,AccountantGroup>();            

            //會計師印鑑
            CreateMap<AccountantSignJournal, AccountantSignViewModel>()
                    //.ForMember(x => x.SealMappingConfigName, y => y.MapFrom(o => o.SealMappingConfig.Name))
                    .ForMember(x => x.ImageBase64, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();

            CreateMap<AccountantSignCreateDateJournal, AccountantSignViewModel>();

            CreateMap<AccountantSign, AccountantSignJournal>()
                    .ForMember(x => x.ImagePath, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();
            
            CreateMap<AccountantSignFormUpdate, AccountantSignJournal>()
                    .ForMember(x => x.ImagePath, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();
            //會計師簽名重複確認用
            CreateMap<AccountantSign, AccountantSignCheck>();
            CreateMap<AccountantSignFormUpdate, AccountantSignCheck>();

            //信頭基本資料
            CreateMap<Letterhead, LetterheadViewModel>();            


            //PDF排版資訊
            CreateMap<TypographicPDFForm, TypographicPDF>()
                .ForMember(x => x.FullPath, y => y.Ignore());

            CreateMap<TypographicPageForm, TypographicPage>();
            CreateMap<CustomerSealLocationForm, CustomerSealLocation>();
            CreateMap<AccountantSingLocationForm, AccountantSignLocation>();
            CreateMap<LetterheadImageLocationForm, LetterheadImageLocation>();



        }
    }
}
