namespace SealTypographicWebAPI.Entities.BaseEntities
{
    /// <summary>
    /// 各類別基本資料 With 名稱 : 客戶資料表、會計師資料表、會計師群組、事務所信頭資料表、PDF排版資訊
    /// </summary>
    public class BaseNameData : BaseData
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }
    }
}
