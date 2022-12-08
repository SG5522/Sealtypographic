namespace SealTypographicWebAPI.Models.SealMappingConfig
{
    /// <summary>
    /// 取得圖片群組資料以及回應訊息
    /// </summary>
    public class SealMappingConfigResponsePage : ResponseViewModel
    {
        /// <summary>
        /// 現在頁數
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// 資料筆數
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 圖片群組資料
        /// </summary>
        public List<SealMappingConfigViewModel> ImageGroup { get; set; }
    }
}
