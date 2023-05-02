using DBEntities.Consts;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.LetterheadTemplate
{
    /// <summary>
    /// 信頭樣板位置
    /// </summary>     
    public class LetterheadImageTemplateLocationUpdateForm : BaseLocationViewModel
    {
        /// <summary>
        /// 樣板位置Id
        /// </summary>
        public int Id { get; set; }
    }
}
