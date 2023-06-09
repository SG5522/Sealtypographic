using System.ComponentModel;

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
        [Description("回傳成功")]
        Success = 0,

        /// <summary>
        /// DB無資料
        /// </summary>
        [Description("資料庫無資料")]
        DbNoData = 1000,

        /// <summary>
        /// 資料庫處理錯誤
        /// </summary>
        [Description("資料庫異常，請聯絡工程師")]
        DbError = 1001,

        /// <summary>
        /// 客戶無資料
        /// </summary>
        [Description("查無客戶資料")]
        CustomeNoData = 2001,

        /// <summary>
        /// 資料庫客戶資料建立失敗
        /// </summary>
        [Description("客戶資料建立失敗")]
        CreateCustomerFailed = 2010,

        /// <summary>
        /// 建立客戶資料時編號重複
        /// </summary>                
        [Description("客戶編號重複，無法建檔，請重新確認編號")]
        CreateCustomerNumberRepeat = 2011,

        /// <summary>
        /// 更新客戶資料找不到資料
        /// </summary>
        [Description("查無客戶資料，無法更新客戶資料")]
        UpdateCustomerNoData = 2021,


        /// <summary>
        /// 刪除客戶時找不到資料
        /// </summary>
        [Description("查無客戶資料，無法刪除客戶資料")]
        DeleteCustomerNoData = 2031,

        /// <summary>
        /// 客戶印鑑序號重複
        /// </summary>
        [Description("客戶印鑑序號重複，請確認印鑑")]
        CustomerSealSequenceRepeat = 2101,

        /// <summary>
        /// 資料庫客戶印鑑無資料
        /// </summary>
        [Description("查無客戶印鑑資料")]
        CustomerSealNoData = 2102,

        /// <summary>
        /// 客戶印鑑建立時序號重複
        /// </summary>
        [Description("客戶印鑑序號重複，無法建立新的印鑑")]
        CreateCustomerSealSequenceRepeat = 2111,

        /// <summary>
        /// 客戶印鑑建立時季度重複
        /// </summary>        
        [Description("客戶印鑑季度重複，無法建立新的印鑑")]
        CreateCustomerSealQuarterRepeat = 2112,

        /// <summary>
        /// 更新客戶印鑑時序號重複
        /// </summary>
        [Description("客戶印鑑序號重複，無法更新印鑑")]
        UpdateCustomerSealSequenceRepeat = 2121,

        /// <summary>
        /// 更新客戶印鑑時找不到資料
        /// </summary>
        [Description("查無客戶印鑑資料，無法更新印鑑")]
        UpdateCustomerSealNoData = 2122,


        /// <summary>
        /// 刪除客戶印鑑時找不到資料
        /// </summary>
        [Description("查無客戶印鑑資料，無法刪除印鑑")]
        DeleteCustomerSealNoData = 2131,

        /// <summary>
        /// 會計師無資料
        /// </summary>
        [Description("查無會計師資料")]
        AccountantNoData = 3001,

        /// <summary>
        /// 會計師編號重複
        /// </summary>
        [Description("會計師編號重複，請重新確認")]
        AccountantNumberRepeat = 3002,

        /// <summary>
        /// 建立會計師資料失敗
        /// </summary>
        [Description("建立會計師資料失敗")]
        CreateAccountantFailed = 3010,

        /// <summary>
        /// 建立會計師資料時編號重複
        /// </summary>
        [Description("會計師編號重複，無法建檔，請重新確認編號")]
        CreateAccountantNumberRepeat = 3011,

        /// <summary>
        /// 更新會計師資料時找不到
        /// </summary>
        [Description("查無會計師資料，無法更新會計師資料")]
        UpdateAccountantNoData = 3021,

        /// <summary>
        /// 刪除會計師時找不到資料
        /// </summary>
        [Description("查無會計師資料，無法刪除會計師資料")]
        DeleteAccountantNoData = 3031,

        /// <summary>
        /// 會計師簽印無資料
        /// </summary>
        [Description("查無會計師簽印資料")]
        AccountantSignNoData = 3101,

        /// <summary>
        /// 建立會計計簽印時發現重複
        /// </summary>
        [Description("會計師簽印序號重複，無法建立新的印鑑")]
        CreateAccountantSignRepeat = 3111,

        /// <summary>
        /// 更新會計師簽印時發現重複
        /// </summary>
        [Description("會計師簽印序號重複，無法更新印鑑")]
        UpdateAccountantSignRepeat = 3121,

        /// <summary>
        /// 更新會計師簽印時找不到資料
        /// </summary>
        [Description("查無會計師簽印資料，無法更新印鑑")]
        UpdateAccountantSignNoData = 3122,

        /// <summary>
        /// 刪除會計師簽印時找不到資料
        /// </summary>
        [Description("查無會計師簽印資料，無法刪除印鑑")]
        DeleteAccountantSignNoData = 3131,


        /// <summary>
        /// 會計師群組找不到資料
        /// </summary>
        [Description("查無會計師群組資料")]
        AccountantGroupNoData = 3200,

        /// <summary>
        /// 會計師群組建立時編號重複
        /// </summary>
        [Description("會計師群組編號重複，無法建立群組，請重新確認編號")]
        CreateAccountantGroupNumberRepeat = 3211,

        /// <summary>
        /// 更新會計師群組找不到資料
        /// </summary>
        [Description("查無會計師群組資料，無法更新會計師群組資料")]
        UpdateAccountantGroupNoData = 3221,

        /// <summary>
        /// 刪除會計師群組時找不到資料
        /// </summary>
        [Description("查無會計師群組資料，無法刪除會計師群組資料")]
        DeleteAccountantGroupNoData = 3231,

        /// <summary>
        /// 信頭無資料
        /// </summary>
        [Description("查無信頭資料")]
        CreateLetterheadNoData = 4012,

        /// <summary>
        /// 更新信頭找不到資料
        /// </summary>
        [Description("查無信頭資料，無法更新信頭資料")]
        UpdateLetterheadNoData = 4021,

        /// <summary>
        /// 刪除信頭時找不到資料
        /// </summary>
        [Description("查無信頭資料，無法刪除信頭資料")]
        DeleteLetterheadNoData = 4031,

        /// <summary>
        /// 信頭圖像無資料
        /// </summary>
        [Description("查無信頭圖像資料")]
        LetterheadImageNoData = 4102,

        /// <summary>
        /// 更新信頭圖像時找不到資料
        /// </summary>
        [Description("查無信頭圖像資料，無法更新信頭圖像資料")]
        UpdateLetterheadImageNoData = 4122,

        /// <summary>
        /// 刪除信頭圖像找不到資料
        /// </summary>
        [Description("查無信頭圖像資料，無法刪除信頭圖像資料")]
        DeleteLetterheadImageNoData = 4131,

        /// <summary>
        /// 上傳失敗
        /// </summary>
        [Description("檔案上傳失敗，請確認網路環境")]
        FileUploadFailed = 6001,

        /// <summary>
        /// 找不到上傳資料
        /// </summary>
        [Description("查無上傳資料，請確認或重新上傳")]
        FileUploadNoData = 6002,

        /// <summary>
        /// 找不到臨時章
        /// </summary>
        [Description("查無臨時章")]
        TemporarySealNoData = 7001,

        /// <summary>
        /// 更新臨時章找不到資料
        /// </summary>
        [Description("查無臨時章資料，無法更新臨時章資料")]
        UpdateTemporarySealNoData = 7003,

        /// <summary>
        /// 刪除臨時章時找不到資料
        /// </summary>
        [Description("查無臨時章資料，無法刪除臨時章資料")]
        DeleteTemporarySealNoData = 7004,

        /// <summary>
        /// 刪除客戶印鑑樣板時找不到資料
        /// </summary>
        [Description("查無客戶印鑑樣板，無法刪除資料")]
        DeleteCustomerSealTemplateNoData = 8004,

        /// <summary>
        /// 刪除會計師簽印樣板時找不到資料
        /// </summary>
        [Description("查無會計師簽印樣板，無法刪除資料")]
        DeleteAccountantSignTemplateNoData = 8104,

        /// <summary>
        /// 刪除信頭樣板時找不到資料
        /// </summary>
        [Description("查無信頭樣板，無法刪除資料")]
        DeleteLetterImageTemplateNoData = 8204,

        /// <summary>
        /// 回傳失敗
        /// </summary>
        [Description("回傳失敗")]
        Error = 9999,
    }
}
