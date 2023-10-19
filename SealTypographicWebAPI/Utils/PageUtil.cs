using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// 計算資料總頁數
    /// </summary>
    public class PageUtil
    {
        /// <summary>
        /// 取得總頁數
        /// </summary>
        /// <returns></returns>
        public static int GetTotalPage(int count,int pageSize)
        {
            return count / pageSize + (count % pageSize == 0 ? 0 : 1);
        }

        /// <summary>
        /// 分頁頁次處理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">繼承 PaginateViewModel的Class</param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        public static void GetPageData<T> (T t, int pageNumber, int pageSize, int totalCount) where T : PaginateViewModel
        {
            t.PageNumber = pageNumber;
            t.PageSize = pageSize;
            t.TotalPage = GetTotalPage(totalCount, pageSize);
            t.TotalCount = totalCount;
        }
    }
}
