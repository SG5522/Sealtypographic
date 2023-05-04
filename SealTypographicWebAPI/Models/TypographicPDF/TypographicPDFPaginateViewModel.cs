using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.TypographicPDF
{

    /// <summary>
    /// 排板分頁
    /// </summary>
    public class TypographicPDFPaginateViewModel : PaginateViewModel
    {
        /// <summary>
        /// New ViewModels
        /// </summary>
        public TypographicPDFPaginateViewModel() 
        {
            ViewModels = new();
        }

        /// <summary>
        /// 排版PDF資訊
        /// </summary>
        public List<TypographicPDFViewModel> ViewModels { get; set; }
    }
}
