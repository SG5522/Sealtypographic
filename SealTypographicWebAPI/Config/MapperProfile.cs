using AutoMapper;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Models.AccountantGroup;
using SealTypographicWebAPI.Models.CustomerSealReview;
using SealTypographicWebAPI.Models.AccountantSignReview;
using SealTypographicWebAPI.Models.TemporarySeal;

using DBEntities;

namespace SealTypographicWebAPI.Config
{
    /// <summary>
    /// AutoMapper用的LIST
    /// </summary>
    public class MapperProfile : Profile
    {        
        /// <summary>
        /// 建置
        /// </summary>
        public MapperProfile()
        {            
            //客戶基本資料
            CreateMap<Customer, CustomerDetail>();
            CreateMap<Customer, CustomerViewModel>();
            CreateMap<CustomerForm, Customer>();
            CreateMap<CustomerUpdateForm, Customer>();

            //客戶印鑑
            CreateMap<CustomerSealGroup, CustomerSealViewModel>()
                    .ForMember(x => x.ImageBase64, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略                    
                    .ReverseMap();

            CreateMap<CustomerSeal, TypographicResource>()
                    .ForMember(x => x.ImageFullPath, y => y.Ignore()) // <---ImagePath要額外處理所以要忽略                    
                    .ReverseMap();
               
            CreateMap<CustomerSealViewModel, TypographicResource>()
                    .ForMember(x => x.ImageFullPath, y => y.Ignore()) // <---ImagePath要額外處理所以要忽略  
                    .ReverseMap();

            CreateMap<CustomerSealUpdateForm, TypographicResource>()
                    .ForMember(x => x.ImageFullPath, y => y.Ignore()) // <---ImagePath要額外處理所以要忽略
                    .ReverseMap();


            //客戶印鑑季度審核清單
            CreateMap<CustomerSealGroup, CustomerSealQuarterReviewViewModel>()                    
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
            CreateMap<AccountantGroupUpdateForm, AccountantGroup>();            

            //會計師印鑑
            CreateMap<TypographicResource, AccountantSignViewModel>()                    
                    .ForMember(x => x.ImageBase64, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();
            
            CreateMap<AccountantSignGroup, AccountantSignViewModel>();

            CreateMap<AccountantSign, TypographicResource>()
                    .ForMember(x => x.ImageFullPath, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();

            CreateMap<AccountantSignUpdateForm, TypographicResource>()
                    .ForMember(x => x.ImageFullPath, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();

            //會計師簽印重複確認
            CreateMap<AccountantSign, AccountantSignCheck>();
            CreateMap<AccountantSignUpdateForm, AccountantSignCheck>();

            //會計師簽印審核清單
            CreateMap<AccountantSignGroup, AccountantSignGroupReviewViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Accountant.Name))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(o => o.Accountant.Code))
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(o => o.Accountant.AccountantGroup.Name))
                .ForMember(dest => dest.ReviewStatus, opt => opt.MapFrom(o => o.ReviewStatus))
                .ReverseMap();

            //會計師簽印審核詳細資料
            CreateMap<Accountant, AccountantSignGroupDetailReviewViewModel>()
                 .ForMember(dest => dest.Id, y => y.Ignore())
                 .ForMember(dest => dest.GroupName, y => y.MapFrom(o => o.AccountantGroup.Name))
                 .ReverseMap();

            //信頭基本資料
            CreateMap<Letterhead, LetterheadViewModel>();

            //臨時章Log使用
            CreateMap<TemporarySealViewModel, TemporarySealLogModel>();
            CreateMap<TemporarySealDetailViewModel, TemporarySealDetailLogModel>()
                .ForMember(x => x.ViewModels, y => y.Ignore())                
                .ReverseMap();                 
        }
    }
}
