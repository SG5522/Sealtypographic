using AutoMapper;
using DBEntities.Entities;
using SealTypographicWebAPI.Models.Upload;
using SealTypographicWebAPI.Services.Implements;

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
                    .ForMember(dst => dst.UploadDate, opt => opt.MapFrom(src => src.CreateDate.DateTime.ToLocalTime()));

            CreateMap<UploadFile, UploadFileViewModel>()
                    .ForMember(dst => dst.FileName, opt => opt.MapFrom(src => src.OriginalFileName))
                    .ForMember(dst => dst.UploadDate, opt => opt.MapFrom(src => src.CreateDate.DateTime.ToLocalTime()));

        }
    }
}
