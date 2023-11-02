using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// 計算資料總頁數
    /// </summary>
    public class PageUtil
    {
        /// <summary>
        /// 分頁頁次處理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">繼承 PaginateViewModel的Class</param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        public static void SetPaginate<T> (T t, int pageNumber, int pageSize, int totalCount) where T : PaginateViewModel
        {
            t.PageNumber = pageNumber;
            t.PageSize = pageSize;            
            t.TotalCount = totalCount;
        }
    }
}
