namespace SealTypographicWebAPI.Consts
{
    /// <summary>
    /// API傳輸結果代碼(暫定)
    /// </summary>
    public enum ResponseCode
    {

        /// <summary>
        /// 回傳成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// DB無資料
        /// </summary>
        DbNoData = 1000,

        /// <summary>
        /// 資料庫處理錯誤
        /// </summary>
        DbError = 1001,

        /// <summary>
        /// 資料庫欄位限制唯一約束錯誤回傳
        /// </summary>
        UniqueConstraintFailed = 1019,


        /// <summary>
        /// 資料庫客戶資料建立失敗
        /// </summary>
        CustomerCreateFailed = 2000,

        /// <summary>
        /// 資料庫客戶編號重複
        /// </summary>
        CustomerNumberRepeat = 2001,

        /// <summary>
        /// 資料庫客戶無資料
        /// </summary>
        CustomeNoData = 2002,


        /// <summary>
        /// 資料庫客戶印鑑序號重複
        /// </summary>
        CustomerSealSequenceRepeat = 2101,

        /// <summary>
        /// 資料庫客戶印鑑無資料
        /// </summary>
        CustomerSealNoData = 2102,

        /// <summary>
        /// 資料庫客戶印鑑建立失敗
        /// </summary>
        CreateCustomerSealFailed = 2110,

        /// <summary>
        /// 資料庫客戶印鑑建立時序號重複
        /// </summary>
        CreateCustomerSealSequenceRepeat = 2111,

        /// <summary>
        /// 更新客戶印鑑失敗
        /// </summary>
        UpdateCustomerSealFailed = 2120,

        /// <summary>
        /// 更新客戶印鑑時序號重複
        /// </summary>
        UpdateCustomerSealSequenceRepeat = 2121,

        /// <summary>
        /// 更新客戶印鑑時找不到檔案
        /// </summary>
        UpdateCustomerSealNoData = 2122,

        /// <summary>
        /// 刪除(Hide)客戶印鑑失敗
        /// </summary>
        DeleteCustomerSealFailed = 2130,

        /// <summary>
        /// 刪除(Hide)客戶印鑑時找不到檔案
        /// </summary>
        DeleteCustomerSealNoData = 2131,        

        /// <summary>
        /// 資料庫會計師資料建立失敗
        /// </summary>
        AccountantCreateFailed = 3000,

        /// <summary>
        /// 資料庫會計師編號重複
        /// </summary>
        AccountantNumberRepeat = 3001,

        /// <summary>
        /// 資料庫會計師無資料
        /// </summary>
        AccountantNoData = 3002,

        /// <summary>
        /// 資料庫會計師簽印建立失敗
        /// </summary>
        AccountantSignCreateFailed = 3100,

        /// <summary>
        /// 資料庫會計師簽印已有資料
        /// </summary>
        AccountantSignRepeat = 3101,

        /// <summary>
        /// 資料庫會計師簽印無資料
        /// </summary>
        AccountantSignNoData = 3102,

        /// <summary>
        /// 資料庫會計師簽印中已有草稿或是待審的簽印
        /// </summary>
        AccountantSignHaveDraftOrPendingReviewStatus = 3103,

        /// <summary>
        /// 資料庫會計師簽印建立失敗
        /// </summary>
        CreateAccountantSignFailed = 3110,

        /// <summary>
        /// 資料庫會計簽印建立時發現重複(依類別確認)
        /// </summary>
        CreateAccountantSignRepeat = 3111,

        /// <summary>
        /// 更新會計師簽印失敗
        /// </summary>
        UpdateAccountantSignFailed = 3120,

        /// <summary>
        /// 更新會計師簽印時發現重複(更改類別時)
        /// </summary>
        UpdateAccountantSignRepeat = 3121,

        /// <summary>
        /// 更新會計師簽印時找不到檔案
        /// </summary>
        UpdateAccountantSignNoData = 3122,

        /// <summary>
        /// 刪除(Hide)會計師簽印失敗
        /// </summary>
        DeleteAccountantSignFailed = 3130,

        /// <summary>
        /// 刪除(Hide)會計師簽印時找不到資料
        /// </summary>
        DeleteAccountantSignNoData = 3131,

        /// <summary>
        /// 資料庫信頭資料建立失敗
        /// </summary>
        CreateLetterheadFailed = 4010,

        /// <summary>
        /// 資料庫信頭編號重複
        /// </summary>
        CreateLetterheadNumberRepeat = 4011,

        /// <summary>
        /// 資料庫信頭無資料
        /// </summary>
        CreateLetterheadNoData = 4012,

        /// <summary>
        /// 資料庫信頭圖像建立失敗
        /// </summary>
        LetterheadImageCreateFailed = 4100,

        /// <summary>
        /// 資料庫信頭圖像序號重複
        /// </summary>
        LetterheadImageSequenceRepeat = 4101,

        /// <summary>
        /// 資料庫信頭圖像無資料
        /// </summary>
        LetterheadImageNoData = 4102,

        /// <summary>
        /// 上傳失敗
        /// </summary>
        FileUploadFailed = 5001,
    }
}
