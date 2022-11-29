namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師分頁搜尋
    /// </summary>
    public class AccountantQueryPage
    {
        /// <summary>
        /// 會計師ID或名字或是群組名稱
        /// </summary>        
        public string? IdOrNameOrGroupsName { get; set; }

        /// <summary>
        /// 現在頁次(不得小於0)
        /// </summary>
        /// <example>1</example>
        public int PageNumber { get; set; }

        /// <summary>
        /// 單頁筆數(不得小於0)
        /// </summary>
        /// <example>5</example>
        public int PageSize { get; set; }
    }
}
