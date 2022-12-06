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
        /// 無資料
        /// </summary>
        NoData = 404,

        /// <summary>
        /// 伺服器錯誤
        /// </summary>
        InternalServerError = 500,

        /// <summary>
        /// 上傳失敗
        /// </summary>
        FileUploadFailed = 18,
    }
}
