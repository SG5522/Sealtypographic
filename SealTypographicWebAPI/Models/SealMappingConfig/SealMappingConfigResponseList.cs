namespace SealTypographicWebAPI.Models.SealMappingConfig
{
    /// <summary>
    /// 取得圖片群組資料以及回應訊息
    /// </summary>
    public class SealMappingConfigResponseList : ResponseViewModel
    {
        /// <summary>
        /// 圖片群組資料
        /// </summary>
        public List<SealMappingConfigViewModel> SealMappingConfigViewModel { get; set; }
    }
}
