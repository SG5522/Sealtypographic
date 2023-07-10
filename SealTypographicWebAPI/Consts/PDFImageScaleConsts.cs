namespace SealTypographicWebAPI.Consts
{
    /// <summary>
    /// PDF縮放參數
    /// 由於SpirePDF的預設解出來的圖片皆為96DPI
    /// 以下參數皆由DPI/ 96f
    /// </summary>
    public class PDFImageScaleConsts
    {
        /// <summary>
        /// 預設值為300DPI為參數
        /// </summary>
        public const double Default = 3.125;

        /// <summary>
        /// DPI為600的參數
        /// </summary>
        public const double DPI600 = 6.25;

        /// <summary>
        /// 原始倍率
        /// </summary>
        public const double Original = 1.0;
    }
}
