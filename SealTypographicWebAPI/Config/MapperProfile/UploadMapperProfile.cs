using AutoMapper;
using DBEntities;
using SealTypographicWebAPI.Utils;
using SealTypographicWebAPI.Models.QuarterYear;
using SealTypographicWebAPI.Models.Upload;

namespace SealTypographicWebAPI.Config.MapperProfile
{
    /// <summary>
    /// AutoMapper用的LIST
    /// </summary>
    public class UploadMapperProfile : Profile
    {
        /// <summary>
        /// 建置
        /// </summary>
        public UploadMapperProfile()
        {
            CreateMap<UploadFile, UploadViewModel>()                    
                    .ForMember(dst => dst.FileName, opt => opt.MapFrom(src => src.OriginalFileName))
                    .ForMember(dst => dst.UploadDate, opt => opt.MapFrom(src => src.CreateDate));

            CreateMap<UploadFile, UploadFileViewModel>()
                    .ForMember(dst => dst.FileName, opt => opt.MapFrom(src => src.OriginalFileName))
                    .ForMember(dst => dst.UploadDate, opt => opt.MapFrom(src => src.CreateDate));

        }
    }
}
