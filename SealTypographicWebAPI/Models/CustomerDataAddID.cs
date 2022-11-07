namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 顧客基本資料(含ID)
    /// </summary>
    public class CustomerDataAddID : CustomerData
    {
        /// <summary>
        /// 客戶ID(更新或搜尋使用)
        /// </summary>
        public string ID { get; set; } = null!;
    }
}
