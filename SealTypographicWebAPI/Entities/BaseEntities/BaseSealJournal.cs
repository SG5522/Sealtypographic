namespace SealTypographicWebAPI.Entities.BaseEntities
{
    /// <summary>
    /// 各項印鑑(簽名)歷程
    /// </summary>
    public class BaseSealJournal : BaseData
    {
        /// <summary>
        /// 圖檔路徑
        /// </summary>
        public string ImagePath { get; set; }
    }
}
