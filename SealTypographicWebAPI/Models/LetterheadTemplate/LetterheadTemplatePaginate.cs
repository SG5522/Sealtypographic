using SealTypographicWebAPI.Models.BaseModels;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.LetterheadTemplate
{
    /// <summary>
    /// 信頭樣板分頁單列
    /// </summary>
    public class LetterheadTemplateViewModel : BaseData
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 縮圖字串(Base64)
        /// </summary>
        /// <example>image/...</example>
        public string ThumbnailBase64 { get; set; }

        /// <summary>
        /// 檔案路徑
        /// </summary>
        [JsonIgnore]
        public string ImageFullPath { get; set; }
    }

    /// <summary>
    /// 信頭樣板分頁列表
    /// </summary>
    public class LetterheadTemplatePaginate : PaginateViewModel
    {
        /// <summary>
        /// New ViewModels
        /// </summary>
        public LetterheadTemplatePaginate() 
        {
            ViewModels = new ();
        }

        /// <summary>
        /// 樣板列表
        /// </summary>           
        public List<LetterheadTemplateViewModel> ViewModels { get; set; }
    }
}
