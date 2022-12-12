namespace SealTypographicWebAPI.Consts
{
    /// <summary>
    /// API傳輸結果代碼(暫定)
    /// </summary>
    public enum ResponseCode
    {
        /// <summary>
        /// 資料庫欄位限制唯一約束錯誤回傳
        /// </summary>
        UniqueConstraintFailed = 19,

        /// <summary>
        /// 回傳成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// DB無資料
        /// </summary>
        DbNoData = 100,

        /// <summary>
        /// 資料庫錯誤
        /// </summary>
        DbError = 101,

        /// <summary>
        /// 資料庫客戶印鑑序號重複
        /// </summary>
        CustomerSealSequenceRepeat = 21,

        /// <summary>
        /// 資料庫會計師簽印已有資料
        /// </summary>
        AccountSignHaveData = 31,

        /// <summary>
        /// 上傳失敗
        /// </summary>
        FileUploadFailed = 18,
    }
}
