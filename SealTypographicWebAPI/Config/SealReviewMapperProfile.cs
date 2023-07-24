using AutoMapper;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.CustomerSealReview;
using SealTypographicWebAPI.Models.AccountantSignReview;
using DBEntities;
using DJLib;
using SealTypographicWebAPI.Utils;
using DBEntities.Consts;
using SealTypographicWebAPI.Models.Accountant;

namespace SealTypographicWebAPI.Config
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
            CreateMap<CustomerSealGroup, CustomerSealQuarterReviewViewModel>()
                    .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Customer.Name))
                    .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Customer.Code))
                    .ForMember(dst => dst.Quarter, opt => opt.MapFrom(src => QuarterUtil.GetTaiwanYearQuarter(src.Quarter)))
                    .ForMember(dst => dst.SealImageInfos, opt => opt.MapFrom(src => src.TypographicResources.Where(x => x.DeleteStatus == DeleteStatus.No)));

            CreateMap<TypographicResource, SealImageInfo>()
                 .ForMember(dst => dst.ThumbnailBase64, opt => opt.MapFrom(src => ImageSharpUtil.PathImageFileToBase64(src.ThumbnailFullPath)))
                 .ForMember(dst => dst.SealMappingConfigId, opt => opt.MapFrom(src => SealMappingConfigUtil.GetCustomerSealType(src.SubSealType)));

            CreateMap<CustomerSealGroup, CustomerSealQuarterViewModel>()
                    .ForMember(dst => dst.Quarter, opt => opt.MapFrom(src => QuarterUtil.GetTaiwanYearQuarter(src.Quarter)));


            //客戶印鑑審核詳細資料
            CreateMap<CustomerSealGroup, CustomerSealQuarterDetailReviewViewModel>()
                     //客戶基本資料
                     .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Customer.Name))
                     .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Customer.Code))
                     .ForMember(dst => dst.BAN, opt => opt.MapFrom(src => src.Customer.BAN))
                     .ForMember(dst => dst.President, opt => opt.MapFrom(src => src.Customer.President))
                     .ForMember(dst => dst.StockCode, opt => opt.MapFrom(src => src.Customer.StockCode))
                     .ForMember(dst => dst.PostalCode, opt => opt.MapFrom(src => src.Customer.PostalCode))
                     .ForMember(dst => dst.AddressArea, opt => opt.MapFrom(src => src.Customer.AddressArea))
                     .ForMember(dst => dst.AddressCity, opt => opt.MapFrom(src => src.Customer.AddressCity))
                     .ForMember(dst => dst.AddressLocate, opt => opt.MapFrom(src => src.Customer.AddressLocate))
                     .ForMember(dst => dst.AddressStreet, opt => opt.MapFrom(src => src.Customer.AddressStreet))
                     .ForMember(dst => dst.Telephone, opt => opt.MapFrom(src => src.Customer.Telephone))
                     .ForMember(dst => dst.Fax, opt => opt.MapFrom(src => src.Customer.Fax))
                     .ForMember(dst => dst.ContactName, opt => opt.MapFrom(src => src.Customer.ContactName))
                     .ForMember(dst => dst.ContactTitle, opt => opt.MapFrom(src => src.Customer.ContactTitle))
                     .ForMember(dst => dst.ContactTelephone, opt => opt.MapFrom(src => src.Customer.ContactTelephone))
                     //印鑑與季度相關
                     .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                     .ForMember(dst => dst.Quarter, opt => opt.MapFrom(src => QuarterUtil.GetTaiwanYearQuarter(src.Quarter)))
                     .ForMember(dst => dst.Seals, opt => opt.MapFrom(src => src.TypographicResources.Where(x => x.DeleteStatus == DeleteStatus.No)));

            //客戶印鑑審核詳細資料的印鑑部份
            CreateMap<TypographicResource, CustomerSealViewModel>()
                     .ForMember(dst => dst.SealMappingConfigId, opt => opt.MapFrom(src => SealMappingConfigUtil.GetCustomerSealType(src.SubSealType)))
                     .ForMember(dst => dst.ImageBase64, opt => opt.MapFrom(src => ImageSharpUtil.PathImageFileToBase64(src.ImageFullPath)));                        


            //會計師簽印審核清單
            CreateMap<AccountantSignGroup, AccountantSignGroupReviewViewModel>()
                    .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Accountant.Name))
                    .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Accountant.Code))
                    .ForMember(dst => dst.GroupName, opt => opt.MapFrom(src => src.Accountant.AccountantGroup.Name))
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
                    .ForMember(dst => dst.GroupName, opt => opt.MapFrom(src => src.Accountant.AccountantGroup.Name))                    
                    .ForMember(dst => dst.Signs, opt => opt.MapFrom(src => src.TypographicResources.Where(x => x.DeleteStatus == DeleteStatus.No)));

            //會計師簽印審核詳細資料的簽印部份
            CreateMap<TypographicResource, AccountantSignViewModel>()
                     .ForMember(dst => dst.SealMappingConfigId, opt => opt.MapFrom(src => SealMappingConfigUtil.GetAccountantSignType(src.SubSealType)))
                     .ForMember(dst => dst.ImageBase64, opt => opt.MapFrom(src => ImageSharpUtil.PathImageFileToBase64(src.ImageFullPath)));

            //會計師簽印審核詳細資料
            CreateMap<Accountant, AccountantSignGroupDetailReviewViewModel>()
                     .ForMember(dst => dst.Id, y => y.Ignore())
                     .ForMember(dst => dst.GroupName, y => y.MapFrom(o => o.AccountantGroup.Name))
                     .ReverseMap();

        }
    }
}
