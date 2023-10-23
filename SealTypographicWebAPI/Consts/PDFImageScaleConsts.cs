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

        /// <summary>
        /// 客戶公司印鑑縮放參數
        /// </summary>
        public const float CompanySeal = 0.24f;

        /// <summary>
        /// 客戶公司負責人印鑑縮放參數
        /// </summary>
        public const float PresidentSeal = 0.24f;

        /// <summary>
        /// 客戶公司經理人印鑑縮放參數
        /// </summary>
        public const float ManagerSeal = 0.24f;

        /// <summary>
        /// 客戶公司會計主管印鑑縮放參數
        /// </summary>
        public const float AccountingDirectorSeal = 0.24f;

        /// <summary>
        /// 會計師印鑑縮放參數
        /// </summary>
        public const float AccountingSeal = 0.24f;

        /// <summary>
        /// 會計師中文簽名縮放參數
        /// </summary>
        public const float CHSign = 0.24f;

        /// <summary>
        /// 會計師英文簽名縮放參數
        /// </summary>
        public const float ENSign = 0.24f;

        /// <summary>
        /// 會計師舊式簽名縮放參數
        /// </summary>
        public const float OldSign = 0.24f;

        /// <summary>
        /// 信頭圖片縮放
        /// </summary>
        public const float LetterheadImage = 0.24f;

        /// <summary>
        /// 暫存印鑑縮放
        /// </summary>
        public const float TemporarySeal = 0.24f;

        /// <summary>
        /// 其他類型縮放
        /// </summary>
        public const float Other = 0.24f;

    }    
}
