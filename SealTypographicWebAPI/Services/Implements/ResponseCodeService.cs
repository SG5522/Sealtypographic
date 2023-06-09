using Microsoft.Extensions.Localization;
using Microsoft.OpenApi.Extensions;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 取得回應代碼列表
    /// </summary>
    public class ResponseCodeService
    {
        private readonly IStringLocalizer<ResponseCodeService> localizer;

        /// <summary>
        /// IStringLocalizer
        /// </summary>
        /// <param name="localizer"></param>      
        public ResponseCodeService(IStringLocalizer<ResponseCodeService> localizer)
        {
            this.localizer = localizer;
        }

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
                    Description = localizer[responseCode.GetDisplayName()]
                };
                responseCodeList.ViewModels.Add(responseCodeViewModel);
            }
            
            return responseCodeList;
        }

        /// <summary>
        /// 依多國語系取得訊息
        /// </summary>
        /// <param name="responseCode">API傳輸結果代碼</param>
        /// <returns></returns>
        public string GetLocalizerMessage(ResponseCode responseCode) 
        {                        
            return localizer[responseCode.GetDisplayName()];
        }
    }
}
