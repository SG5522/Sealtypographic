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
        /// 資料庫客戶印鑑建立失敗
        /// </summary>
        CustomerSealCreateFailed = 2100,

        /// <summary>
        /// 資料庫客戶印鑑序號重複
        /// </summary>
        CustomerSealSequenceRepeat = 2101,
        /// <summary>
        /// 資料庫客戶印鑑無資料
        /// </summary>
        CustomerSealNoData = 2102,

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
        /// 資料庫信頭資料建立失敗
        /// </summary>
        LetterheadCreateFailed = 4000,

        /// <summary>
        /// 資料庫信頭編號重複
        /// </summary>
        LetterheadNumberRepeat = 4001,

        /// <summary>
        /// 資料庫信頭無資料
        /// </summary>
        LetterheadNoData = 4002,

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
