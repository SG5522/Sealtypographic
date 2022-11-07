namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 客戶資料
    /// </summary>
    public class CustomerListData
    {
        /// <summary>
        /// 顧客ID
        /// </summary>
        public string CustomerID { get; set; } = null!;

        /// <summary>
        /// 統一編號
        /// </summary>
        public string IDnumber { get; set; } = null!;


        /// <summary>
        /// 公司名稱
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// 顧客狀態
        /// 0.待審查
        /// 1.已審查
        /// 2.刪除(系統管理員可以看到資料)
        /// </summary>
        public int Status { get; set; }
    }
}
