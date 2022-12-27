using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// 信頭圖組
    /// </summary>
    public class LetterheadImageViewModel : BaseSeal
    {
        /// <summary>
        /// 印鑑編號(排序) 1為起始
        /// </summary>
        /// <example>1</example>
        public int Sequence { get; set; }
    }

    /// <summary>
    /// 信頭圖片組
    /// </summary>
    public class LetterheadImageViewModels: ResponseViewModel
    {
        /// <summary>
        /// 信頭ID
        /// </summary>
        public int LetterheadID { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime GroupCreateDate { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        public ReviewStatus ReviewStatus { get; set; }

        /// <summary>
        /// 信頭圖片組
        /// </summary>
        public List<LetterheadImageViewModel> LetterheadImageViewModel { get; set; }
    }
}
