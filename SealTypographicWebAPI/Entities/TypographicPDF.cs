namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// PDF排版資訊
    /// </summary>
    public class TypographicPDF
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 路徑
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 季度
        /// </summary>
        public string Quarter { get; set; }

        /// <summary>
        /// 客戶Id
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// 顧客
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// 排版頁
        /// </summary>
        public List<TypographicPage> TypographicPages { get; set; }

    }
}
