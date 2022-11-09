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
        Success = 200,
        /// <summary>
        /// 無資料
        /// </summary>
        NoData = 404,
        /// <summary>
        /// 伺服器錯誤
        /// </summary>
        InternalServerError = 500,
    }
}
