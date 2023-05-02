namespace SealTypographicWebAPI.Models.AccountantSignTemplate
{
    /// <summary>
    /// 樣板存檔時紀錄當下的圖片
    /// </summary>
    public class AccountantSignTemplateImageView : ResponseViewModel
    {
        /// <summary>
        /// 圖片字串(ImageBase64)
        /// </summary>
        /// <example>image/...</example>
        public string ImageBase64 { get; set; }
    }

}
