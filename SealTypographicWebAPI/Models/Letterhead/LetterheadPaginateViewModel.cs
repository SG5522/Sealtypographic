using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// /// 信頭資料列表
    /// </summary>
    public class LetterheadPaginateViewModel : PaginateViewModel
    {
        /// <summary>
        /// 信頭資料列表
        /// </summary>
        public List<LetterheadViewModel> ViewModels { get; set; }
    }
}
