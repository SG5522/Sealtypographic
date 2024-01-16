using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.LogReport.OperationLog
{
    /// <summary>
    /// 操作紀錄列表
    /// </summary>
    public class OperationLogPaginate : PaginateViewModel
    {
        /// <summary>
        /// 列表內容
        /// </summary>
        public List<OperationLogViewModel> ViewModels { get; set; }
    }

}
