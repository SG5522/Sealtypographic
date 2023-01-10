namespace SealTypographicWebAPI.Consts
{
    /// <summary>
    /// API傳輸結果代碼
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
        /// 資料庫客戶無資料
        /// </summary>
        CustomeNoData = 2001,

        /// <summary>
        /// 資料庫客戶資料建立失敗
        /// </summary>
        CreateCustomerFailed = 2010,

        /// <summary>
        /// 資料庫客戶編號重複
        /// </summary>
        CreateCustomerNumberRepeat = 2011,

        /// <summary>
        /// 更新客戶資料失敗
        /// </summary>
        UpdateCustomerFailed = 2020,

        /// <summary>
        /// 更新客戶資料找不到資料
        /// </summary>
        UpdateCustomerNoData = 2021,

        /// <summary>
        /// 刪除(Hide)客戶失敗
        /// </summary>
        DeleteCustomerFailed = 2030,

        /// <summary>
        /// 刪除(Hide)客戶時找不到資料
        /// </summary>
        DeleteCustomerNoData = 2031,

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
        /// 資料庫客戶印鑑建立時季度重複
        /// </summary>
        CreateCustomerSealQuarterRepeat = 2112,

        /// <summary>
        /// 更新客戶印鑑失敗
        /// </summary>
        UpdateCustomerSealFailed = 2120,

        /// <summary>
        /// 更新客戶印鑑時序號重複
        /// </summary>
        UpdateCustomerSealSequenceRepeat = 2121,

        /// <summary>
        /// 更新客戶印鑑時找不到資料
        /// </summary>
        UpdateCustomerSealNoData = 2122,

        /// <summary>
        /// 刪除(Hide)客戶印鑑失敗
        /// </summary>
        DeleteCustomerSealFailed = 2130,

        /// <summary>
        /// 刪除(Hide)客戶印鑑時找不到資料
        /// </summary>
        DeleteCustomerSealNoData = 2131,

        /// <summary>
        /// 資料庫會計師無資料
        /// </summary>
        AccountantNoData = 3001,

        /// <summary>
        /// 資料庫會計師編號重複
        /// </summary>
        AccountantNumberRepeat = 3002,

        /// <summary>
        /// 會計師資料建立失敗
        /// </summary>
        CreateAccountantFailed = 3010,

        /// <summary>
        /// 會計師編號重複
        /// </summary>
        CreateAccountantNumberRepeat = 3011,

        /// <summary>
        /// 會計師資料更新失敗
        /// </summary>
        UpdateAccountantFailed = 3020,

        /// <summary>
        /// 會計師資料更新找不到資料
        /// </summary>
        UpdateAccountantNoData = 3021,

        /// <summary>
        /// 會計師資料刪除失敗
        /// </summary>
        DeleteAccountantFailed = 3030,

        /// <summary>
        /// 會計師資料刪除找不到資料
        /// </summary>
        DeleteAccountantNoData = 3031,

        /// <summary>
        /// 會計師簽印無資料
        /// </summary>
        AccountantSignNoData = 3101,

        /// <summary>
        /// 會計師簽印中已有草稿或是待審的簽印
        /// </summary>
        AccountantSignHaveDraftOrPendingReviewStatus = 3102,

        /// <summary>
        /// 會計師簽印建立失敗
        /// </summary>
        CreateAccountantSignFailed = 3110,

        /// <summary>
        /// 會計簽印建立時發現重複(依類別確認)
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
        /// 更新會計師簽印時找不到資料
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
        /// 會計師群組找不到資料
        /// </summary>
        AccountantGroupNoData = 3200,

        /// <summary>
        /// 會計師群組建立失敗
        /// </summary>
        CreateAccountantGroupFailed = 3210,

        /// <summary>
        /// 會計師群組建立時編號重複
        /// </summary>
        CreateAccountantGroupNumberRepeat = 3211,

        /// <summary>
        /// 更新會計師群組失敗
        /// </summary>
        UpdateAccountantGroupFailed = 3220,

        /// <summary>
        /// 更新會計師群組找不到資料
        /// </summary>
        UpdateAccountantGroupNoData = 3221,

        /// <summary>
        /// 刪除(Hide)會計師群組失敗
        /// </summary>
        DeleteAccountantGroupFailed = 3230,

        /// <summary>
        /// 刪除(Hide)會計師群組時找不到資料
        /// </summary>
        DeleteAccountantGroupNoData = 3231,

        /// <summary>
        /// 加入會計群組失敗
        /// </summary>
        JoinAccountantGroupFailed = 3300,
        
        /// <summary>
        /// 信頭無資料
        /// </summary>
        LetterheadNoData = 4001,

        /// <summary>
        /// 信頭資料建立失敗
        /// </summary>
        CreateLetterheadFailed = 4010,

        /// <summary>
        /// 信頭編號重複
        /// </summary>
        CreateLetterheadNumberRepeat = 4011,

        /// <summary>
        /// 信頭無資料
        /// </summary>
        CreateLetterheadNoData = 4012,

        /// <summary>
        /// 更新信頭資料失敗
        /// </summary>
        UpdateLetterheadFailed = 4020,

        /// <summary>
        /// 更新信頭找不到資料
        /// </summary>
        UpdateLetterheadNoData = 4021,

        /// <summary>
        /// 刪除(Hide)信頭失敗
        /// </summary>
        DeleteLetterheadFailed = 4030,

        /// <summary>
        /// 刪除(Hide)信頭時找不到資料
        /// </summary>
        DeleteLetterheadNoData = 4031,

        /// <summary>
        /// 信頭圖像序號重複
        /// </summary>
        LetterheadImageSequenceRepeat = 4101,
       
        /// <summary>
        /// 信頭圖像無資料
        /// </summary>
        LetterheadImageNoData = 4102,

        /// <summary>
        /// 信頭圖片中已有草稿或是待審的資料
        /// </summary>
        LetterheadImageHaveDraftReviewStatus = 4103,

        /// <summary>
        /// 信頭圖像建立失敗
        /// </summary>
        CreateLetterheadImageFailed = 4110,

        /// <summary>
        /// 信頭圖像序號重複
        /// </summary>
        CreateLetterheadImageSequenceRepeat = 4111,

        /// <summary>
        /// 信頭圖像更新失敗
        /// </summary>
        UpdateLetterheadImageFailed = 4120,

        /// <summary>
        /// 信頭圖像更新時序號重複
        /// </summary>
        UpdateLetterheadImageSequenceRepeat = 4121,

        /// <summary>
        /// 信頭圖像更新時找不到資料
        /// </summary>
        UpdateLetterheadImageNoData = 4122,

        /// <summary>
        /// 信頭圖像刪除失敗
        /// </summary>
        DeleteLetterheadImageFailed = 4130,

        /// <summary>
        /// 信頭圖像刪除時找不到資料
        /// </summary>
        DeleteLetterheadImageNoData = 4131,


        /// <summary>
        /// 上傳失敗
        /// </summary>
        FileUploadFailed = 6001,
    }
}
