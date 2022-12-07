using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 各類別基本資料
    /// </summary>
    public class BaseData
    {
        /// <summary>
        /// ID
        /// </summary>        
        public int Id { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 創建User
        /// </summary>
        public int CreateUserId { get; set; }

        /// <summary>
        /// 更新User
        /// </summary>
        public int UpdateUserId { get; set; }

    }
}
