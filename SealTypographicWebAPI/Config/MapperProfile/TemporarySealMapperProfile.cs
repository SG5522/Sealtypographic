using AutoMapper;
using SealTypographicWebAPI.Models.TemporarySeal;
using SealTypographicWebAPI.Utils;
using DBEntities.Consts;
using DBEntities.Entities.TypographicModels;
using DBEntities.Entities.TemplateModels;
using DJImageLib.Utils;

namespace SealTypographicWebAPI.Config.MapperProfile
{
    /// <summary>
    /// AutoMapper用的LIST
    /// </summary>
    public class TemporarySealMapperProfile : Profile
    {
        /// <summary>
        /// 建置
        /// </summary>
        public TemporarySealMapperProfile()
        {            
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
                    .ForMember(dst => dst.ImageFullPath, opt => opt.MapFrom(src => src.ImageFullPath));

            CreateMap<TemporarySealGroup, TemporaryViewModel>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
                    .ForMember(dst => dst.Quarter, opt => opt.MapFrom(src => QuarterUtil.GetTaiwanYearQuarter(src.QuarterYear)));
        }
    }
}
