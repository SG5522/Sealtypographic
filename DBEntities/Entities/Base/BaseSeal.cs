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
        /// 圖片加密key (部份圖檔無需加密)
        /// </summary>
        public string? ImageEncryptKey { get; set; }

        /// <summary>
        /// 縮圖路徑
        /// </summary>
        public string? ThumbnailFullPath { get; set; }

        /// <summary>
        /// 縮圖加密key (部份圖檔無需加密)
        /// </summary>
        public string? ThumbnailEncryptKey { get; set; }
    }
}
