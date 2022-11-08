using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Services.Letterhead
{
    /// <summary>
    /// 顧客資料處理的interface
    /// </summary>
    public interface ILetterheadService
    {
        /// <summary>
        /// 取得顧客印鑑組
        /// </summary>
        /// <param name="litterheadID">顧客ID</param>         
        /// <returns></returns>
        List<LetterheadImageWithId> GetLetterheadImages(int litterheadID);

        /// <summary>
        /// 取得顧客基本資料
        /// </summary>
        /// <param name="litterheadID">顧客ID</param>
        /// <returns></returns>
        LetterheadWithId GetLetterheadData(int litterheadID);

    }
}
