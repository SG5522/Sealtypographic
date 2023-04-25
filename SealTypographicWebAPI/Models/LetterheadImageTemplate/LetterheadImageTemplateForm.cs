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
    public class LetterheadTemplateLocationForm : BaseLocationViewModel
    {

    }

    /// <summary>
    /// 信頭樣板 (新增使用)
    /// </summary>
    public class LetterheadImageTemplateForm : BaseTemplateWithFile
    {
        /// <summary>
        /// 信頭樣板位置
        /// </summary>                       
        public LetterheadTemplateLocationForm LetterheadTemplateLocationForm { get; set; }
    }
}
