namespace SealTypographicWebAPI.Consts
{
    /// <summary>
    /// 客戶、會計師、印鑑、簽名的狀態
    /// </summary>
    public enum ReviewStatus : sbyte
    {
        /// <summary>
        /// 全部
        /// </summary>
        All = -1,

        /// <summary>
        /// 通過(審核完成)(啟用)
        /// </summary>
        Approval = 0,

        /// <summary>
        /// 啟用
        /// </summary>
        Activated = 1,

        /// <summary>
        /// 未啟用
        /// </summary>
        NotActivated = 2,

        /// <summary>
        /// 退件
        /// </summary>
        Reject = 10,

        /// <summary>
        /// 草稿
        /// </summary>
        Draft = 20,

        /// <summary>
        /// 待審
        /// </summary>
        Pending = 30,

        /// <summary>
        /// 作廢
        /// </summary>
        Invalid = 40,
    }

    /// <summary>
    /// 刪除狀態
    /// </summary>
    public enum DeleteStatus : byte
    {
        /// <summary>
        /// 無標記
        /// </summary>
        NO = 0,

        /// <summary>
        /// 隱藏或標記刪除
        /// </summary>
        Yes = 1,
    }

    /// <summary>
    /// 每頁資料上限
    /// </summary>
    public enum PageSizeLimit : byte
    {
        /// <summary>
        /// 最小值
        /// </summary>
        Min = 5,
        /// <summary>
        /// 最大值
        /// </summary>
        Max = 20
    }

    /// <summary>
    /// 啟用日期
    /// </summary>
    public enum Available : byte
    {
        /// <summary>
        /// 未啟用
        /// </summary>
        NotActivated = 0,
        /// <summary>
        /// 啟用
        /// </summary>
        Activated = 1,
    }

    /// <summary>
    /// 印鑑類型
    /// </summary>
    public enum SealType : byte
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

    /// <summary>
    /// Db處理資料動作方式
    /// </summary>
    public enum DbActionMode : byte
    {
        /// <summary>
        /// 建檔
        /// </summary>
        Create = 0,

        /// <summary>
        /// 更新檔案
        /// </summary>
        Update = 1,
    }
}
