using AutoMapper;
using SealTypographicWebAPI.Models.CustomerSealReview;
using SealTypographicWebAPI.Models.AccountantSignReview;
using SealTypographicWebAPI.Utils;
using DBEntities.Consts;
using SealTypographicWebAPI.Models.CustomerSeal;
using DBEntities.Entities.TypographicModels;
using DBEntities.Entities.CustomerModels;
using DBEntities.Entities.AccountantModels;

namespace SealTypographicWebAPI.Config.MapperProfile
{
    /// <summary>
    /// AutoMapper用的LIST
    /// </summary>
    public class SealReviewMapperProfile : Profile
    {
        /// <summary>
        /// 建置
        /// </summary>
        public SealReviewMapperProfile()
        {

            //客戶印鑑季度審核清單
            CreateMap<CustomerSealGroup, CustomerSealReviewViewModel>()
                    .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Customer.Name))
                    .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Customer.Code))
                    .ForMember(dst => dst.QuarterYearId, opt => opt.MapFrom(src => src.QuarterYear.Id))
                     //Log Save
                     .ForMember(dst => dst.CustomerId, opt => opt.MapFrom(src => src.Customer.Id))
                     .ForMember(dst => dst.GregorainQuarterYear, opt => opt.MapFrom(src =>
                        src.QuarterYear.Type == TypographyType.FinancialReport ?
                        QuarterUtil.GetGregorainQuarter(src.QuarterYear) : QuarterUtil.GetGregorainYear(src.QuarterYear)
                    ))
                    .ForMember(dst => dst.DisplayQuarterYear, opt => opt.MapFrom(src =>
                        src.QuarterYear.Type == TypographyType.FinancialReport ?
                        QuarterUtil.GetTaiwanYearQuarter(src.QuarterYear) : QuarterUtil.GetTaiwanYear(src.QuarterYear)
                    ))
                    .ForMember(dst => dst.SealImageInfos, opt => opt.MapFrom(src => src.TypographicResources.Where(x => x.DeleteStatus == DeleteStatus.No)
                    .OrderBy(x => x.SubSealType).ThenBy(x => x.Sequence)));

            CreateMap<TypographicResource, SealImageInfo>()
                    .ForMember(dst => dst.SealMappingConfigId, opt => opt.MapFrom(src => SealMappingConfigUtil.GetCustomerSealType(src.SubSealType)));

            CreateMap<CustomerSealGroup, CustomerSealQuarterViewModel>()
                    .ForMember(dst => dst.QuarterYearId, opt => opt.MapFrom(src => src.QuarterYear.Id));


            //客戶印鑑審核詳細資料
            CreateMap<CustomerSealGroup, CustomerSealDetailReviewViewModel>()
                     //客戶基本資料
                     .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Customer.Name))
                     .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Customer.Code))
                     //Log Save
                     .ForMember(dst => dst.CustomerId, opt => opt.MapFrom(src => src.Customer.Id))
                     .ForMember(dst => dst.GregorainQuarterYear, opt => opt.MapFrom(src =>
                        src.QuarterYear.Type == TypographyType.FinancialReport ?
                        QuarterUtil.GetGregorainQuarter(src.QuarterYear) : QuarterUtil.GetGregorainYear(src.QuarterYear)
                    ))
                    .ForMember(dst => dst.DisplayQuarterYear, opt => opt.MapFrom(src =>
                        src.QuarterYear.Type == TypographyType.FinancialReport ?
                        QuarterUtil.GetTaiwanYearQuarter(src.QuarterYear) : QuarterUtil.GetTaiwanYear(src.QuarterYear)
                    ))
                     //印鑑與季度相關
                     .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                     .ForMember(dst => dst.QuarterYearId, opt => opt.MapFrom(src => src.QuarterYear.Id))
                     .ForMember(dst => dst.Seals, opt => opt.MapFrom(src => src.TypographicResources.Where(x => x.DeleteStatus == DeleteStatus.No).OrderBy(x => x.SubSealType)));

            //會計師簽印審核清單
            CreateMap<AccountantSignGroup, AccountantSignReviewViewModel>()
                    .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Accountant.Name))
                    .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Accountant.Code))
                    .ForMember(dst => dst.GroupName, opt => opt.MapFrom(src => src.Accountant.AccountantGroups.First().Name))
                    .ForMember(dst => dst.ReviewStatus, opt => opt.MapFrom(src => src.ReviewStatus))
                    //Log Save
                    .ForMember(dst => dst.AccountantId, opt => opt.MapFrom(src => src.Accountant.Id))
                    .ForMember(dst => dst.GroupCreateDate, opt => opt.MapFrom(src => src.CreateDate.DateTime))
                    .ForMember(dst => dst.SignImageInfos, opt => opt.MapFrom(src => src.TypographicResources.Where(x => x.DeleteStatus == DeleteStatus.No)
                    .OrderBy(x => x.SubSealType)));

            //會計師簽印審核詳細資料的簽印部份
            CreateMap<TypographicResource, SignImageInfo>()
                     .ForMember(dst => dst.SealMappingConfigId, opt => opt.MapFrom(src => SealMappingConfigUtil.GetAccountantSignType(src.SubSealType)));

            //會計師簽印審核詳細資料
            CreateMap<AccountantSignGroup, AccountantSignDetailReviewViewModel>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Accountant.Name))
                    .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Accountant.Code))
                    //Log Save
                    .ForMember(dst => dst.AccountantId, opt => opt.MapFrom(src => src.Accountant.Id))
                    .ForMember(dst => dst.GroupCreateDate, opt => opt.MapFrom(src => src.CreateDate.DateTime.ToLocalTime()))
                    //TODO 之後要想辦法改成顯示多個Name
                    .ForMember(dst => dst.GroupName, opt => opt.MapFrom(src => src.Accountant.AccountantGroups.First().Name))
                    .ForMember(dst => dst.Signs, opt => opt.MapFrom(src => src.TypographicResources.Where(x => x.DeleteStatus == DeleteStatus.No)));


            //會計師簽印審核詳細資料
            CreateMap<Accountant, AccountantSignDetailReviewViewModel>()
                     .ForMember(dst => dst.Id, y => y.Ignore())
                     //.ForMember(dst => dst.GroupName, y => y.MapFrom(o => o.AccountantGroup.Name))
                     .ForMember(dst => dst.GroupName, y => y.MapFrom(src => src.AccountantGroups.First().Name))
                     .ReverseMap();

        }
    }
}
