using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.TemporarySeal
{
    /// <summary>
    /// 臨時章
    /// </summary>
    public class TemporarySealViewModel : BaseSeal
    {
        /// <summary>
        /// 印鑑編號(排序)
        /// </summary>
        public int Sequence { get; set; }
    }

    /// <summary>
    /// 臨時章詳細
    /// </summary>
    public class TemporarySealDetailViewModel : ResponseViewModel
    {
        /// <summary>
        /// New ViewModels
        /// </summary>
        public TemporarySealDetailViewModel() 
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
        public List<TemporarySealViewModel> ViewModels{ get; set; }
    }
}
