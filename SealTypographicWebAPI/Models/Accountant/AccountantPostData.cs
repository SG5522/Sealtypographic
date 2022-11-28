namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計資料
    /// </summary>
    public class AccountantPostData
    {
        /// <summary>
        /// 會計師ID
        /// </summary>   
        /// <example>ACC001</example>
        public string Id { get; set; } = null!;

        /// <summary>
        /// 名稱
        /// </summary>
        /// <example>測試</example>
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
        /// <example>0</example>
        public int Status { get; set; }

        /// <summary>
        /// 會計師群組ID
        /// 0 無群組
        /// </summary>
        /// <example>0</example>
        public string AccountantGroupId { get; set; }
    }
}
