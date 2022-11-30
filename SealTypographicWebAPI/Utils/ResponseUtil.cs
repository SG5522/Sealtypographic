using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Util
{
    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public class ResponseUtil
    {
        /// <summary>
        /// 回傳成功
        /// </summary>
        /// <returns></returns>
        public static ResponseViewModel Success()
        {
            return Get(ResponseCode.Success);
        }
        /// <summary>
        /// 回傳伺服器錯誤
        /// </summary>
        /// <returns></returns>
        public static ResponseViewModel InternalServerError()
        {
            return Get(ResponseCode.InternalServerError);
        }
        /// <summary>
        /// 回傳無資料
        /// </summary>
        /// <returns></returns>
        public static ResponseViewModel NoData()
        {
            return Get(ResponseCode.NoData);
        }
        /// <summary>
        /// 資料庫欄位限制唯一約束錯誤回傳
        /// </summary>
        /// <returns></returns>
        public static ResponseViewModel UniqueConstraintFailed()
        {
            return Get(ResponseCode.UniqueConstraintFailed);
        }

        /// <summary>
        /// 取得訊息
        /// </summary>
        /// <returns></returns>
        public static ResponseViewModel Get(ResponseCode responseCode)
        {
            switch (responseCode)
            {
                case ResponseCode.Success:
                    return new ResponseViewModel()
                    {
                        Code = 200,
                        Message = "Success"
                    };
                case ResponseCode.InternalServerError:
                    return new ResponseViewModel()
                    {
                        Code = 500,
                        Message = "Error"
                    };
                case ResponseCode.NoData:
                    return new ResponseViewModel()
                    {
                        Code = 404,
                        Message = "NoData"
                    };
                case ResponseCode.UniqueConstraintFailed:
                    return new ResponseViewModel()
                    {
                        Code = 19,
                        Message = "Unique constraint failed"
                    };
                default:
                    return new ResponseViewModel();
            }
        }
    }
}
