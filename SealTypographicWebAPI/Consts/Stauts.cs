namespace SealTypographicWebAPI.Consts
{
    /// <summary>
    /// 客戶、會計師、印鑑、簽名的狀態
    /// </summary>
    public enum Status : int
    {
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
}
