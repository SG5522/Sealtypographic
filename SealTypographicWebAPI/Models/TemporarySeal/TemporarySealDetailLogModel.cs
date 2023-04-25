using SealTypographicWebAPI.Models.BaseModels;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.TemporarySeal
{
    /// <summary>
    /// 臨時章
    /// </summary>
    public class TemporarySealLogModel : BaseData
    {
        /// <summary>
        /// 印鑑編號(排序)
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 紀錄檔名使用
        /// </summary>        
        public string ImageFileName { get; set; }

    }

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
