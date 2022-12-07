using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶資料
    /// </summary>
    public class CustomerForm
    {
        /// <summary>
        /// 客戶編號(更新或搜尋使用)
        /// </summary>
        /// <example>AAA001</example>
        [Required]
        [StringLength(6)]
        public string CustomerNumber { get; set; }

        /// <summary>
        /// 統一編號 (business administration number)
        /// </summary>
        /// <example>12345678</example>
        public string BAN { get; set; }

        /// <summary>
        /// 證券代號
        /// </summary>
        /// <example>9666</example>
        public string StockCode { get; set; }

        /// <summary>
        /// 公司名稱
        /// </summary>
        /// <example>映像有限公司</example>
        public string Name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        /// <example>台北市大同區環河北路路二段115號5樓</example>
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

    }
}
