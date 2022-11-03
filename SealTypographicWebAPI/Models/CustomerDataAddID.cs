namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 取得基本資料
    /// </summary>
    public class CustomerDataAddID : CustomerData
    {
        /// <summary>
        /// 客戶ID(更新或搜尋使用)
        /// </summary>
        public int ID { get; set; }
    }
}
