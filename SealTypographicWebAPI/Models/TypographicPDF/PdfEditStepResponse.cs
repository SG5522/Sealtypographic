namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 排版步驟資料回傳
    /// </summary>
    public class PdfEditStepResponse : ResponseViewModel
    {
        /// <summary>
        /// 建置
        /// </summary>
        public PdfEditStepResponse() 
        {
            ViewModels = new ();
        }

        /// <summary>
        /// 排版步驟資料
        /// </summary>
        public List<PdfEditStepViewModel> ViewModels { get; set; }
    }
}
