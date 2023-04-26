using DBEntities.Consts;
using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.LetterheadTemplate
{
    /// <summary>
    /// 信頭樣板位置
    /// </summary> 
    [ModelBinder(BinderType = typeof(JsonModelBinder))]
    public class LetterheadImageTemplateLocationUpdateForm : BaseLocationViewModel
    {
        /// <summary>
        /// 樣板位置Id
        /// </summary>
        public int Id { get; set; }
    }

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
        /// 樣板位置座標
        /// </summary>                       
        public LetterheadImageTemplateLocationUpdateForm LocationUpdateForm { get; set; }
    }
}
