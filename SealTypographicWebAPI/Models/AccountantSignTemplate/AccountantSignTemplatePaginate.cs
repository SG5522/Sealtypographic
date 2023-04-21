using SealTypographicWebAPI.Models.BaseModels;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.AccountantSignTemplate
{
    /// <summary>
    /// 會計師簽印樣板分頁單列
    /// </summary>
    public class AccountantSignTemplateViewModel : BaseData
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
    /// 會計師簽印樣板分頁列表
    /// </summary>
    public class AccountantSignTemplatePaginate : PaginateViewModel
    {
        /// <summary>
        /// New ViewModels
        /// </summary>
        public AccountantSignTemplatePaginate() 
        {
            ViewModels = new ();
        }

        /// <summary>
        /// 分頁資料
        /// </summary>           
        public List<AccountantSignTemplateViewModel> ViewModels { get; set; }
    }
}
