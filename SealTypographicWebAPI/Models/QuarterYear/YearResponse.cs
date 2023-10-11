namespace SealTypographicWebAPI.Models.QuarterYear
{
    /// <summary>
    /// 稅報年度
    /// </summary>
    public class YearResponse : ResponseViewModel
    {
        /// <summary>
        /// 稅報年度
        /// </summary>
        public List<YearViewModel> Years { get; set; }
    }
}
