namespace SealTypographicWebAPI.Consts
{
    /// <summary>
    /// 客戶、會計師、印鑑、簽名的狀態
    /// </summary>
    public enum ReviewStatus : int
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
        Approval = 10,

        /// <summary>
        /// 退件
        /// </summary>
        Reject = 20,

        /// <summary>
        /// 隱藏(被刪除時的狀態)
        /// </summary>
        Hidden = 99,

    }

    /// <summary>
    /// 每頁資料上限
    /// </summary>
    public enum PageSizeLimit : int
    {
        /// <summary>
        /// 最小值
        /// </summary>
        Min = 5,
        /// <summary>
        /// 最大值
        /// </summary>
        Max = 100
    }

    /// <summary>
    /// 啟用日期
    /// </summary>
    public enum Available
    {
        /// <summary>
        /// 未啟用
        /// </summary>
        NotActivated,
        /// <summary>
        /// 啟用
        /// </summary>
        Activated
    }

    /// <summary>
    /// 印鑑類型
    /// </summary>
    public enum SealType : int
    {
        /// <summary>
        /// 客戶
        /// </summary>
        Customer = 1,

        /// <summary>
        /// 會計師
        /// </summary>
        Accountant = 2,

        /// <summary>
        /// 信頭
        /// </summary>
        Letterhead = 3,
    }
}
