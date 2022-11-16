namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師群組分頁搜尋
    /// </summary>
    public class AccountantGroupQueryPage
    {
        /// <summary>
        /// 會計師群組ID或是群組名稱
        /// </summary>
        public string? IdOrGroupsName { get; set; }

        /// <summary>
        /// 現在頁次(不得小於0)
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 單頁筆數(不得小於0)
        /// </summary>
        public int PageSize { get; set; }
    }
}
