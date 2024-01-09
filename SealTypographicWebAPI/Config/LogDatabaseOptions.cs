using DBEntities.Consts;

namespace SealTypographicWebAPI.Config
{
    /// <summary>
    /// 匯入LogDatabase AppSetting資料
    /// </summary>
    public class LogDatabaseOptions
    {
        /// <summary>
        /// 連線字串
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Db名稱
        /// </summary>
        public string DatabaseName { get; set; } = string.Empty;
    }
}
