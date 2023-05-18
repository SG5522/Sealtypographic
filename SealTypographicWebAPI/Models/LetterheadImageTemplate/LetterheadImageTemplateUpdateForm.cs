using DBEntities.Consts;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Models.AccountantSignTemplate;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.LetterheadImageTemplate
{
    /// <summary>
    /// 信頭樣板 (更新使用)
    /// </summary>
    public class LetterheadImageTemplateUpdateForm : BaseTemplateWithFile
    {
        /// <summary>
        /// 樣板Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 修改樣板位置座標
        /// </summary>                       
        public LetterheadImageTemplateLocationUpdateForm LocationUpdateForm { get; set; }
    }
}
