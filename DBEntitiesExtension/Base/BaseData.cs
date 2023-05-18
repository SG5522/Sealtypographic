using DBEntitiesExtension.Consts;

namespace DBEntitiesExtension.Base
{
    /// <summary>
    /// 各類別基本資料 : 客戶印鑑歷程、會計師簽印歷程、事務所信頭圖片歷程、印鑑擺放位置、排版頁、基本資料包含名稱
    /// </summary>
    public abstract class BaseData
    {
        /// <summary>
        /// Id
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
        /// 建立此筆資料的User
        /// </summary>
        public int CreateUserId { get; set; }

        /// <summary>
        /// 更新此筆資料的User
        /// </summary>
        public int UpdateUserId { get; set; }

        /// <summary>
        /// 刪除狀態
        /// </summary>
        public DeleteStatus DeleteStatus { get; set; }
    }
}
