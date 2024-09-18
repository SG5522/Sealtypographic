using Microsoft.Extensions.Localization;
using Microsoft.OpenApi.Extensions;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Consts;
using Serilog;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 取得回應代碼列表
    /// </summary>
    public class ResponseCodeService
    {
        private readonly IStringLocalizer<ResponseCodeService> localizer;
        private readonly ILogger<ResponseCodeService> logger;

        /// <summary>
        /// IStringLocalizer
        /// </summary>
        /// <param name="localizer"></param>
        /// <param name="logger"></param>      
        public ResponseCodeService(IStringLocalizer<ResponseCodeService> localizer, ILogger<ResponseCodeService> logger)
        {
            this.localizer = localizer;
            this.logger = logger;
        }

        /// <summary>
        /// 取得回應代碼列表
        /// </summary>
        /// <returns></returns>
        public ResponseCodeList Get()
        {
            ResponseCodeList responseCodeList = new();

            try
            {
                foreach (ResponseCode responseCode in (ResponseCode[])Enum.GetValues(typeof(ResponseCode)))
                {
                    ResponseCodeViewModel responseCodeViewModel = new()
                    {
                        Code = (int)responseCode,
                        Description = localizer[responseCode.GetDisplayName()]
                    };
                    responseCodeList.ViewModels.Add(responseCodeViewModel);
                }                
            }
            catch (Exception ex) 
            {
                logger.LogError("Get error {@Error}", ex.Message);
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
