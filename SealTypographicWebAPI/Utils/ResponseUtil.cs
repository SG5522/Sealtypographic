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
        public static ResponseViewModel DBError()
        {
            return Get(ResponseCode.DBError);
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
        /// 上傳失敗
        /// </summary>
        /// <returns></returns>
        public static ResponseViewModel FileUploadFailed()
        {
            return Get(ResponseCode.FileUploadFailed);
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
                        Code = (int)ResponseCode.Success,
                        Message = "Success"
                    };
                case ResponseCode.DBError:
                    return new ResponseViewModel()
                    {
                        Code = (int)ResponseCode.DBError,
                        Message = "DataBase Error"
                    };
                case ResponseCode.NoData:
                    return new ResponseViewModel()
                    {
                        Code = (int)ResponseCode.NoData,
                        Message = "NoData"
                    };
                case ResponseCode.UniqueConstraintFailed:
                    return new ResponseViewModel()
                    {
                        Code = (int)ResponseCode.UniqueConstraintFailed,
                        Message = "Unique constraint failed"
                    };
                case ResponseCode.FileUploadFailed:
                    return new ResponseViewModel()
                    {
                        Code = (int)ResponseCode.FileUploadFailed,
                        Message = "File Upload failed"
                    };
                default:
                    return new ResponseViewModel();
            }
        }
    }
}
