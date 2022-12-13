using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶資料
    /// </summary>
    public class CustomerDetail : CustomerViewModel
    {        
        /// <summary>
        /// 公司負責人
        /// </summary>
        public string President { get; set; }

        /// <summary>
        /// 證券代號
        /// </summary>
        /// <example>9666</example>
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
        public string Telephone { get; set; }

        /// <summary>
        /// 傳真
        /// </summary>
        /// <example>28825252</example>
        public string Fax { get; set; }



        /// <summary>
        /// 聯絡人
        /// </summary>
        public string? ContactPerson { get; set; }

        /// <summary>
        /// 聯絡人職稱
        /// </summary>
        public string? ContactTitle { get; set; }
    }

    /// <summary>
    /// 客戶資料
    /// </summary>
    public class CustomerDetailViewModel : ResponseViewModel
    {
        /// <summary>
        /// 客戶基本資料
        /// </summary>
        public CustomerDetail? CustomerDetail { get; set; }
    }
}
