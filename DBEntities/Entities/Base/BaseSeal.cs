namespace DBEntities.Entities.Base
{
    /// <summary>
    /// 各項印鑑(簽名)歷程基本資料
    /// </summary>
    public abstract class BaseSeal : BaseData
    {
        /// <summary>
        /// 圖檔路徑
        /// </summary>
        public string ImageFullPath { get; set; }

        /// <summary>
        /// 縮圖路徑
        /// </summary>
        public string? ThumbnailFullPath { get; set; }
    }
}
