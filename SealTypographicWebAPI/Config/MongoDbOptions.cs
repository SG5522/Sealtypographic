using DBEntities.Consts;

namespace SealTypographicWebAPI.Config
{
    /// <summary>
    /// 匯入AppSetting資料
    /// </summary>
    public class MongoDbOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string ConnectionString { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        public string DatabaseName { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        public string CollectionName { get; set; } = null!;
    }
}
