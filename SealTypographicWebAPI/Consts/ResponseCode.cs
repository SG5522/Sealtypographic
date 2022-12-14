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
        /// 資料庫客戶編號重複
        /// </summary>
        CustomerNumberRepeat = 2001,

        /// <summary>
        /// 資料庫客戶印鑑序號重複
        /// </summary>
        CustomerSealSequenceRepeat = 2002,

        /// <summary>
        /// 資料庫客戶資料建立失敗
        /// </summary>
        CustomerCreateFailed = 2003,

        /// <summary>
        /// 資料庫會計師編號重複
        /// </summary>
        AccountantNumberRepeat = 3001,

        /// <summary>
        /// 資料庫會計師簽印已有資料
        /// </summary>
        AccountantSignRepeat = 3002,

        /// <summary>
        /// 資料庫會計師資料建立失敗
        /// </summary>
        AccountantCreateFailed = 3003,

        /// <summary>
        /// 上傳失敗
        /// </summary>
        FileUploadFailed = 4001,
    }
}
