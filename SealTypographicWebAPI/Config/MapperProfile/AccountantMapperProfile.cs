using AutoMapper;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Models.AccountantGroup;
using SealTypographicWebAPI.Models.TemporarySeal;
using SealTypographicWebAPI.Utils;
using DBEntities.Consts;
using SealTypographicWebAPI.Models.AccountantGroupMember;
using DBEntities.Entities;
using DBEntities.Entities.TypographicModels;
using DBEntities.Entities.TemplateModels;
using DBEntities.Entities.AccountantModels;
using DJImageLib.Utils;

namespace SealTypographicWebAPI.Config.MapperProfile
{
    /// <summary>
    /// AutoMapper用的LIST
    /// </summary>
    public class AccountantMapperProfile : Profile
    {
        /// <summary>
        /// 建置
        /// </summary>
        public AccountantMapperProfile()
        {
            //會計師基本資料
            CreateMap<Accountant, AccountantViewModel>()
                    .ForMember(dst => dst.AccountantNumber, opt => opt.MapFrom(src => src.Code))
                    .ForMember(dst => dst.AccountantGroupInfos, opt => opt.MapFrom(src => src.AccountantGroups));

            CreateMap<AccountantGroup, AccountantGroupInfo>()
                    .ForMember(dst => dst.AccountantGroupId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.AccountantGroupName, opt => opt.MapFrom(src => src.Name));

            //會計師分頁顯示Map
            CreateMap<Accountant, AccountantViewModelWithCreateDate>()
                    .ForMember(dst => dst.AccountantGroupNames, opt => opt.MapFrom(o => o.AccountantGroups.Select(x => x.Name)))
                    .ForMember(dst => dst.AccountantNumber, opt => opt.MapFrom(o => o.Code))
                    .ForMember(dst => dst.AccountantSignGroupId, opt => opt.MapFrom
                    (
                        src => src.AccountantSignGroups
                                .Where(x => x.ReviewStatus <= ReviewStatus.Pending)
                                .OrderByDescending(x => x.Id)                                
                                .Select(x => x.Id)
                                .FirstOrDefault()
                    ));

            CreateMap<AccountantForm, Accountant>()
                     .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.AccountantNumber))
                     .ReverseMap();

            CreateMap<AccountantUpdateForm, Accountant>()
                     .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.AccountantNumber))
                     .ReverseMap();

            //會計師群組成員
            CreateMap<Accountant, AccountantGroupMember>()
                     .ForMember(dst => dst.AccountantNumber, opt => opt.MapFrom(src => src.Code));

            //會計師群組成員
            CreateMap<GroupAccountant, AccountantGroupMember>()
                     .ForMember(dst => dst.AccountantNumber, opt => opt.MapFrom(src => src.Accountant.Code))
                     .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Accountant.Id))
                     .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Accountant.Name));

            //會計師群組
            CreateMap<AccountantGroup, AccountantGroupViewModel>()
                    .ForMember(dst => dst.AccountantGroupNumber, opt => opt.MapFrom(src => src.Code));
            CreateMap<AccountantGroupForm, AccountantGroup>()
                    .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.AccountantGroupNumber));
            CreateMap<AccountantGroupUpdateForm, AccountantGroup>();

            CreateMap<AccountantSignGroup, AccountantSignGroupViewModel>()
                    .ForMember(dst => dst.GroupCreateDate, opt => opt.MapFrom(src => src.CreateDate.DateTime.ToLocalTime()));

            //會計師印鑑
            CreateMap<TypographicResource, AccountantSignViewModel>()
                    .ForMember(dst => dst.ImageBase64, opt => opt.Ignore()); // <---imagebase64要額外處理所以要忽略

            CreateMap<AccountantSignGroup, AccountantSignViewModels>()
                    .ForMember(dst => dst.AccountantSignGroupId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.GroupCreateDate, opt => opt.MapFrom(src => src.CreateDate.DateTime.ToLocalTime()))
                    .ForMember(dst => dst.ReviewStatus, opt => opt.MapFrom(src => src.ReviewStatus))
                    //--Log Save--//
                    .ForMember(dst => dst.AccountantId, opt => opt.MapFrom(src => src.Accountant.Id))
                    .ForMember(dst => dst.AccountantName, opt => opt.MapFrom(src => src.Accountant.Name))
                    //--Log Save--//
                    .ForMember(dst => dst.SignViewModels, opt => opt.MapFrom(src => src.TypographicResources.Where(x => x.DeleteStatus == DeleteStatus.No)));

            CreateMap<TypographicResource, AccountantSignViewModel>()
                    .ForMember(dst => dst.SealMappingConfigId, opt => opt.MapFrom(src => SealMappingConfigUtil.GetAccountantSignType(src.SubSealType)));                    
                    //.ForMember(dst => dst.ImageBase64, opt => opt.MapFrom(src => ImageUtil.ToDataUrlFromFilePath(src.ImageFullPath)));

            CreateMap<AccountantSign, TypographicResource>()
                    .ForMember(dst => dst.ImageFullPath, opt => opt.Ignore()); // <---imagebase64要額外處理所以要忽略


            CreateMap<AccountantSignUpdateForm, TypographicResource>()
                    .ForMember(dst => dst.ImageFullPath, opt => opt.Ignore()) // <---imagebase64要額外處理所以要忽略
                    .ReverseMap();

            //會計師簽印重複確認
            CreateMap<AccountantSign, AccountantSignCheck>();
            CreateMap<AccountantSignUpdateForm, AccountantSignCheck>();

            //信頭基本資料
            CreateMap<Letterhead, LetterheadViewModel>()
                      .ForMember(dst => dst.LetterheadImageId, opt => opt.MapFrom(src => src.TypographicResources.Single(x => x.DeleteStatus == DeleteStatus.No).Id));

            //臨時章Log使用
            CreateMap<TemporarySealDetailViewModel, TemporarySealDetailLogModel>();
            //subList
            CreateMap<TemporarySealViewModel, TemporarySealLogModel>();


            CreateMap<TemporarySealGroup, TemporarySealDetailViewModel>()
                    .ForMember(dst => dst.CustomerId, opt => opt.MapFrom(src => src.Customer.Id))
                    .ForMember(dst => dst.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
                    .ForMember(dst => dst.ViewModels, opt => opt.MapFrom(src => src.TypographicResources
                                                                        .Where(x => x.DeleteStatus == DeleteStatus.No)
                                                                        .OrderBy(x => x.Sequence)));

            CreateMap<TypographicResource, TemporarySealViewModel>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Sequence, opt => opt.MapFrom(src => src.Sequence))
                    .ForMember(dst => dst.ImageFullPath, opt => opt.MapFrom(src => src.ImageFullPath))
                    .ForMember(dst => dst.ImageBase64, opt => opt.MapFrom(src => ImageUtil.ToDataUrlFromFilePath(src.ImageFullPath)));

            CreateMap<TemporarySealGroup, TemporaryViewModel>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
                    .ForMember(dst => dst.Quarter, opt => opt.MapFrom(src => QuarterUtil.GetTaiwanYearQuarter(src.QuarterYear)));
        }
    }
}
