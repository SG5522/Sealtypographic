using SealTypographicWebAPI.Models.BaseModels;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.CustomerSealTemplate
{
    /// <summary>
    /// 客戶印鑑樣板分頁單列
    /// </summary>
    public class CustomerSealTemplateLogModel : BaseData
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 縮圖字串(Base64)
        /// </summary>
        /// <example>image/...</example>
        public string ImageFullPath { get; set; }
    }

    /// <summary>
    /// 客戶印鑑樣板分頁列表
    /// </summary>
    public class CustomerSealTemplatePaginateLog : PaginateViewModel
    {
        /// <summary>
        /// New ViewModels
        /// </summary>
        public CustomerSealTemplatePaginateLog() 
        {
            LogModels = new ();
        }

        /// <summary>
        /// 樣板列表
        /// </summary>           
        public List<CustomerSealTemplateLogModel> LogModels { get; set; }
    }
}
