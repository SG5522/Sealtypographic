using DBEntities;
using DBEntities.Consts;
using Keycloak.AuthServices.Sdk.Admin.Models;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// PDF輸出檔名
    /// </summary>
    public static class PdfOutputUtil
    {
        private const float CompanyScale = 0.24f;
        private const float PresidentScale = 0.24f;
        private const float ManagerScale = 0.24f;
        private const float AccountingDirectorScale = 0.24f;
        private const float SealScale = 0.24f;
        private const float CHSignScale = 0.24f;
        private const float ENSignScale = 0.24f;
        private const float OldSignScale = 0.24f;
        private const float LetterheadScale = 0.24f;
        private const float TemporarySealScale = 0.24f;
        private const float OtherScale = 0.24f;

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
                    scale = CompanyScale;
                    break;
                case SubSealType.President:
                    scale = PresidentScale;
                    break;
                case SubSealType.Manager:
                    scale = ManagerScale;
                    break;
                case SubSealType.AccountingDirector:
                    scale = AccountingDirectorScale;
                    break;
                case SubSealType.Seal:
                    scale = SealScale;
                    break;
                case SubSealType.CHSign:
                    scale = CHSignScale;
                    break;
                case SubSealType.ENSign:
                    scale = ENSignScale;
                    break;
                case SubSealType.OldSign:
                    scale = OldSignScale;
                    break;
                case SubSealType.Letterhead:
                    scale = LetterheadScale;
                    break;
                case SubSealType.TemporarySeal:
                    scale = TemporarySealScale;
                    break;
                case SubSealType.Other:
                    scale = OtherScale;
                    break;
            }
            return scale;
        }
    }
}
