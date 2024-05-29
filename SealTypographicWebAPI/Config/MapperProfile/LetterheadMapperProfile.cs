using AutoMapper;
using DBEntities.Entities;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Models.Upload;

namespace SealTypographicWebAPI.Config.MapperProfile
{
    /// <summary>
    /// AutoMapper用的LIST
    /// </summary>
    public class LetterheadMapperProfile : Profile
    {
        /// <summary>
        /// 建置
        /// </summary>
        public LetterheadMapperProfile()
        {
            CreateMap<LetterheadImageUpdate, LetterheadImageUpdate>()
                    .ForMember(dst => dst.ImageBase64, opt => opt.MapFrom(src => src.ImageBase64 != null ? "Image/base64..." : null));                    

        }
    }
}
