using SealTypographicWebAPI.Consts;
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
        public Response Get(ResponseCode responseCode)
        {
            switch (responseCode)
            {
                case ResponseCode.Success:
                    return new Response()
                    {
                        Code = 200,
                        Message = "Success"
                    };
                case ResponseCode.InternalServerError:
                    return new Response()
                    {
                        Code = 500,
                        Message = "error"
                    };
                default:
                    return new Response()
                    {
                        Code = 200,
                        Message = "Success"
                    };
            }                      
        }
    }
}
