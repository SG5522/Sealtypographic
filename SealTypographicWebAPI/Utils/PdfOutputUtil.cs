using DBEntities;
using DBEntities.Consts;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// PDF輸出檔名
    /// </summary>
    public static class PdfOutputUtil
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

        /// <summary>
        /// 依照圖像類別決定縮放大小
        /// </summary>
        /// <param name="subSealType"></param>
        /// <returns></returns>
        public static float GetImageScale(SubSealType subSealType)
        {
            float scale = 0;
            switch(subSealType)
            {
                case SubSealType.Company:
                    scale = 0.24f;
                    break;
                case SubSealType.President:
                    scale = 0.24f;
                    break;
                case SubSealType.Manager:
                    scale = 0.24f;
                    break;
                case SubSealType.AccountingDirector:
                    scale = 0.24f;
                    break;
                case SubSealType.Seal:
                    scale = 0.24f;
                    break;
                case SubSealType.CHSign:
                    scale = 0.24f;
                    break;
                case SubSealType.ENSign:
                    scale = 0.24f;
                    break;
                case SubSealType.OldSign:
                    scale = 0.24f;
                    break;
                case SubSealType.Letterhead:
                    scale = 0.24f;
                    break;
                case SubSealType.TemporarySeal:
                    scale = 0.24f;
                    break;
                case SubSealType.Other:
                    scale = 0.24f;
                    break;
            }
            return scale;
        }
    }
}
