namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 排版步驟
    /// </summary>
    public class PdfEditStepViewModel
    {
        /// <summary>
        /// Enum參數
        /// </summary>
        public int Id { set; get; } 

        /// <summary>
        /// 步驟名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 步驟說明
        /// </summary>
        public string Description { get; set; }
    }
}
