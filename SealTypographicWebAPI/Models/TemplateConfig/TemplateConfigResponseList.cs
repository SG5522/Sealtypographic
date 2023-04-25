

using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.TemplateConfig
{
    /// <summary>
    /// 取得圖片群組資料以及回應訊息
    /// </summary>
    public class TemplateConfigResponseList : ResponseViewModel
    {

        /// <summary>
        /// new SealMappingConfigViewModel
        /// </summary>
        public TemplateConfigResponseList ()
        {
            ViewModels = new();
        }

        /// <summary>
        /// 圖片群組資料
        /// </summary>
        public List<TemplateConfigViewModel> ViewModels { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TemplateConfigViewModel : BaseData
    {
        /// <summary>
        /// 名稱
        /// </summary>
        /// <example></example>
        public string? Name { get; set; }

        /// <summary>
        /// 在地化名稱
        /// </summary>
        public string? Localizer { get; set; }
    }
}
