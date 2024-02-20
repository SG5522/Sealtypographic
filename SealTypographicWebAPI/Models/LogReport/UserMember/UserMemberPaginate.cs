using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.LogReport.UserMember
{
    /// <summary>
    /// 客戶財稅報排版紀錄列表
    /// </summary>
    public class UserMemberPaginate : PaginateViewModel
    {
        /// <summary>
        /// 客戶財稅報排版紀錄
        /// </summary>
        public List<UserMemberViewModel> ViewModels { get; set; }
    }
}
