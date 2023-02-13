using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 取得回應代碼列表
    /// </summary>
    public class ResponseCodeService
    {

        /// <summary>
        /// 取得回應代碼列表
        /// </summary>
        /// <returns></returns>
        public ResponseCodeList Get()
        {
            ResponseCodeList responseCodeList = new();

            foreach (ResponseCode responseCode in (ResponseCode[])Enum.GetValues(typeof(ResponseCode)))
            {
                ResponseCodeViewModel responseCodeViewModel = new()
                {
                    Code = (int)responseCode,
                    Description = responseCode.GetDescription()
                };
                responseCodeList.ViewModels.Add(responseCodeViewModel);
            }
            
            return responseCodeList;
        }        
    }
}
