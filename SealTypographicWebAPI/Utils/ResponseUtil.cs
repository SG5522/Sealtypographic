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
            return Get(ResponseCode.DbError);
        }
        /// <summary>
        /// 回傳無資料
        /// </summary>
        /// <returns></returns>
        public static ResponseViewModel DbNoData()
        {
            return Get(ResponseCode.DbNoData);
        }

        /// <summary>
        /// 印鑑排序重複
        /// </summary>
        /// <returns></returns>
        public static ResponseViewModel SealSequenceError()
        {
            return Get(ResponseCode.CustomerSealSequenceRepeat);
        }

        /// <summary>
        /// 會計師簽印已有資料
        /// </summary>
        /// <returns></returns>
        public static ResponseViewModel AccountSignHaveData()
        {
            return Get(ResponseCode.AccountSignHaveData);
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
                case ResponseCode.DbError:
                    return new ResponseViewModel()
                    {
                        Code = (int)ResponseCode.DbError,
                        Message = "DataBase Error"
                    };
                case ResponseCode.DbNoData:
                    return new ResponseViewModel()
                    {
                        Code = (int)ResponseCode.DbNoData,
                        Message = "Database DbNoData"
                    };
                case ResponseCode.CustomerSealSequenceRepeat:
                    return new ResponseViewModel()
                    {
                        Code = (int)ResponseCode.CustomerSealSequenceRepeat,
                        Message = "CustomerSeal Sequence Repeat"
                    };
                case ResponseCode.AccountSignHaveData:
                    return new ResponseViewModel()
                    {
                        Code = (int)ResponseCode.AccountSignHaveData,
                        Message = "AccountSign have data"
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
