using DBEntities.Consts;
using DBEntities.Entities.AccountantModels;
using DBEntities.Entities.Base;
using DBEntities.Entities.CustomerModels;
using DBEntities.Entities.TemplateModels;

namespace DBEntities.Entities.TypographicModels
{
    /// <summary>
    /// 排版資源
    /// </summary>
    public class TypographicResource : BaseSeal
    {
        /// <summary>
        /// 印鑑編號(排序)
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 印鑑配置類別
        /// </summary>
        public SealType SealType { get; set; }

        /// <summary>
        /// 印鑑子類別
        /// </summary>
        public SubSealType SubSealType { get; set; }

        /// <summary>
        /// 影像處理後圖檔路徑
        /// TODO: 目前暫時跟原圖路徑一致
        /// TODO: 保留以防需要原圖與影像處理的圖像時能做分別儲存的動作
        /// </summary>
        public string? ImageProcessingFullPath { get; set; }

        /// <summary>
        /// 客戶印鑑季度資料表
        /// </summary>
        public CustomerSealGroup? CustomerSealGroup { get; set; }

        /// <summary>
        /// 會計師簽印群組
        /// </summary>
        public AccountantSignGroup? AccountantSignGroup { get; set; }

        /// <summary>
        /// 事務所信頭
        /// </summary>
        public Letterhead? Letterhead { get; set; }

        /// <summary>
        /// 臨時章群組
        /// </summary>
        public TemporarySealGroup? TemporarySealGroup { get; set; }

        /// <summary>
        /// 上傳檔案資料表
        /// </summary>
        public UploadFile? UploadFile { get; set; }

        /// <summary>
        /// 各印鑑簽印排版位置
        /// </summary>
        public IList<TypographicResourceLocation> TypographicResourceLocations { get; set; }

    }
}
