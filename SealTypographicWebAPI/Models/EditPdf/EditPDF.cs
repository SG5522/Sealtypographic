using SealTypographicWebAPI.Consts;


namespace SealTypographicWebAPI.Models.EditPdf
{
    /// <summary>
    /// 
    /// </summary>
    public class EditPDF
    {
        private string pdfPath;        

        /// <summary>
        /// 建置
        /// </summary>
        public EditPDF()
        {
            EditPages = new List<EditPage>();
        }

        /// <summary>
        /// PDF檔案路徑
        /// </summary>
        public string? PdfPath 
        {
            get => pdfPath;
            set
            {
                if(!string.IsNullOrWhiteSpace(value))
                {
                    pdfPath = value;
                    Bytes = File.ReadAllBytes(pdfPath);
                }
            }
        }

        /// <summary>
        /// PDFByes
        /// </summary>
        public byte[] Bytes { get; set; }

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
