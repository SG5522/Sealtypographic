using SealTypographicWebAPI.Consts;

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
            Message = "Other no data";
        }

        /// <summary>
        /// 客戶資料建立失敗
        /// </summary>
        public void CreateCustomerFailed()
        {
            Code = (int)ResponseCode.CreateCustomerFailed;
            Message = "New Other failed";
        }

        /// <summary>
        /// 資料庫客戶編號重複
        /// </summary>
        public void CreateCustomerNumberRepeat()
        {
            Code = (int)ResponseCode.CreateCustomerNumberRepeat;
            Message = "New Other number repeat";
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
            Message = "Other seal Sequence Repeat";
        }

        /// <summary>
        /// 客戶印鑑無資料
        /// </summary>
        public void CustomerSealNoData()
        {
            Code = (int)ResponseCode.CustomerSealNoData;
            Message = "Other seal no data";
        }

        /// <summary>
        /// 資料庫客戶印鑑建立失敗
        /// </summary>
        public void CreateCustomerSealFailed()
        {
            Code = (int)ResponseCode.CreateCustomerSealFailed;
            Message = "Other seal New Failed";
        }

        /// <summary>
        /// 客戶印鑑新增時序號重複
        /// </summary>
        public void CreateCustomerSealSequenceRepeat()
        {
            Code = (int)ResponseCode.CreateCustomerSealSequenceRepeat;
            Message = "New customer seal sequence repeat";
        }
        //CreateCustomerSealQuarterRepeat

        /// <summary>
        /// 客戶印鑑新增時季度重複
        /// </summary>
        public void CreateCustomerSealQuarterRepeat()
        {
            Code = (int)ResponseCode.CreateCustomerSealQuarterRepeat;
            Message = "New customer seal quarter repeat";
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
            Message = "Other seal delete no data";
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
            Message = "New accountant failed";
        }

        /// <summary>
        /// 會計師編號重複
        /// </summary>
        public void CreateAccountantNumberRepeat()
        {
            Code = (int)ResponseCode.CreateAccountantNumberRepeat;
            Message = "New accountant number repeat";
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
        /// 會計師簽印無資料
        /// </summary>
        public void AccountantSignNoData()
        {
            Code = (int)ResponseCode.AccountantSignNoData;
            Message = "AccountantSign no data";
        }

        /// <summary>
        /// 會計師簽印已有一筆草稿或是待審狀態
        /// </summary>
        public void AccountantSignHaveDraftOrPendingReviewStatus()
        {
            Code = (int)ResponseCode.AccountantSignHaveDraftOrPendingReviewStatus;
            Message = "Accountant sign 'GroupCreateDate' have draft or pending reviewStatus";
        }

        /// <summary>
        /// 會計師簽印建立失敗
        /// </summary>
        public void CreateAccountantSignFailed()
        {
            Code = (int)ResponseCode.CreateAccountantSignFailed;
            Message = "New accountant sign failed";
        }

        /// <summary>
        /// 資料庫會計簽印建立時發現重複(依類別確認)
        /// </summary>
        public void CreateAccountantSignRepeat()
        {
            Code = (int)ResponseCode.CreateAccountantSignRepeat;
            Message = "New accountant sign repeat";
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
        /// 會計師群組找不到資料
        /// </summary>
        public void AccountantGroupNoData()
        {
            Code = (int)ResponseCode.AccountantGroupNoData;
            Message = "Accountant group no data";
        }

        /// <summary>
        /// 會計師群組建立失敗
        /// </summary>
        public void CreateAccountantGroupFailed()
        {
            Code = (int)ResponseCode.CreateAccountantGroupFailed;
            Message = "New accountant group failed";
        }

        /// <summary>
        /// 會計師群組建立時編號重複
        /// </summary>
        public void CreateAccountantGroupNumberRepeat()
        {
            Code = (int)ResponseCode.CreateAccountantGroupNumberRepeat;
            Message = "New accountant group number repeat";
        }

        /// <summary>
        /// 更新會計師群組失敗
        /// </summary>
        public void UpdateAccountantGroupFailed()
        {
            Code = (int)ResponseCode.UpdateAccountantGroupFailed;
            Message = "Update accountant group failed";
        }

        /// <summary>
        /// 更新會計師群組找不到資料
        /// </summary>
        public void UpdateAccountantGroupNoData()
        {
            Code = (int)ResponseCode.UpdateAccountantGroupNoData;
            Message = "Update accountant group no data";
        }

        /// <summary>
        /// 刪除(Hide)會計師群組失敗
        /// </summary>
        public void DeleteAccountantGroupFailed()
        {
            Code = (int)ResponseCode.DeleteAccountantGroupFailed;
            Message = "Delete accountant group failed";
        }

        /// <summary>
        /// 刪除(Hide)會計師簽印時找不到資料
        /// </summary>
        public void DeleteAccountantGroupNoData()
        {
            Code = (int)ResponseCode.DeleteAccountantGroupNoData;
            Message = "Delete accountant group no data";
        }

        /// <summary>
        /// 加入會計群組失敗
        /// </summary>
        public void JoinAccountantGroupFailed()
        {
            Code = (int)ResponseCode.JoinAccountantGroupFailed;
            Message = "Join accountant group failed";
        }

        /// <summary>
        /// 資料庫信頭編號重複
        /// </summary>
        public void LetterheadNoData()
        {
            Code = (int)ResponseCode.LetterheadNoData;
            Message = "Letterhead no data";
        }

        /// <summary>
        /// 信頭資料建立失敗
        /// </summary>
        public void CreateLetterheadFailed()
        {
            Code = (int)ResponseCode.CreateLetterheadFailed;
            Message = "New letterhead failed";
        }

        /// <summary>
        /// 資料庫信頭編號重複
        /// </summary>
        public void CreateLetterheadNumberRepeat()
        {
            Code = (int)ResponseCode.CreateLetterheadNumberRepeat;
            Message = "New letterhead number repeat";
        }

        /// <summary>
        /// 資料庫信頭無資料
        /// </summary>
        public void CreateLetterheadNoData()
        {
            Code = (int)ResponseCode.CreateLetterheadNoData;
            Message = "New letterhead no data";
        } 

        /// <summary>
        /// 更新信頭資料失敗
        /// </summary>
        public void UpdateLetterheadFailed()
        {
            Code = (int)ResponseCode.UpdateLetterheadFailed;
            Message = "Update letterhead failed";
        }

        /// <summary>
        /// 更新信頭找不到資料
        /// </summary>
        public void UpdateLetterheadNoData()
        {
            Code = (int)ResponseCode.UpdateLetterheadNoData;
            Message = "Update letterhead no data";
        }

        /// <summary>
        /// 刪除(Hide)信頭失敗
        /// </summary>
        public void DeleteLetterheadFailed()
        {
            Code = (int)ResponseCode.DeleteLetterheadFailed;
            Message = "Delete letterhead failed";
        }

        /// <summary>
        /// 刪除(Hide)信頭時找不到資料
        /// </summary>
        public void DeleteLetterheadNoData()
        {
            Code = (int)ResponseCode.DeleteLetterheadNoData;
            Message = "Delete letterhead no data";
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
        /// 信頭圖像已有一筆草稿
        /// </summary>
        public void LetterheadImageHaveDraftReviewStatus()
        {
            Code = (int)ResponseCode.LetterheadImageHaveDraftReviewStatus;
            Message = "Letterhead image have draft review status";
        }

        /// <summary>
        /// 信頭圖像建立失敗
        /// </summary>
        public void CreateLetterheadImageFailed()
        {
            Code = (int)ResponseCode.CreateLetterheadImageFailed;
            Message = "New letterhead image failed";
        }

        /// <summary>
        /// 資料庫信頭圖像序號重複
        /// </summary>
        public void CreateLetterheadImageSequenceRepeat()
        {
            Code = (int)ResponseCode.CreateLetterheadImageSequenceRepeat;
            Message = "New letterhead image sequence repeat";
        }

        /// <summary>
        /// 資料庫信頭圖像更新失敗
        /// </summary>
        public void UpdateLetterheadImageFailed()
        {
            Code = (int)ResponseCode.UpdateLetterheadImageFailed;
            Message = "Update letterhead image failed";
        }

        /// <summary>
        /// 資料庫信頭圖像更新時序號重複
        /// </summary>
        public void UpdateLetterheadImageSequenceRepeat()
        {
            Code = (int)ResponseCode.UpdateLetterheadImageSequenceRepeat;
            Message = "Update letterhead image sequence repeat";
        }

        /// <summary>
        /// 資料庫信頭圖像更新時找不到資料
        /// </summary>
        public void UpdateLetterheadImageNoData()
        {
            Code = (int)ResponseCode.UpdateLetterheadImageNoData;
            Message = "Update letterhead image no data";
        }

        /// <summary>
        /// 資料庫信頭圖像更新時找不到資料
        /// </summary>
        public void DeleteLetterheadImageFailed()
        {
            Code = (int)ResponseCode.DeleteLetterheadImageFailed;
            Message = "Delete letterhead image failed";
        }

        /// <summary>
        /// 資料庫信頭圖像刪除失敗
        /// </summary>
        public void DeleteLetterheadImageNoData()
        {
            Code = (int)ResponseCode.DeleteLetterheadImageNoData;
            Message = "Delete letterhead image no data";
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
