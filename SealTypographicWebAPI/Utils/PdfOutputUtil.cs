using DBEntities.Consts;
using DBEntities.Entities;
using SealTypographicWebAPI.Consts;

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
        public static string GetName(string code, QuarterYear quarter)
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
                    scale = PDFImageScaleConsts.CompanySeal;
                    break;
                case SubSealType.President:
                    scale = PDFImageScaleConsts.PresidentSeal;
                    break;
                case SubSealType.Manager:
                    scale = PDFImageScaleConsts.ManagerSeal;
                    break;
                case SubSealType.AccountingDirector:
                    scale = PDFImageScaleConsts.AccountingDirectorSeal;
                    break;
                case SubSealType.Seal:
                    scale = PDFImageScaleConsts.AccountingSeal;
                    break;
                case SubSealType.CHSign:
                    scale = PDFImageScaleConsts.CHSign;
                    break;
                case SubSealType.ENSign:
                    scale = PDFImageScaleConsts.ENSign;
                    break;
                case SubSealType.OldSign:
                    scale = PDFImageScaleConsts.OldSign;
                    break;
                case SubSealType.Letterhead:
                    scale = PDFImageScaleConsts.LetterheadImage;
                    break;
                case SubSealType.TemporarySeal:
                    scale = PDFImageScaleConsts.TemporarySeal;
                    break;
                case SubSealType.Other:
                    scale = PDFImageScaleConsts.Other;
                    break;
            }
            return scale;
        }
    }
}
