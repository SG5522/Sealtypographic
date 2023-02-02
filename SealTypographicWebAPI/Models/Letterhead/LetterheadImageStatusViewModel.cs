using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 信頭圖像狀態顯示
    /// </summary>
    public class LetterheadImageStatusViewModel : BaseData
    {
        /// <summary>
        /// 信頭狀態名稱
        /// </summary>        
        public string? Name { get; set; }
    }

    /// <summary>
    /// 信頭圖片狀態列表
    /// </summary>
    public class LetterheadImageStatusResponse : ResponseViewModel
    {
        /// <summary>
        /// new LetterheadImageStatusViewModel
        /// </summary>
        public LetterheadImageStatusResponse()
        {
            ViewModels = new();
        }

        /// <summary>
        /// 信頭圖像狀態顯示
        /// </summary>
        public List<LetterheadImageStatusViewModel> ViewModels { get; set; }
    }
}
