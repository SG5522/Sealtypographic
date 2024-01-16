namespace SealTypographicWebAPI.Models.LogReport
{
    /// <summary>
    /// LogViewModel基本結構
    /// </summary>
    public abstract class LogViewModelBase
    {
        /// <summary>
        /// 使用者id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 使用者姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 紀錄日期
        /// </summary>
        public DateTime DateTime { get; set; }
    }
}
