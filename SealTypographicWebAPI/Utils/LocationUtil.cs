using AutoMapper;
using SealTypographicWebAPI.Entities;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class LocationUtil<T1,T2>
    {        
        private readonly IMapper mapper;

        /// <summary>
        /// /// 取得Automapper
        /// </summary>
        /// <param name="mapper"></param>
        public LocationUtil(IMapper mapper)
        {
            this.mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locations"></param>
        /// <returns></returns>
        public List<T1> Locations(List<T2> locations)
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
