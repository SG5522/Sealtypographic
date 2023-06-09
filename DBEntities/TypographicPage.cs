namespace DBEntities
{
    /// <summary>
    /// 排版頁
    /// </summary>
    public class TypographicPage
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 頁數
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 確認是否需要插入空白頁
        /// </summary>
        public bool BlankCheck { get; set; }

        /// <summary>
        /// 確認是否為刪除頁
        /// </summary>
        public bool DeleteCheck { get; set; }

        /// <summary>
        /// 此頁是否加入會計師證明書
        /// </summary>
        public bool IsAccountantCertificate { get; set; }

        /// <summary>
        /// 排版PDF資料表
        /// </summary>
        public TypographicPDF TypographicPDF { get; set; }

        /// <summary>
        /// 各印鑑簽印排版位置
        /// </summary>
        public List<TypographicResourceLocation> TypographicResourceLocations { get; set; }


    }
}
