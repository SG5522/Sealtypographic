using Spire.Pdf.Graphics;
using System.Collections.Generic;


namespace DJSpire.Models
{
    public class EditPDF
    {
        public EditPDF() 
        {
            EditPages = new List<EditPage>();
        }

        /// <summary>
        /// PDF輸出顏色
        /// </summary>
        public PdfColorSpace PdfColorSpace { get; set; }

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
