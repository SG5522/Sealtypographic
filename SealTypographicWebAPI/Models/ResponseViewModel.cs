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
        /// 客戶資料建立失敗
        /// </summary>
        public void CustomerCreateFailed()
        {
            Code = (int)ResponseCode.CustomerCreateFailed;
            Message = "Customer create failed";
        }

        /// <summary>
        /// 客戶編號重複
        /// </summary>
        public void CustomerNumberRepeat()
        {
            Code = (int)ResponseCode.CustomerNumberRepeat;
            Message = "Customer number repeat";
        }

        /// <summary>
        /// 客戶無資料
        /// </summary>
        public void CustomeNoData()
        {
            Code = (int)ResponseCode.CustomeNoData;
            Message = "Customer no data";
        }

        /// <summary>
        /// CustomerSeal Sequence Repeat
        /// </summary>
        public void CustomerSealCreateFailed()
        {
            Code = (int)ResponseCode.CustomerSealCreateFailed;
            Message = "CustomerSeal Create Failed";
        }

        /// <summary>
        /// 客戶印鑑序號重複(類別內重複序號)
        /// </summary>
        public void CustomerSealSequenceRepeat()
        {
            Code = (int)ResponseCode.CustomerSealSequenceRepeat;
            Message = "CustomerSeal Sequence Repeat";
        }

        /// <summary>
        /// 客戶印鑑無資料
        /// </summary>
        public void CustomerSealNoData()
        {
            Code = (int)ResponseCode.CustomerSealNoData;
            Message = "CustomerSeal no data";
        }

        /// <summary>
        /// 會計師建立錯誤
        /// </summary>
        public void AccountantCreateFailed()
        {
            Code = (int)ResponseCode.AccountantCreateFailed;
            Message = "Accountant create failed";
        }

        /// <summary>
        /// 會計師編號重複
        /// </summary>
        public void AccountantNumberRepeat()
        {
            Code = (int)ResponseCode.AccountantNumberRepeat;
            Message = "Accountant number repeat";
        }

        /// <summary>
        /// 會計師無資料
        /// </summary>
        public void AccountantNoData()
        {
            Code = (int)ResponseCode.AccountantNoData;
            Message = "Accountant no data";
        }

        /// <summary>
        /// 會計師簽印建立失敗
        /// </summary>
        public void AccountantSignCreateFailed()
        {
            Code = (int)ResponseCode.AccountantSignCreateFailed;
            Message = "AccountantSign create failed";
        }

        /// <summary>
        /// 會計師簽印重複(該類別已有資料)
        /// </summary>
        public void AccountantSignRepeat()
        {
            Code = (int)ResponseCode.AccountantSignRepeat;
            Message = "AccountantSign have data";
        }

        /// <summary>
        /// 會計師簽印無資料
        /// </summary>
        public void AccountantSignNoData()
        {
            Code = (int)ResponseCode.AccountantSignNoData;
            Message = "AccountantSign no data";
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
