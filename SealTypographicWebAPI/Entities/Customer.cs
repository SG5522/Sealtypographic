using SealTypographicWebAPI.Entities.BaseEntities;

namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 客戶資料表
    /// </summary>        
    public class Customer : BaseNameData
    {
        /// <summary>
        /// 客戶編號
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 簡稱
        /// </summary>
        public string? ShortName { get; set; }

        /// 公司負責人
        public string? President { get; set; }

        /// <summary>
        /// 統一編號 (business administration number)
        /// </summary>
        public string? BAN { get; set; }

        /// <summary>
        /// 證券代號
        /// </summary>
        public string? StockCode { get; set; }

        /// <summary>
        /// 郵遞區號
        /// </summary>
        public string? PostalCode { get; set; }

        /// <summary>
        /// 地址：城市
        /// </summary>
        public string? AddressCity { get; set; }

        /// <summary>
        /// 地址：市區
        /// </summary>
        public string? AddressArea { get; set; }

        /// <summary>
        /// 地址：路街
        /// </summary>
        public string? AddressStreet { get; set; }

        /// <summary>
        /// 巷弄樓
        /// </summary>
        public string? AddressLocate { get; set; }

        /// <summary>
        /// 聯絡人
        /// </summary>
        public string? ContactName { get; set; }

        /// <summary>
        /// 聯絡人職稱
        /// </summary>
        public string? ContactTitle { get; set; }

        /// <summary>
        /// 電話
        /// </summary>
        public string? Telephone { get; set; }

        /// <summary>
        /// 傳真
        /// </summary>
        public string? Fax { get; set; }

        /// <summary>
        /// 客戶印鑑資料(歷程)
        /// </summary>
        public List<CustomerSealJournal> CustomerSealJournals { get; set; }
    }
}
