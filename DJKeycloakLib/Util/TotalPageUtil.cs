namespace DJKeycloakLib.Util
{
    /// <summary>
    /// 計算資料總頁數
    /// </summary>
    public class TotalPageUtil
    {
        /// <summary>
        /// 取得總頁數
        /// </summary>
        /// <returns></returns>
        public static int GetTotalPage(int count, int pagesize)
        {
            return count / pagesize + (count % pagesize == 0 ? 0 : 1);
        }
    }
}
