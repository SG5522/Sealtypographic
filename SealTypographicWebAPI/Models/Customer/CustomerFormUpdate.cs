using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶資料(新增或是更新使用)
    /// </summary>
    public class CustomerFormUpdate : BaseUpdateName
    {
        /// <summary>
        /// 公司負責人
        /// </summary>
        /// <example>負責人</example>
        public string CompanyPrincipal { get; set; }

        /// <summary>
        /// 客戶編號(搜尋使用)
        /// </summary>
        /// <example>AAA001</example>
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        public string CustomerNumber { get; set; }

        /// <summary>
        /// 統一編號 (business administration number)
        /// </summary>
        /// <example>12345678</example>        
        [RegularExpression(@"^[0-9]*$")]
        public string BAN { get; set; }

        /// <summary>
        /// 證券代號
        /// </summary>
        /// <example>9666</example>
        [RegularExpression(@"^[0-9]*$")]
        public string StockCode { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        /// <example>台北市大同區環河北路路二段115號5樓</example>
        public string Address { get; set; }

        /// <summary>
        /// 電話
        /// </summary>
        /// <example>28825252</example>
        [RegularExpression(@"^[0-9]*$")]
        public string Telephone { get; set; }

        /// <summary>
        /// 傳真
        /// </summary>
        /// <example>28825252</example>
        [RegularExpression(@"^[0-9]*$")]
        public string Fax { get; set; }

        /// <summary>
        /// 聯絡人
        /// </summary>
        /// <example>映先生</example>
        public string? ContactPerson { get; set; }

        /// <summary>
        /// 聯絡人職稱
        /// </summary>
        /// <example>業務</example>
        public string? ContactTitle { get; set; }
    }
}
