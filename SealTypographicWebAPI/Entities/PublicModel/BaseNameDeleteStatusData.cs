using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Entities.PublicModel
{
    /// <summary>
    /// 各類別基本資料
    /// </summary>
    public class BaseNameDeleteStatusData : BaseNameData
    {
        /// <summary>
        /// 刪除狀態
        /// </summary>
        public DeleteStatus DeleteStatus { get; set; }
    }
}
