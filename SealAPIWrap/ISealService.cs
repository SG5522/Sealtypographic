using SealAPIWrap.Models;

namespace SealAPIWrap
{
    /// <summary>
    /// 印鑑處理
    /// </summary>
    public interface ISealService
    {
        /// <summary>
        /// 按照OPMode執行對應功能
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="opMode"></param>
        /// <param name="form">傳入資料</param>
        /// <returns></returns>
        K Operation<T, K>(OPMode opMode, T form) where T : ApiRequest where K : ApiResult;
    }
}
