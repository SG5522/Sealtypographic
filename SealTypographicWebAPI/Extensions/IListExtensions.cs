namespace SealTypographicWebAPI.Extensions
{
    /// <summary>
    /// IList擴充方法
    /// </summary>
    public static class IListExtensions
    {
        /// <summary>
        /// 新增List實體
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="items"></param>
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            foreach (T? item in items)
            {
                list.Add(item);
            }
        }
    }
}
