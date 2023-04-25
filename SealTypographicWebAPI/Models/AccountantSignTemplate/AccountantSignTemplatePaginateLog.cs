using SealTypographicWebAPI.Models.BaseModels;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.AccountantSignTemplate
{
    /// <summary>
    /// 會計師簽印樣板分頁單列
    /// </summary>
    public class AccountantSignTemplateLogModel : BaseData
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
    /// 會計師簽印樣板分頁列表(log)
    /// </summary>
    public class AccountantSignTemplatePaginateLog : PaginateViewModel
    {
        /// <summary>
        /// New ViewModels
        /// </summary>
        public AccountantSignTemplatePaginateLog() 
        {
            LogModels = new ();
        }

        /// <summary>
        /// 分頁資料
        /// </summary>           
        public List<AccountantSignTemplateLogModel> LogModels { get; set; }
    }
}
