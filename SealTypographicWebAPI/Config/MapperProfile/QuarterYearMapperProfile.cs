using AutoMapper;
using SealTypographicWebAPI.Models.TemporarySeal;
using DBEntities;
using DJLib;
using SealTypographicWebAPI.Utils;
using DBEntities.Consts;
using SealTypographicWebAPI.Models.QuarterYear;

namespace SealTypographicWebAPI.Config.MapperProfile
{
    /// <summary>
    /// AutoMapper用的LIST
    /// </summary>
    public class QuarterYearMapperProfile : Profile
    {
        /// <summary>
        /// 建置
        /// </summary>
        public QuarterYearMapperProfile()
        {
            CreateMap<QuarterYear, QuarterViewModel>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.GregorianQuarter, opt => opt.MapFrom(src => QuarterUtil.GetGregorainQuarter(src)))
                    .ForMember(dst => dst.DisplayQuarter, opt => opt.MapFrom(src => QuarterUtil.GetTaiwanYearQuarter(src)));

            CreateMap<QuarterYear, YearViewModel>()
                    .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.GregorianYear, opt => opt.MapFrom(src => QuarterUtil.GetGregorainYear(src)))
                    .ForMember(dst => dst.DisplayYear, opt => opt.MapFrom(src => QuarterUtil.GetTaiwanYear(src)));

        }
    }
}
