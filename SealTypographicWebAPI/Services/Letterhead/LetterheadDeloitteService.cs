using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Services.Letterhead
{
    /// <summary>
    /// 勤業用的顧客資料
    /// </summary>
    public class LetterheadDeloitteService : ILetterheadService
    {
        /// <summary>
        /// 取得顧客印鑑組
        /// </summary>
        /// <param name="litterheadID"></param>
        /// <returns></returns>
        public List<LetterheadImageWithId> GetLetterheadImages(int litterheadID)
        {
            List<LetterheadImageWithId> letterheadImages = new();
            for (int i = 0; i < 4; i++)
            {
                //測試資料
                LetterheadImageWithId letterheadImage = new()
                {
                    ID = i,
                    LetterheadID = litterheadID,
                    LetterheadImageGroup = i + 1,
                    ImagePath = "C://123.jpg",
                    CreateDate = DateOnly.FromDateTime(DateTime.Now),                    
                };
                letterheadImages.Add(letterheadImage);
            }
            return letterheadImages;
        }

        /// <summary>
        /// 取得信頭資料
        /// </summary>
        /// <param name="litterheadID">信頭ID</param>
        /// <returns></returns>
        public LetterheadWithId GetLetterheadData(int litterheadID)
        {
            //測試資料
            LetterheadWithId letterheadData = new()
            {
                ID = litterheadID,
                Name = "信頭"
            };
            return letterheadData;
        }
    }
}