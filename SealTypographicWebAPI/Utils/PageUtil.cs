using AutoMapper.QueryableExtensions;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// 計算資料總頁數
    /// </summary>
    public class PageUtil
    {
        /// <summary>
        /// 設定分頁基本資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">繼承 PaginateViewModel的Class</param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        public static void SetPaginate<T>(T t, int pageNumber, int pageSize, int totalCount) where T : PaginateViewModel
        {
            t.PageNumber = pageNumber;
            t.PageSize = pageSize;
            t.TotalCount = totalCount;
        }

        /// <summary>
        /// 設定分頁顯示內容
        /// </summary>
        /// <typeparam name="TSoucre">來源</typeparam>
        /// <typeparam name="TDestination">目標</typeparam>
        /// <param name="srcData">來源的IQueryable</param>
        /// <param name="pageNumber">分頁頁次</param>
        /// <param name="pageSize">每頁顯示的個數</param>
        /// <param name="configurationProvider">AutoMapper配置提供者</param>
        /// <returns></returns>
        public static List<TDestination> SetPaginateViewModel<TSoucre, TDestination>(IQueryable<TSoucre> srcData, int pageNumber, int pageSize, AutoMapper.IConfigurationProvider configurationProvider)                
        {            
            return  srcData
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ProjectTo<TDestination>(configurationProvider)
                    .ToList();            
        }
    }
}
