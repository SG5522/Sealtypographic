using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.LogReport
{
    /// <summary>
    /// 會計師成員列表
    /// </summary>
    public class AccountantMemberPaginate : PaginateViewModel
    {
        /// <summary>
        /// 會計師成員
        /// </summary>
        public List<AccountantMemberViewModel> ViewModels { get; set; }
    }
        
}
