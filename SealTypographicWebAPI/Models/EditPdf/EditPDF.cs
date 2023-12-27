using SealTypographicWebAPI.Consts;


namespace SealTypographicWebAPI.Models.EditPdf
{
    /// <summary>
    /// 
    /// </summary>
    public class EditPDF
    {
        /// <summary>
        /// 
        /// </summary>
        public EditPDF()
        {
            EditPages = new List<EditPage>();
        }

        /// <summary>
        /// PDF輸出顏色
        /// </summary>
        public PDFColor PDFColor { get; set; }

        /// <summary>
        /// 是否空白頁
        /// </summary>
        public bool IsBlank { get; set; }

        /// <summary>
        /// 所有頁次編輯內容
        /// </summary>
        public List<EditPage> EditPages { get; set; }
    }
}
