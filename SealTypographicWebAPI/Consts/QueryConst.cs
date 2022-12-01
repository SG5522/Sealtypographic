namespace SealTypographicWebAPI.Consts
{
    /// <summary>
    /// 客戶、會計師、印鑑、簽名的狀態
    /// </summary>
    public enum Status : int
    {
        /// <summary>
        /// 全部
        /// </summary>
        All = -1,

        /// <summary>
        /// 待審
        /// </summary>
        Pending = 0,

        /// <summary>
        /// 通過(審核完成)
        /// </summary>
        Approval = 1,

        /// <summary>
        /// 退件
        /// </summary>
        Reject = 2,

        /// <summary>
        /// 隱藏(被刪除時的狀態)
        /// </summary>
        Hidden = 3,

    }

    /// <summary>
    /// 每頁資料上限
    /// </summary>
    public static class PageSizeLimit
    {
        /// <summary>
        /// 最小值
        /// </summary>
        public const int Min = 5;
        /// <summary>
        /// 最大值
        /// </summary>
        public const int Max = 100;
    }

    /// <summary>
    /// 上傳文件型態
    /// </summary>
    public enum UploadType
    {
        /// <summary>
        /// 客戶印鑑授權書
        /// </summary>
        CustomerAuthorization = 1,

        /// <summary>
        /// 會計印鑑簽名授權書
        /// </summary>
        AccountantAuthorization = 2,

        /// <summary>
        /// 信頭
        /// </summary>
        LetterheadImage = 3,

    }
}
