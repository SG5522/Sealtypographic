using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Service.Letterhead
{   
    /// <summary>
    /// 顧客資料處理的interface
    /// </summary>
    public interface ILetterhead
    {
        /// <summary>
        /// 取得顧客印鑑組
        /// </summary>
        /// <param name="litterheadID">顧客ID</param>         
        /// <returns></returns>
        List<LetterheadImageAddID> GetLetterheadImages(int litterheadID);

        /// <summary>
        /// 取得顧客基本資料
        /// </summary>
        /// <param name="litterheadID">顧客ID</param>
        /// <returns></returns>
        LetterheadDataAddID GetLetterheadData(int litterheadID);

    }
}
