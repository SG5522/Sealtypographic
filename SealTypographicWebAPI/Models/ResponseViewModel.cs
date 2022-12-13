using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Util;

namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 錯誤訊息使用的Class
    /// </summary>
    public class ResponseViewModel
    {
        /// <summary>
        /// 狀態號碼
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 回傳訊息
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// 成功
        /// </summary>
        public void Success()
        {            
            Code = (int)ResponseCode.Success;
            Message = "Success";
        }

        /// <summary>
        /// DbError
        /// </summary>
        public void DbError()
        {
            Code = (int)ResponseCode.DbError;
            Message = "DataBase Error";
        }

        /// <summary>
        /// DbNoData
        /// </summary>
        public void DbNoData()
        {
            Code = (int)ResponseCode.DbNoData;
            Message = "Database DbNoData";
        }

        /// <summary>
        /// CustomerSeal Sequence Repeat
        /// </summary>
        public void CustomerNumberRepeat()
        {
            Code = (int)ResponseCode.CustomerNumberRepeat;
            Message = "Customer Number Repeat";
        }

        /// <summary>
        /// CustomerSeal Sequence Repeat
        /// </summary>
        public void CustomerSealSequenceRepeat()
        {
            Code = (int)ResponseCode.CustomerSealSequenceRepeat;
            Message = "CustomerSeal Sequence Repeat";
        }

        /// <summary>
        /// Accountant Number Repeat
        /// </summary>
        public void AccountantNumberRepeat()
        {
            Code = (int)ResponseCode.AccountantNumberRepeat;
            Message = "Accountant Number Repeat";
        }

        /// <summary>
        /// AccountSign have data
        /// </summary>
        public void AccountantSignRepeat()
        {
            Code = (int)ResponseCode.AccountantSignRepeat;
            Message = "AccountantSign have data";
        }

        /// <summary>
        /// Unique constraint failed
        /// </summary>
        public void UniqueConstraintFailed()
        {
            Code = (int)ResponseCode.UniqueConstraintFailed;
            Message = "Unique constraint failed";
        }

        /// <summary>
        /// File Upload failed
        /// </summary>
        public void FileUploadFailed()
        {
            Code = (int)ResponseCode.FileUploadFailed;
            Message = "File Upload failed";
        }
    }
}
