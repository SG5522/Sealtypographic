using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶資料(新增或是更新使用)
    /// </summary>
    public class CustomerForm : BaseCreateName
    {
        /// <summary>
        /// 客戶編號(更新或搜尋使用)
        /// </summary>
        /// <example>AAA001</example>
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        public string Code { get; set; }

        /// <summary>
        /// 公司負責人
        /// </summary>
        /// <example>負責人</example>
        public string? President { get; set; }

        /// <summary>
        /// 客戶公司簡稱
        /// </summary>
        /// <example>台電</example>
        public string? ShortName { get; set; }

        /// <summary>
        /// 統一編號 (business administration number)
        /// </summary>
        /// <example>12345678</example>        
        [StringLength(8)]
        [RegularExpression(@"^[0-9]*$")]
        public string? BAN { get; set; }

        /// <summary>
        /// 證券代號
        /// </summary>
        /// <example>9666</example>
        [StringLength(4)]
        [RegularExpression(@"^[0-9]*$")]        
        public string? StockCode { get; set; }


        /// <summary>
        /// 郵遞區號
        /// </summary>
        [StringLength(6)]
        [RegularExpression(@"^[0-9]*$")]
        public string? PostalCode { get; set; }

        /// <summary>
        /// 地址：城市
        /// </summary>
        /// <example>台北市</example>
        public string? AddressCity { get; set; }

        /// <summary>
        /// 地址：市區
        /// </summary>
        /// <example>大同區</example>
        public string? AddressArea { get; set; }

        /// <summary>
        /// 地址：路街
        /// </summary>
        /// <example>環河北路</example>
        public string? AddressStreet { get; set; }

        /// <summary>
        /// 巷弄樓
        /// </summary>
        /// <example>二段115號5樓</example>
        public string? AddressLocate { get; set; }

        /// <summary>
        /// 電話
        /// </summary>
        /// <example>28825252</example>
        [StringLength(10)]
        [RegularExpression(@"^[0-9]*$")]        
        public string? Telephone { get; set; }

        /// <summary>
        /// 傳真
        /// </summary>
        /// <example>28825252</example>
        [StringLength(8)]
        [RegularExpression(@"^[0-9]*$")]
        public string? Fax { get; set; }

        /// <summary>
        /// 聯絡人
        /// </summary>
        /// <example>映先生</example>
        public string? ContactName { get; set; }

        /// <summary>
        /// 聯絡人職稱
        /// </summary>
        /// <example>業務</example>
        public string? ContactTitle { get; set; }
    }
}
