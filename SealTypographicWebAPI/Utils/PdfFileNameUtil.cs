using DBEntities;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// PDF輸出檔名
    /// </summary>
    public static class PdfFileNameUtil
    {
        /// <summary>
        /// 取得輸出PDF預設之檔名 (目前依勤業為主)
        /// </summary>
        /// <param name="code">客戶編號</param>
        /// <param name="quarter">季度</param>
        /// <returns></returns>
        public static string GetName(string code, Quarter quarter)
        {            
            return $"{code}{"A4"}{QuarterUtil.GetTaiwanYearQuarter(quarter)}";
        }
    }
}
