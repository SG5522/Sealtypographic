using System.ComponentModel;

namespace DJSpire.Consts
{
    public enum PDFColor
    {
        /// <summary>
        /// 原始顏色(彩色)
        /// </summary>
        Original,
        /// <summary>
        /// 灰階(黑白)
        /// </summary>
        GrayScale
    }

    /// <summary>
    /// 印鑑顏色
    /// </summary>
    public enum SealColor
    {
        /// <summary>
        /// 原色
        /// </summary>
        [Description("原色")]
        Default = 0,

        /// <summary>
        /// 紅色
        /// </summary>
        [Description("紅色")]
        Red,

        /// <summary>
        /// 藍色
        /// </summary>
        [Description("藍色")]
        Blue,

        /// <summary>
        /// 黑色
        /// </summary>
        [Description("黑色")]
        Black
    }
}
