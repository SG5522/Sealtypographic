using DBEntities.Consts;

namespace SealTypographicWebAPI.Config
{
    /// <summary>
    /// 匯入AppSetting資料
    /// </summary>
    public class TypographyEditImagePathOptions
    {
        /// <summary>
        /// 印鑑路徑
        /// </summary>
        public string RootPath { get; set; } = string.Empty;

        /// <summary>
        /// 縮圖比例
        /// </summary>
        public float ResizeScale { get; set; }
    }
}
