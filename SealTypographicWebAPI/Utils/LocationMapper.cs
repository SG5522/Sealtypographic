using AutoMapper;
using SealTypographicWebAPI.Entities;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class LocationMapper<T1,T2>
    {        
        private readonly IMapper mapper;

        /// <summary>
        /// /// 取得Automapper
        /// </summary>
        /// <param name="mapper"></param>
        public LocationMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locations"></param>
        /// <returns></returns>
        public List<T1> LocationsMappper(List<T2> locations)
        {
            List<T1> dBlocations = new();
            foreach (T2 location in locations)
            {
                dBlocations.Add(mapper.Map<T1>(location));
            }
            return dBlocations;
        }
    }
}
