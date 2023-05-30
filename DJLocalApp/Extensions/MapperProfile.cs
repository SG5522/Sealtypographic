using AutoMapper;
using SealAPIWrap.Models;

namespace DJLocalApp.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class MapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapperProfile"/> class.
        /// </summary>
        public MapperProfile()
        {            
            CreateMap<ImageProcessRequest, ImageProcessForm>();
            CreateMap<SealBuildRequest, SealBuildForm>();
            CreateMap<SealIdentifyRequest, SealIdentifyForm>();
            CreateMap<SealShowRequest, SealShowForm>();
        }
    }
}
