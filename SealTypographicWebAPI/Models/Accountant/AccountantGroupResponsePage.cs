namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 依搜尋結果顯示會計師列表
    /// </summary>
    public class AccountantGroupResponsePage : Response
    {
        /// <summary>
        /// 現在頁數
        /// </summary>
        public int ThisPage { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// 資料筆數
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 會計師列表
        /// </summary>
        public List<AccountantGroupData> AccountantGroups { get; set; }
    }
}
