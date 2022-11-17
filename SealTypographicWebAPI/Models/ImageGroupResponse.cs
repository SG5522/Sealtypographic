namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 取得圖片群組資料以及回應訊息
    /// </summary>
    public class ImageGroupResponse : Response
    {
        /// <summary>
        /// 圖片群組資料
        /// </summary>
        public ImageGroupViewModel ImageGroup { get; set; }
    }
}
