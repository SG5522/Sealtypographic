namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計資料
    /// </summary>
    public class AccountantBaseData
    {
        /// <summary>
        /// 會計師ID
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 啟用日期
        /// </summary>
        public DateTime AvailableDate { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 會計師狀態
        /// 0.待審查
        /// 1.已審查
        /// 2.刪除(系統管理員可以看到資料)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 會計師群組ID
        /// </summary>
        public string AccountantGroupsId { get; set; }
    }
}
