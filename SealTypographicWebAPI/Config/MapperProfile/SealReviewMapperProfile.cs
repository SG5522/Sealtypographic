using AutoMapper;
using SealTypographicWebAPI.Models.CustomerSealReview;
using SealTypographicWebAPI.Models.AccountantSignReview;
using DBEntities;
using DJLib;
using SealTypographicWebAPI.Utils;
using DBEntities.Consts;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.CustomerSeal;

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
            CreateMap<CustomerSealGroup, CustomerSealGroupReviewViewModel>()
                    .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Customer.Name))
                    .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Customer.Code))
                    .ForMember(dst => dst.Quarter, opt => opt.MapFrom(src => QuarterUtil.GetTaiwanYearQuarter(src.QuarterYear)))
                    .ForMember(dst => dst.SealImageInfos, opt => opt.MapFrom(src => src.TypographicResources.Where(x => x.DeleteStatus == DeleteStatus.No)));

            CreateMap<TypographicResource, SealImageInfo>()
                 .ForMember(dst => dst.ThumbnailBase64, opt => opt.MapFrom(src => ImageSharpUtil.PathImageFileToBase64(src.ThumbnailFullPath)))
                 .ForMember(dst => dst.SealMappingConfigId, opt => opt.MapFrom(src => SealMappingConfigUtil.GetCustomerSealType(src.SubSealType)));

            CreateMap<CustomerSealGroup, CustomerSealQuarterViewModel>()
                    .ForMember(dst => dst.QuarterYearId, opt => opt.MapFrom(src => src.QuarterYear.Id));


            //客戶印鑑審核詳細資料
            CreateMap<CustomerSealGroup, CustomerSealGroupDetailReviewViewModel>()
                     //客戶基本資料
                     .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Customer.Name))
                     .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Customer.Code))
                     //印鑑與季度相關
                     .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                     .ForMember(dst => dst.QuarterYearId, opt => opt.MapFrom(src => src.QuarterYear.Id))
                     .ForMember(dst => dst.Seals, opt => opt.MapFrom(src => src.TypographicResources.Where(x => x.DeleteStatus == DeleteStatus.No)));

            //客戶印鑑審核詳細資料的印鑑部份
            CreateMap<TypographicResource, CustomerSealViewModel>()
                     .ForMember(dst => dst.SealMappingConfigId, opt => opt.MapFrom(src => SealMappingConfigUtil.GetCustomerSealType(src.SubSealType)))
                     .ForMember(dst => dst.ImageBase64, opt => opt.MapFrom(src => ImageSharpUtil.PathImageFileToBase64(src.ImageFullPath)));


            //會計師簽印審核清單
            CreateMap<AccountantSignGroup, AccountantSignGroupReviewViewModel>()
                    .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Accountant.Name))
                    .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Accountant.Code))
                    .ForMember(dst => dst.GroupName, opt => opt.MapFrom(src => src.Accountant.AccountantGroups.First().Name))
                    .ForMember(dst => dst.ReviewStatus, opt => opt.MapFrom(src => src.ReviewStatus))
                    .ForMember(dst => dst.SignImageInfos, opt => opt.MapFrom(src => src.TypographicResources.Where(x => x.DeleteStatus == DeleteStatus.No)));

            //會計師簽印審核詳細資料的簽印部份
            CreateMap<TypographicResource, SignImageInfo>()
                     .ForMember(dst => dst.SealMappingConfigId, opt => opt.MapFrom(src => SealMappingConfigUtil.GetAccountantSignType(src.SubSealType)))
                     .ForMember(dst => dst.ThumbnailBase64, opt => opt.MapFrom(src => ImageSharpUtil.PathImageFileToBase64(src.ThumbnailFullPath)));

            //會計師簽印審核詳細資料
            CreateMap<AccountantSignGroup, AccountantSignGroupDetailReviewViewModel>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Accountant.Name))
                    .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Accountant.Code))
                    //.ForMember(dst => dst.GroupName, opt => opt.MapFrom(src => src.Accountant.AccountantGroup.Name))
                    .ForMember(dst => dst.GroupName, opt => opt.MapFrom(src => src.Accountant.AccountantGroups.First().Name))
                    .ForMember(dst => dst.Signs, opt => opt.MapFrom(src => src.TypographicResources.Where(x => x.DeleteStatus == DeleteStatus.No)));

            //會計師簽印審核詳細資料的簽印部份
            CreateMap<TypographicResource, AccountantSignViewModel>()
                     .ForMember(dst => dst.SealMappingConfigId, opt => opt.MapFrom(src => SealMappingConfigUtil.GetAccountantSignType(src.SubSealType)))
                     .ForMember(dst => dst.ImageBase64, opt => opt.MapFrom(src => ImageSharpUtil.PathImageFileToBase64(src.ImageFullPath)));

            //會計師簽印審核詳細資料
            CreateMap<Accountant, AccountantSignGroupDetailReviewViewModel>()
                     .ForMember(dst => dst.Id, y => y.Ignore())
                     //.ForMember(dst => dst.GroupName, y => y.MapFrom(o => o.AccountantGroup.Name))
                     .ForMember(dst => dst.GroupName, y => y.MapFrom(src => src.AccountantGroups.First().Name))
                     .ReverseMap();

        }
    }
}
