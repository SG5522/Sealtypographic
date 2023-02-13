using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models
{
    /// <summary>
    ///  (使用印鑑、簽名、LOGO)
    /// </summary>
    public class ResponseCodeViewModel
    {
        /// <summary>
        /// 回應代碼
        /// </summary>        
        public int Code { get; set; }

        /// <summary>
        /// 代碼詳述
        /// </summary>
        /// <example></example>
        public string? Description { get; set; }
    }

    /// <summary>
    /// 取得圖片群組資料以及回應訊息
    /// </summary>
    public class ResponseCodeList
    {
        /// <summary>
        /// new SealMappingConfigViewModel
        /// </summary>
        public ResponseCodeList()
        {
            ViewModels = new();
        }

        /// <summary>
        /// 圖片群組資料
        /// </summary>
        public List<ResponseCodeViewModel> ViewModels { get; set; }
    }
}
