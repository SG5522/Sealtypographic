using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.CustomerSealTemplate
{
    /// <summary>
    /// 客樣樣板顯示存檔時當下紀錄的圖片
    /// </summary>
    public class CustomerSealTemplateImageView : BaseData
    {
        /// <summary>
        /// 縮圖字串(ImageBase64)
        /// </summary>
        /// <example>image/...</example>
        public string ImageBase64 { get; set; }
    }

}
