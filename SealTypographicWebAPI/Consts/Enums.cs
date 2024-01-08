using System.ComponentModel;

namespace SealTypographicWebAPI.Consts
{
    /// <summary>
    /// 圖片型態
    /// </summary>    
    public enum ImageType
    {
        /// <summary>
        /// 
        /// </summary>
        Jpg,

        /// <summary>
        /// 
        /// </summary>
        Png,

        /// <summary>
        /// 
        /// </summary>
        Bmp 
    }

    /// <summary>
    /// PDF
    /// </summary>
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

    /// <summary>
    /// 動作類別(紀錄使用)
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// 客戶資料查詢
        /// </summary>
        [Description("客戶資料查詢")]
        CustomerQuery = 1,

        /// <summary>
        /// 會計師資料查詢
        /// </summary>
        [Description("會計師資料查詢")]
        AccountantQuery = 2,

        /// <summary>
        /// 財報印鑑查詢
        /// </summary>
        [Description("財報印鑑查詢")]
        FinancialReportSealQuery = 3,

        /// <summary>
        /// 稅報印鑑查詢
        /// </summary>
        [Description("稅報印鑑查詢")]
        TaxReportSealQuery = 4,

        /// <summary>
        /// 會計師簽印查詢
        /// </summary>
        [Description("會計師簽印查詢")]
        AccountantSignQuery = 5,

        /// <summary>
        /// 登入
        /// </summary>
        [Description("登入")]
        Login = 10,

        /// <summary>
        /// 登入
        /// </summary>
        [Description("登出")]
        Logout = 11,
    }
}
