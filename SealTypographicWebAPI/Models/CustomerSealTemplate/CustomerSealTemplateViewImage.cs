using DBEntities.Consts;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.CustomerSealTemplate
{
    /// <summary>
    /// 客戶印鑑樣板圖片顯示
    /// </summary>     
    public class CustomerSealTemplateViewImage : ResponseViewModel
    {
        /// <summary>
        /// 圖像
        /// </summary>
        public string ImageBase64 { get; set; }
    }

}
