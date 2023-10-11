namespace SealTypographicWebAPI.Models.QuarterYear
{
    /// <summary>
    /// 財報季度
    /// </summary>
    public class QuarterResponse : ResponseViewModel
    {
        /// <summary>
        /// 財報季度
        /// </summary>
        public List<QuarterViewModel> Quarters { get; set; }
    }
}
