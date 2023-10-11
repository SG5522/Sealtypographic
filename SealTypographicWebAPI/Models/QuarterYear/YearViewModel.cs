namespace SealTypographicWebAPI.Models.QuarterYear
{
    /// <summary>
    /// 稅報年度
    /// </summary>
    public class YearViewModel
    {
        /// <summary>
        /// 財報季度Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 西元年季度
        /// </summary>
        public string GregorianYear { get; set; }

        /// <summary>
        /// 民國年季度
        /// </summary>
        public string TaiwanYear { get; set; }
    }
}
