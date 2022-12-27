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
        /// 回傳錯誤項目
        /// </summary>
        public string? ErrorItem { get; set; }

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
        /// 客戶無資料
        /// </summary>
        public void CustomeNoData()
        {
            Code = (int)ResponseCode.CustomeNoData;
            Message = "Customer no data";
        }

        /// <summary>
        /// 客戶資料建立失敗
        /// </summary>
        public void CreateCustomerFailed()
        {
            Code = (int)ResponseCode.CreateCustomerFailed;
            Message = "Create Customer failed";
        }

        /// <summary>
        /// 資料庫客戶編號重複
        /// </summary>
        public void CreateCustomerNumberRepeat()
        {
            Code = (int)ResponseCode.CreateCustomerNumberRepeat;
            Message = "Create Customer number repeat";
        }

        /// <summary>
        /// 更新客戶資料失敗
        /// </summary>
        public void UpdateCustomerFailed()
        {
            Code = (int)ResponseCode.UpdateCustomerFailed;
            Message = "Update customer failed";
        }

        /// <summary>
        /// 更新客戶資料找不到檔案
        /// </summary>
        public void UpdateCustomerNoData()
        {
            Code = (int)ResponseCode.UpdateCustomerNoData;
            Message = "Update customer no data";
        }

        /// <summary>
        /// 刪除(Hide)客戶失敗
        /// </summary>
        public void DeleteCustomerFailed()
        {
            Code = (int)ResponseCode.DeleteCustomerFailed;
            Message = "Delete customer failed";
        }

        /// <summary>
        /// 刪除(Hide)客戶時找不到資料
        /// </summary>
        public void DeleteCustomerNoData()
        {
            Code = (int)ResponseCode.DeleteCustomerNoData;
            Message = "Delete customer noData";
        }

        /// <summary>
        /// 客戶印鑑序號重複(類別內重複序號)
        /// </summary>
        public void CustomerSealSequenceRepeat()
        {
            Code = (int)ResponseCode.CustomerSealSequenceRepeat;
            Message = "Customer seal Sequence Repeat";
        }

        /// <summary>
        /// 客戶印鑑無資料
        /// </summary>
        public void CustomerSealNoData()
        {
            Code = (int)ResponseCode.CustomerSealNoData;
            Message = "Customer seal no data";
        }

        /// <summary>
        /// 資料庫客戶印鑑建立失敗
        /// </summary>
        public void CreateCustomerSealFailed()
        {
            Code = (int)ResponseCode.CreateCustomerSealFailed;
            Message = "Customer seal Create Failed";
        }

        /// <summary>
        /// 客戶印鑑新增時序號重複
        /// </summary>
        public void CreateCustomerSealSequenceRepeat()
        {
            Code = (int)ResponseCode.CreateCustomerSealSequenceRepeat;
            Message = "Create customer seal sequence repeat";
        }

        /// <summary>
        /// 客戶印鑑刪除失敗
        /// </summary>
        public void UpdateCustomerSealFailed()
        {
            Code = (int)ResponseCode.UpdateCustomerSealFailed;
            Message = "Update customer seal failed";
        }

        /// <summary>
        /// 客戶印鑑更新時序號重複
        /// </summary>
        public void UpdateCustomerSealSequenceRepeat()
        {
            Code = (int)ResponseCode.UpdateCustomerSealSequenceRepeat;
            Message = "Update customer seal sequence repeat";
        }

        /// <summary>
        /// 客戶印鑑刪除時無資料
        /// </summary>
        public void UpdateCustomerSealNoData()
        {
            Code = (int)ResponseCode.UpdateCustomerSealNoData;
            Message = "Update customer seal no data";
        }

        /// <summary>
        /// 客戶印鑑刪除失敗
        /// </summary>
        public void DeleteCustomerSealFailed()
        {
            Code = (int)ResponseCode.DeleteCustomerSealFailed;
            Message = "Delete customer seal failed";
        }

        /// <summary>
        /// 客戶印鑑刪除時無資料
        /// </summary>
        public void DeleteCustomerSealNoData()
        {
            Code = (int)ResponseCode.DeleteCustomerSealNoData;
            Message = "Customer seal delete no data";
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
        /// 會計師編號重複
        /// </summary>
        public void AccountantNumberRepeat()
        {
            Code = (int)ResponseCode.AccountantNumberRepeat;
            Message = "Accountant number repeat";
        }

        /// <summary>
        /// 會計師建立失敗
        /// </summary>
        public void CreateAccountantFailed()
        {
            Code = (int)ResponseCode.CreateAccountantFailed;
            Message = "Create accountant failed";
        }

        /// <summary>
        /// 會計師編號重複
        /// </summary>
        public void CreateAccountantNumberRepeat()
        {
            Code = (int)ResponseCode.CreateAccountantNumberRepeat;
            Message = "Create accountant number repeat";
        }

        /// <summary>
        /// 資料庫會計師資料更新失敗
        /// </summary>
        public void UpdateAccountantFailed()
        {
            Code = (int)ResponseCode.UpdateAccountantFailed;
            Message = "Update accountant no data";
        }

        /// <summary>
        /// 會計師資料更新找不到資料
        /// </summary>
        public void UpdateAccountantNoData()
        {
            Code = (int)ResponseCode.UpdateAccountantNoData;
            Message = "Update accountant no data";
        }

        /// <summary>
        /// 資料庫會計師資料刪除找不到資料
        /// </summary>
        public void DeleteAccountantFailed()
        {
            Code = (int)ResponseCode.DeleteAccountantFailed;
            Message = "Delete accountant failed";
        }

        /// <summary>
        /// 資料庫會計師資料刪除找不到資料
        /// </summary>
        public void DeleteAccountantNoData()
        {
            Code = (int)ResponseCode.DeleteAccountantNoData;
            Message = "Delete accountant no data";
        }

        /// <summary>
        /// 會計師簽印建立失敗
        /// </summary>
        public void AccountantSignCreateFailed()
        {
            Code = (int)ResponseCode.CreateAccountantSignFailed;
            Message = "Accountant sign create failed";
        }

        /// <summary>
        /// 會計師簽印重複(該類別已有資料)
        /// </summary>
        public void AccountantSignRepeat()
        {
            Code = (int)ResponseCode.AccountantSignRepeat;
            Message = "Accountant sign repeat";
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
        /// 會計師簽印重複(該類別已有資料)
        /// </summary>
        public void AccountantSignHaveDraftOrPendingReviewStatus()
        {
            Code = (int)ResponseCode.AccountantSignHaveDraftOrPendingReviewStatus;
            Message = "Accountant sign 'GroupCreateDate' have draft or pending reviewStatus";
        }

        /// <summary>
        /// 新增會計師簽印失敗
        /// </summary>
        public void CreateAccountantSignFailed()
        {
            Code = (int)ResponseCode.CreateAccountantSignFailed;
            Message = "Create accountant sign failed";
        }

        /// <summary>
        /// 資料庫會計簽印建立時發現重複(依類別確認)
        /// </summary>
        public void CreateAccountantSignRepeat()
        {
            Code = (int)ResponseCode.CreateAccountantSignRepeat;
            Message = "Create accountant sign repeat";
        }

        /// <summary>
        /// 更新會計師簽印失敗
        /// </summary>
        public void UpdateAccountantSignFailed()
        {
            Code = (int)ResponseCode.UpdateAccountantSignFailed;
            Message = "Updatea ccountant sign failed";
        }

        /// <summary>
        /// 更新會計師簽印時發現重複(更改類別時)
        /// </summary>
        public void UpdateAccountantSignRepeat()
        {
            Code = (int)ResponseCode.UpdateAccountantSignRepeat;
            Message = "Update accountant sign repeat";
        }

        /// <summary>
        /// 更新會計師簽印時找不到檔案
        /// </summary>
        public void UpdateAccountantSignNoData()
        {
            Code = (int)ResponseCode.UpdateAccountantSignNoData;
            Message = "Update accountant sign no data";
        }

        /// <summary>
        /// 刪除(Hide)會計師簽印失敗
        /// </summary>
        public void DeleteAccountantSignFailed()
        {
            Code = (int)ResponseCode.DeleteAccountantSignFailed;
            Message = "Delete accountant sign failed";
        }


        /// <summary>
        /// 刪除(Hide)會計師簽印時找不到資料
        /// </summary>
        public void DeleteAccountantSignNoData()
        {
            Code = (int)ResponseCode.DeleteAccountantSignNoData;
            Message = "Delete accountant sign no data";
        }

        /// <summary>
        /// 信頭建立失敗
        /// </summary>
        public void CreateLetterheadFailed()
        {
            Code = (int)ResponseCode.CreateLetterheadFailed;
            Message = "Create letterhead failed";
        }

        /// <summary>
        /// 信頭編號重複
        /// </summary>
        public void CreateLetterheadNumberRepeat()
        {
            Code = (int)ResponseCode.CreateLetterheadNumberRepeat;
            Message = "Create letterhead number repeat";
        }

        /// <summary>
        /// 信頭無資料
        /// </summary>
        public void CreateLetterheadNoData()
        {
            Code = (int)ResponseCode.CreateLetterheadNoData;
            Message = "Create letterhead no data";
        }

        /// <summary>
        /// 信頭圖像建立失敗
        /// </summary>
        public void CreateLetterheadImageFailed()
        {
            Code = (int)ResponseCode.CreateLetterheadImageFailed;
            Message = "AccountantSign create failed";
        }

        /// <summary>
        /// 信頭圖像序號重複
        /// </summary>
        public void LetterheadImageSequenceRepeat()
        {
            Code = (int)ResponseCode.LetterheadImageSequenceRepeat;
            Message = "LetterheadImage sequence repeat";
        }

        /// <summary>
        /// 會計師簽印無資料
        /// </summary>
        public void LetterheadImageNoData()
        {
            Code = (int)ResponseCode.LetterheadImageNoData;
            Message = "Letterhead image no Data";
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
