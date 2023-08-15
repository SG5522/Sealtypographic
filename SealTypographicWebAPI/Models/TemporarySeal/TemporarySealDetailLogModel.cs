namespace SealTypographicWebAPI.Models.TemporarySeal
{
    /// <summary>
    /// 臨時章詳細
    /// </summary>
    public class TemporarySealDetailLogModel : ResponseViewModel
    {
        /// <summary>
        /// New ViewModels
        /// </summary>
        public TemporarySealDetailLogModel() 
        {
            ViewModels = new ();
        }

        /// <summary>
        /// 客戶ID
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 客戶名稱
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 臨時章
        /// </summary>
        public List<TemporarySealLogModel> ViewModels{ get; set; }
    }
}
