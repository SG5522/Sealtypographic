using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.DbModels
{
    /// <summary>
    /// 客戶資料表
    /// </summary>    
    public class Customer
    {
        /// <summary>
        /// 客戶ID(更新或搜尋使用)
        /// </summary>
        [Key]        
        public string ID { get; set; } = null!;
        /// <summary>
        /// 統一編號 (business administration number)
        /// </summary>
        public string BAN { get; set; } = null!;

        /// <summary>
        /// 證券代號
        /// </summary>
        public string StockCode { get; set; }

        /// <summary>
        /// 公司名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 電話
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 傳真
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 顧客狀態
        /// 0.待審查
        /// 1.已審查
        /// 2.刪除(系統管理員可以看到資料)
        /// </summary>
        public int Status { get; set; }
    }
}
