using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.TemporarySeal
{
    /// <summary>
    /// 
    /// </summary>
    public class TypographicTemporaryViewModel : BaseData
    {
        /// <summary>
        /// 季度
        /// </summary>
        public string Quarter { get; set; }
    }

    /// <summary>
    /// 依搜尋結果與分頁顯示臨時章列表
    /// </summary>
    public class TypographicTemporarySealPaginateViewModel : PaginateViewModel
    {
        /// <summary>
        /// New ViewModels
        /// </summary>
        public TypographicTemporarySealPaginateViewModel() 
        {
            ViewModels = new ();
        }

        /// <summary>
        /// 臨時章資料列表
        /// </summary>           
        public List<TypographicTemporaryViewModel> ViewModels { get; set; }
    }
}
