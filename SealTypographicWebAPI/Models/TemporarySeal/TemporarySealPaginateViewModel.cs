using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Temporary
{
    /// <summary>
    /// 
    /// </summary>
    public class TemporaryViewModel : BaseName
    {
        /// <summary>
        /// 公司名稱(客戶名稱)
        /// </summary>
        /// <example>映像有限公司</example>   
        public string CustomerName { get; set; }
    }

    /// <summary>
    /// 依搜尋結果與分頁顯示臨時章列表
    /// </summary>
    public class TemporarySealPaginateViewModel : PaginateViewModel
    {
        /// <summary>
        /// New ViewModels
        /// </summary>
        public TemporarySealPaginateViewModel() 
        {
            ViewModels = new ();
        }
        /// <summary>
        /// 臨時章資料列表
        /// </summary>           
        public List<TemporaryViewModel> ViewModels { get; set; }
    }
}
