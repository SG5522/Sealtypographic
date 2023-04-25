using DBEntities.Consts;

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
        /// 更新客戶資料找不到檔案
        /// </summary>
        public void UpdateCustomerNoData()
        {
            Code = (int)ResponseCode.UpdateCustomerNoData;
            Message = "Update customer no data";
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
        /// 客戶印鑑新增時序號重複
        /// </summary>
        public void CreateCustomerSealSequenceRepeat()
        {
            Code = (int)ResponseCode.CreateCustomerSealSequenceRepeat;
            Message = "Create customer seal sequence repeat";
        }
        //CreateCustomerSealQuarterRepeat

        /// <summary>
        /// 客戶印鑑新增時季度重複
        /// </summary>
        public void CreateCustomerSealQuarterRepeat()
        {
            Code = (int)ResponseCode.CreateCustomerSealQuarterRepeat;
            Message = "Create customer seal quarter repeat";
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
        /// 資料庫會計簽印建立時發現重複(依類別確認)
        /// </summary>
        public void CreateAccountantSignRepeat()
        {
            Code = (int)ResponseCode.CreateAccountantSignRepeat;
            Message = "New accountant sign repeat";
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
            Message = "AccountantSignAuthorization group no data";
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
        /// 更新會計師群組找不到資料
        /// </summary>
        public void UpdateAccountantGroupNoData()
        {
            Code = (int)ResponseCode.UpdateAccountantGroupNoData;
            Message = "Update accountant group no data";
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
        /// 資料庫信頭無資料
        /// </summary>
        public void CreateLetterheadNoData()
        {
            Code = (int)ResponseCode.CreateLetterheadNoData;
            Message = "New letterhead no data";
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
        /// 刪除(Hide)信頭時找不到資料
        /// </summary>
        public void DeleteLetterheadNoData()
        {
            Code = (int)ResponseCode.DeleteLetterheadNoData;
            Message = "Delete letterhead no data";
        }

        /// <summary>
        /// 信頭圖像無資料
        /// </summary>
        public void LetterheadImageNoData()
        {
            Code = (int)ResponseCode.LetterheadImageNoData;
            Message = "LetterheadImage image no Data";
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
        /// 刪除信頭圖像找不到資料
        /// </summary>
        public void DeleteLetterheadImageNoData()
        {
            Code = (int)ResponseCode.DeleteLetterheadImageNoData;
            Message = "Delete letterhead image no data";
        }

        /// <summary>
        /// 上傳失敗
        /// </summary>
        public void FileUploadFailed()
        {
            Code = (int)ResponseCode.FileUploadFailed;
            Message = "File Upload failed";
        }

        /// <summary>
        /// 找不到上傳資料
        /// </summary>
        public void FileUploadNoData()
        {
            Code = (int)ResponseCode.FileUploadNoData;
            Message = "File upload no data";
        }

        /// <summary>
        /// 查無臨時章
        /// </summary>
        public void TemporarySealNoData()
        {
            Code = (int)ResponseCode.TemporarySealNoData;
            Message = "Temporary seal no data";
        }

        /// <summary>
        /// 更新臨時章時找不到資料
        /// </summary>
        public void UpdateTemporarySealNoData()
        {
            Code = (int)ResponseCode.UpdateTemporarySealNoData;
            Message = "Update temporary seal no data";
        }

        /// <summary>
        /// 刪除臨時章找不到資料
        /// </summary>
        public void DeleteTemporarySealNoData()
        {
            Code = (int)ResponseCode.DeleteTemporarySealNoData;
            Message = "Delete temporary seal no data";
        }

        /// <summary>
        /// 刪除客戶印鑑樣板時找不到資料
        /// </summary>
        public void DeleteCustomerSealTemplateNoData()
        {
            Code = (int)ResponseCode.DeleteCustomerSealTemplateNoData;
            Message = "Delete customerSeal template no data";
        }

        /// <summary>
        /// 刪除客戶印鑑樣板時找不到資料
        /// </summary>
        public void DeleteAccountantSignTemplateNoData()
        {
            Code = (int)ResponseCode.DeleteAccountantSignTemplateNoData;
            Message = "Delete accountantSign template no Data";
        }

        /// <summary>
        /// 刪除客戶印鑑樣板時找不到資料
        /// </summary>
        public void DeleteLetterImageTemplateNoData()
        {
            Code = (int)ResponseCode.DeleteLetterImageTemplateNoData;
            Message = "Delete letterImage template no data";
        }

        /// <summary>
        /// 回傳失敗
        /// </summary>
        public void Error()
        {
            Code = (int)ResponseCode.DbError;
            Message = "Error";
        }
    }
}
