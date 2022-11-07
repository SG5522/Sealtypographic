using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Service.Letterhead
{
    /// <summary>
    /// 顧客資料處理
    /// </summary>
    public class Letterhead
    {
        /// <summary>
        /// 宣告顧客資料處理的interface
        /// </summary>
        public readonly ILetterhead _letterhead;

        /// <summary>
        /// 注入顧客interface
        /// </summary>
        /// <param name="letterhead"></param>
        public Letterhead(ILetterhead letterhead)
        {
            _letterhead = letterhead;
        }

        /// <summary>
        /// 取得信頭圖像
        /// </summary>
        /// <param name="litterheadID">信頭ID</param>
        /// <returns></returns>
        public List<LetterheadImageAddID> GetLetterheadImages(int litterheadID)
        {
            return _letterhead.GetLetterheadImages(litterheadID);
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="litterheadID">信頭ID</param>        
        /// <returns></returns>
        public LetterheadDataAddID GetLetterheadData(int litterheadID)
        {
            return _letterhead.GetLetterheadData(litterheadID);
        }
    }
}
