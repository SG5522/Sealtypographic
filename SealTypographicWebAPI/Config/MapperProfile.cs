using AutoMapper;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Models.AccountantGroup;
using SealTypographicWebAPI.Models.CustomerSealReview;
using SealTypographicWebAPI.Models.AccountantSignReview;
using SealTypographicWebAPI.Models.TemporarySeal;

using DBEntities;
using DJLib;
using SealTypographicWebAPI.Utils;
using DBEntities.Consts;

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
            CreateMap<CustomerSealGroup, CustomerSealViewModels>()
                    .ForMember(dst => dst.CustomerSealQuarterId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Quarter, opt => opt.MapFrom(src => new string($"{src.Quarter.TaiwanYear}{src.Quarter.Period}")))
                    .ForMember(dst => dst.ReviewStatus, opt => opt.MapFrom(src => src.ReviewStatus))
                    .ForMember(dst => dst.SealViewModels, opt => opt.MapFrom(src => src.TypographicResources.Where(x => x.DeleteStatus == DeleteStatus.No)));

            CreateMap<TypographicResource, CustomerSealViewModel>()
                    .ForMember(dst => dst.SealMappingConfigId, opt => opt.MapFrom(src => SealMappingConfigUtil.GetCustomerSealType(src.SubSealType)))
                    .ForMember(dst => dst.ImageBase64, opt => opt.MapFrom(src => ImageSharpUtil.PathImageFileToBase64(src.ImageFullPath)));
                    
            CreateMap<CustomerSeal, TypographicResource>()
                    .ForMember(x => x.ImageFullPath, y => y.Ignore()) // <---ImagePath要額外處理所以要忽略                    
                    .ReverseMap();

            CreateMap<CustomerSealViewModel, TypographicResource>()
                    .ForMember(x => x.ImageFullPath, y => y.Ignore()); // <---ImagePath要額外處理所以要忽略  


            CreateMap<CustomerSealUpdateForm, TypographicResource>()
                    .ForMember(x => x.ImageFullPath, y => y.Ignore()) // <---ImagePath要額外處理所以要忽略
                    .ReverseMap();


            //客戶印鑑季度審核清單
            CreateMap<CustomerSealGroup, CustomerSealQuarterReviewViewModel>()                    
                    .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Customer.Name))
                    .ForMember(x => x.Code, y => y.MapFrom(o => o.Customer.Code))
                    .ForMember(x => x.Quarter, y => y.MapFrom(o => o.Quarter))
                    .ForMember(x => x.ReviewStatus, y => y.MapFrom(o => o.ReviewStatus))
                    .ReverseMap();

            CreateMap<CustomerSealGroup, CustomerSealQuarterViewModel>()
                    .ForMember(dst => dst.Quarter, opt => opt.MapFrom(src => QuarterUtil.GetTaiwanYearQuarter(src.Quarter)));


            //客戶印鑑審核詳細資料
            CreateMap<Customer, CustomerSealQuarterDetailReviewViewModel>()
                     .ForMember(x => x.Id, y => y.Ignore())
                     .ReverseMap();

            //會計師基本資料
            CreateMap<Accountant, AccountantViewModel>()                    
                    .ForMember(x => x.AccountantGroupName, y => y.MapFrom(o => o.AccountantGroup.Name))
                    .ForMember(x => x.AccountantNumber, y => y.MapFrom(o => o.Code))
                    .ReverseMap();

            //會計師分頁顯示Map
            CreateMap<Accountant, AccountantViewModelWithCreateDate>()
                    .ForMember(dst => dst.AccountantGroupName, opt => opt.MapFrom(o => o.AccountantGroup.Name))
                    .ForMember(dst => dst.AccountantNumber, opt => opt.MapFrom(o => o.Code))
                    .ForMember(dst => dst.AccountantSignGroupId, opt => opt.MapFrom
                    (
                        src => src.AccountantSignGroups.Any() ? src.AccountantSignGroups.First().Id : 0
                    ));;

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

            CreateMap<AccountantSignGroup, AccountantSignGroupViewModel>()                    
                    .ForMember(dst => dst.GroupCreateDate, opt => opt.MapFrom(src => src.CreateDate));

            //會計師印鑑
            CreateMap<TypographicResource, AccountantSignViewModel>()                    
                    .ForMember(x => x.ImageBase64, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();
            
            CreateMap<AccountantSignGroup, AccountantSignViewModels>()
                    .ForMember(dst => dst.AccountantSignGroupId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.GroupCreateDate, opt => opt.MapFrom(src => src.CreateDate))
                    .ForMember(dst => dst.ReviewStatus, opt => opt.MapFrom(src => src.ReviewStatus))
                    .ForMember(dst => dst.SignViewModels, opt => opt.MapFrom(src => src.TypographicResources.Where(x => x.DeleteStatus == DeleteStatus.No)));

            CreateMap<TypographicResource, AccountantSignViewModel>()
                    .ForMember(dst => dst.SealMappingConfigId, opt => opt.MapFrom(src => SealMappingConfigUtil.GetAccountantSignType(src.SubSealType)))
                    .ForMember(dst => dst.ImageBase64, opt => opt.MapFrom(src => ImageSharpUtil.PathImageFileToBase64(src.ImageFullPath)));

            CreateMap<AccountantSign, TypographicResource>()
                    .ForMember(x => x.ImageFullPath, y => y.Ignore()); // <---imagebase64要額外處理所以要忽略


            CreateMap<AccountantSignUpdateForm, TypographicResource>()
                    .ForMember(x => x.ImageFullPath, y => y.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();

            //會計師簽印重複確認
            CreateMap<AccountantSign, AccountantSignCheck>();
            CreateMap<AccountantSignUpdateForm, AccountantSignCheck>();

            //會計師簽印審核清單
            CreateMap<AccountantSignGroup, AccountantSignGroupReviewViewModel>()
                    .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Accountant.Name))
                    .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Accountant.Code))
                    .ForMember(dst => dst.GroupName, opt => opt.MapFrom(src => src.Accountant.AccountantGroup.Name))
                    .ForMember(dst => dst.ReviewStatus, opt => opt.MapFrom(src => src.ReviewStatus))
                    .ReverseMap();

            //會計師簽印審核詳細資料
            CreateMap<Accountant, AccountantSignGroupDetailReviewViewModel>()
                     .ForMember(dst => dst.Id, y => y.Ignore())
                     .ForMember(dst => dst.GroupName, y => y.MapFrom(o => o.AccountantGroup.Name))
                     .ReverseMap();

            //信頭基本資料
            CreateMap<Letterhead, LetterheadViewModel>()
                      .ForMember(dst => dst.LetterheadImageId, opt => opt.MapFrom(src => src.TypographicResources.Single(x => x.DeleteStatus == DeleteStatus.No).Id));

            //臨時章Log使用
            CreateMap<TemporarySealViewModel, TemporarySealLogModel>();
            CreateMap<TemporarySealDetailViewModel, TemporarySealDetailLogModel>()
                    .ForMember(dst => dst.ViewModels, y => y.Ignore());

            CreateMap<TemporarySealGroup, TemporarySealDetailViewModel>()
                    .ForMember(dst => dst.CustomerId, opt => opt.MapFrom(src => src.Customer.Id))
                    .ForMember(dst => dst.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
                    .ForMember(dst => dst.ViewModels, opt => opt.MapFrom(src => src.TypographicResources));

            CreateMap<TypographicResource, TemporarySealViewModel>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Sequence, opt => opt.MapFrom(src => src.Sequence))
                    .ForMember(dst => dst.ImageFullPath, opt => opt.MapFrom(src => src.ImageFullPath))
                    .ForMember(dst => dst.ImageBase64, opt => opt.MapFrom(src => ImageSharpUtil.PathImageFileToBase64(src.ImageFullPath)));
        }
    }
}
