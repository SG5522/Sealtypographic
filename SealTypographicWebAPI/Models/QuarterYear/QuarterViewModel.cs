namespace SealTypographicWebAPI.Models.QuarterYear
{
    /// <summary>
    /// 財報季度
    /// </summary>
    public class QuarterViewModel
    {
        /// <summary>
        /// 財報季度Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 西元年季度
        /// </summary>
        public string GregorianQuarter { get; set; }

        /// <summary>
        /// 民國年季度
        /// </summary>
        public string DisplayQuarter { get; set; }
    }
}
