using SealTypographicWebAPI.Models.Customer;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// 更新信頭圖片組資料
    /// </summary>
    public class LetterheadImageUpdate
    {
        /// <summary>
        /// 客戶Id
        /// </summary>        
        [Required]
        public int LetterheadId { get; set; }


        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime GroupCreateDate { get; set; }

        /// <summary>
        /// 刪除客戶印鑑列表(ID)
        /// </summary>
        public List<int> DeleteLetterheadImageIds { get; set; }

        /// <summary>
        /// 更新客戶印鑑列表
        /// </summary>
        public List<LetterheadImageFormUpdate> UpdateLetterheadImages { get; set; }

        /// <summary>
        /// 新增客戶印鑑列表
        /// </summary>
        public List<LetterheadImageForm> CreateLetterheadImages { get; set; }
    }
}
