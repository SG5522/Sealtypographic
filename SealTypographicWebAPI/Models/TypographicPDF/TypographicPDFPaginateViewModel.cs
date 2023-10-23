using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.TypographicPDF
{
    /// <summary>
    /// 排版PDF資訊
    /// </summary>
    public class TypographicPDFViewModel : BaseData
    {
        /// <summary>
        /// 原始檔名
        /// </summary>
        public string OriginFileName { get; set; }

        /// <summary>
        /// 客戶編號
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// 客戶名稱
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 季度        
        /// </summary>
        public int QuarterYearId { get; set; }

        /// <summary>
        /// 建檔狀態
        /// (此欄位未來有審核時功能時會改為審核狀態)
        /// 0.通過
        /// 10.編輯中
        /// </summary>
        public ReviewStatus ReviewStatus { get; set; }
    }

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
