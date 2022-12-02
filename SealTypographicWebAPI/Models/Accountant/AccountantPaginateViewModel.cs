namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計資料
    /// </summary>
    public class AccountantPaginateViewModel
    {
        /// <summary>
        /// 會計師ID
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 會計師群組名稱
        /// </summary>
        public string AccountantGroupName { get; set; }

        /// <summary>
        /// 啟用日期
        /// </summary>
        /// <example>0001-01-01T00:00:00</example>
        public DateTime AvailableDate { get; set; }

        /// <summary>
        /// 會計師狀態字串顯示   
        /// 0.待審查
        /// 1.已審查
        /// 2.刪除(系統管理員可以看到資料)
        /// </summary>
        public string StatusString { get; set; }
    }
    /// <summary>
    /// 依搜尋結果顯示會計師列表
    /// </summary>
    public class AccountantPaginatesViewModel : PaginateViewModel
    {
        /// <summary>
        /// 會計師列表
        /// </summary>
        public List<AccountantPaginateViewModel> AccountantPaginates { get; set; }
    }
}
