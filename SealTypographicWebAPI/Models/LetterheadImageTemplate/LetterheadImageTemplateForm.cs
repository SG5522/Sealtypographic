using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.LetterheadTemplate
{
    /// <summary>
    /// 信頭樣板 (新增使用)
    /// </summary>
    public class LetterheadImageTemplateForm : BaseTemplateWithFile
    {
        /// <summary>
        /// 信頭樣板位置
        /// </summary>                       
        public LetterheadImageTemplateLocationForm LetterheadTemplateLocationForm { get; set; }
    }
}
