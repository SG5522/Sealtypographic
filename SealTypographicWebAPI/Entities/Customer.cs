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
        public string CustomerNumber { get; set; }
        /// <summary>
        /// 統一編號 (business administration number)
        /// </summary>
        public string BAN { get; set; }

        /// <summary>
        /// 證券代號
        /// </summary>
        public string StockCode { get; set; }

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
        /// 客戶印鑑資料(歷程)
        /// </summary>
        public List<CustomerSealJournal> CustomerSealJournals { get; set; }
    }
}
