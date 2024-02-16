namespace SealTypographicWebAPI.Models.LogReport
{
    /// <summary>
    /// 紀錄使用會計師簽印 Service時所讀取的資料
    /// </summary>
    public class AccountantLog
    {
        /// <summary>
        /// 會計師Id
        /// </summary>
        public int AccountantId { get; set; }

        /// <summary>
        /// 會計師姓名
        /// </summary>
        public string AccountantName { get; set; }

        /// <summary>
        /// 會計師簽印群組Id
        /// </summary>
        public int AccountantSignGroupId { get; set; }

        /// <summary>
        /// 會計師簽印群組建立日期
        /// </summary>
        public DateTime AccountantSignGroupCreateDate { get; set; }
    }
}
