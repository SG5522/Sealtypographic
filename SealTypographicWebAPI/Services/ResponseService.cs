using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public class ResponseService
    {
        /// <summary>
        /// 取得錯誤訊息
        /// </summary>
        /// <returns></returns>
        public Response Get()
        {
            Response data = new()
            {
                ResponseStatus = 404,
                ResponseMessage = "error"
            };
            return data;
        }
    }
}
