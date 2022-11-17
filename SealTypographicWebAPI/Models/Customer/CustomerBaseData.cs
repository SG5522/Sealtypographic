namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶資料
    /// </summary>
    public class CustomerBaseData
    {
        /// <summary>
        /// 客戶ID(更新或搜尋使用)
        /// </summary>
        /// <example>AAA001</example>
        public string Id { get; set; } = null!;

        /// <summary>
        /// 統一編號 (business administration number)
        /// </summary>
        /// <example>12345678</example>
        public string BAN { get; set; } = null!;

        /// <summary>
        /// 證券代號
        /// </summary>
        /// <example>9999</example>
        public string StockCode { get; set; }

        /// <summary>
        /// 公司名稱
        /// </summary>
        /// <example>xxxCompany</example>
        public string Name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        /// <example>XX市XX區XX路XX巷XX號XX樓</example>
        public string Address { get; set; }

        /// <summary>
        /// 電話
        /// </summary>
        /// <example>28825252</example>
        public string Telephone { get; set; }

        /// <summary>
        /// 傳真
        /// </summary>
        /// <example>28825252</example>
        public string Fax { get; set; }

        /// <summary>
        /// 顧客狀態
        /// 0.待審查
        /// 1.已審查
        /// 2.刪除(系統管理員可以看到資料)
        /// </summary>
        /// <example>0</example>
        public int Status { get; set; }
    }
}
