using AutoMapper;
using SealTypographicWebAPI.Models.Customer;
using DBEntities;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Models.TypographicPDF;
using SealTypographicWebAPI.Models.AccountantGroup;
using SealTypographicWebAPI.Models.CustomerSealReview;
using SealTypographicWebAPI.Models.AccountantSignReview;
using SealTypographicWebAPI.Models.TemporarySeal;
using SealTypographicWebAPI.Models.CustomerSealTemplate;
using SealTypographicWebAPI.Models.AccountantSignTemplate;
using SealTypographicWebAPI.Models.LetterheadTemplate;

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

            CreateMap<AccountantSignGroupJournal, CustomerSealViewModel>()
                    .ForMember(x => x.ImageBase64, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();

            CreateMap<CustomerSeal, CustomerSealJournal>()
                    .ForMember(x => x.ImageFullPath, y => y.Ignore()) // <---ImagePath要額外處理所以要忽略                    
                    .ReverseMap();

            CreateMap<CustomerSeal, AccountantSignGroupJournal>();                    

            CreateMap<CustomerSealUpdateForm, CustomerSealJournal>()
                    .ForMember(x => x.ImageFullPath, y => y.Ignore()) // <---ImagePath要額外處理所以要忽略
                    .ReverseMap();

            //客戶印鑑序號確認用
            CreateMap<CustomerSeal, CustomerSealSequenceCheck>();            
            CreateMap<CustomerSealJournal, CustomerSealSequenceCheck>();


            //客戶印鑑季度審核清單
            CreateMap<CustomerSealQuarterJournal, CustomerSealQuarterReviewViewModel>()                    
                    .ForMember(x => x.Name, y => y.MapFrom(o => o.Customer.Name))
                    .ForMember(x => x.Code, y => y.MapFrom(o => o.Customer.Code))
                    .ForMember(x => x.Quarter, y => y.MapFrom(o => o.Quarter))
                    .ForMember(x => x.ReviewStatus, y => y.MapFrom(o => o.ReviewStatus))
                    .ReverseMap();

            //客戶印鑑審核詳細資料
            CreateMap<Customer, CustomerSealQuarterDetailReviewViewModel>()
                 .ForMember(x => x.Id, y => y.Ignore())
                 .ReverseMap();

            //會計師基本資料
            CreateMap<Accountant, AccountantViewModel>()                    
                    .ForMember(x => x.AccountantGroupName, y => y.MapFrom(o => o.AccountantGroup.Name))
                    .ForMember(x => x.AccountantNumber, y => y.MapFrom(o => o.Code))
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
            CreateMap<AccountantUpdateForm, Accountant>()
                     .ForMember(x => x.Code, y => y.MapFrom(o => o.AccountantNumber))
                     .ReverseMap();                        

            //會計師群組
            CreateMap<AccountantGroup, AccountantGroupViewModel>();            
            CreateMap<AccountantGroupForm, AccountantGroup>();
            CreateMap<AccountantGroupUpdateForm,AccountantGroup>();            

            //會計師印鑑
            CreateMap<AccountantSignJournal, AccountantSignViewModel>()                    
                    .ForMember(x => x.ImageBase64, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();
            
            CreateMap<AccountantSignGroupJournal, AccountantSignViewModel>();

            CreateMap<AccountantSign, AccountantSignJournal>()
                    .ForMember(x => x.ImageFullPath, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();

            CreateMap<AccountantSignUpdateForm, AccountantSignJournal>()
                    .ForMember(x => x.ImageFullPath, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();

            //會計師簽印重複確認
            CreateMap<AccountantSign, AccountantSignCheck>();
            CreateMap<AccountantSignUpdateForm, AccountantSignCheck>();

            //會計師簽印審核清單
            CreateMap<AccountantSignGroupJournal, AccountantSignGroupReviewViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(o => o.Accountant.Name))
                .ForMember(x => x.Code, y => y.MapFrom(o => o.Accountant.Code))
                .ForMember(x => x.GroupName, y => y.MapFrom(o => o.Accountant.AccountantGroup.Name))
                .ForMember(x => x.ReviewStatus, y => y.MapFrom(o => o.ReviewStatus))
                .ReverseMap();

            //會計師簽印審核詳細資料
            CreateMap<Accountant, AccountantSignGroupDetailReviewViewModel>()
                 .ForMember(x => x.Id, y => y.Ignore())
                 .ForMember(x => x.GroupName, y => y.MapFrom(o => o.AccountantGroup.Name))
                 .ReverseMap();

            //信頭基本資料
            CreateMap<Letterhead, LetterheadViewModel>();

            //臨時章Log使用
            CreateMap<TemporarySealViewModel, TemporarySealLogModel>();
            CreateMap<TemporarySealDetailViewModel, TemporarySealDetailLogModel>()
                .ForMember(x => x.ViewModels, y => y.Ignore())                
                .ReverseMap();

            //客戶印鑑樣板使用
            CreateMap<CustomerSealTemplateForm, CustomerSealTemplate>();
            CreateMap<CustomerSealTemplateLocationForm, CustomerSealTemplateLocation>()
                .ForMember(x => x.ConfigType , y => y.MapFrom(o => o.CustomerSealType))
                .ReverseMap();
            //客戶印鑑樣板異動使用
            CreateMap<CustomerSealTemplateUpdateForm, CustomerSealTemplate>();
            CreateMap<CustomerSealTemplateLocationUpdateForm, CustomerSealTemplateLocation>()
                .ForMember(x => x.ConfigType , y => y.MapFrom(o => o.CustomerSealType))
                .ReverseMap();
            //客戶印鑑樣板單筆查詢使用
            CreateMap<CustomerSealTemplate, CustomerSealTemplateDetailViewModel>();
            CreateMap<CustomerSealTemplateLocation, CustomerSealTemplateLocationViewModel>()
                .ForMember(x => x.CustomerSealType, y => y.MapFrom(o => o.ConfigType))
                .ReverseMap();
            //Log使用
            CreateMap<CustomerSealTemplateViewModel, CustomerSealTemplateLogModel>();
            CreateMap<CustomerSealTemplatePaginate, CustomerSealTemplatePaginateLog>();


            //會計師簽印樣板使用
            CreateMap<AccountantSignTemplateForm, AccountantSignTemplate>();
            CreateMap<AccountantSignTemplateLocationForm, AccountantSignTemplateLocation>()
                .ForMember(x => x.ConfigType, y => y.MapFrom(o => o.AccountantSignType))
                .ReverseMap();
            //會計師簽印樣板異動使用
            CreateMap<AccountantSignTemplateUpdateForm, AccountantSignTemplate>();
            CreateMap<AccountantSignTemplateLocationUpdateForm, AccountantSignTemplateLocation>()
                .ForMember(x => x.ConfigType, y => y.MapFrom(o => o.AccountantSignType))
                .ReverseMap();
            //會計師簽印樣板單筆查詢使用
            CreateMap<AccountantSignTemplate, AccountantSignTemplateDetailViewModel>();
            CreateMap<AccountantSignTemplateLocation, AccountantSignTemplateLocationViewModel>()
                .ForMember(x => x.AccountantSignType, y => y.MapFrom(o => o.ConfigType))
                .ReverseMap();
            //Log使用
            CreateMap<AccountantSignTemplateViewModel, AccountantSignTemplateLogModel>();
            CreateMap<AccountantSignTemplatePaginate, AccountantSignTemplatePaginateLog>();


            //信頭樣板使用
            CreateMap<LetterheadImageTemplateForm, LetterheadImageTemplate>();
            CreateMap<LetterheadImageTemplateLocationForm, LetterheadImageTemplateLocation>();
            //信頭樣板異動使用
            CreateMap<LetterheadImageTemplateUpdateForm, LetterheadImageTemplate>();
            CreateMap<LetterheadImageTemplateLocationUpdateForm, LetterheadImageTemplateLocation>();
            //信頭樣板單筆查詢使用
            CreateMap<LetterheadImageTemplate, LetterheadImageTemplateDetailViewModel>();
            CreateMap<LetterheadImageTemplateLocation, LetterheadImageTemplateLocationViewModel>();
            //Log使用
            CreateMap<LetterheadImageTemplateViewModel, LetterheadImageTemplateLogModel>();
            CreateMap<LetterheadImageTemplatePaginate, LetterheadImageTemplatePaginateLog>();


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
