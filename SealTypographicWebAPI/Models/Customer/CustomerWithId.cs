namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 顧客基本資料(含ID)
    /// </summary>
    public class CustomerWithId
    {
        /// <summary>
        /// 客戶ID(更新或搜尋使用)
        /// </summary>
        public string Id { get; set; } = null!;
    }
}
