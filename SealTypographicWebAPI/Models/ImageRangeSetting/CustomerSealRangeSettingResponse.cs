namespace SealTypographicWebAPI.Models.ImageRangeSetting
{
    /// <summary>
    /// 客戶印鑑分離截取設定
    /// </summary>
    public class CustomerSealRangeSettingResponse : ResponseViewModel
    {
        /// <summary>
        /// 客戶印鑑截取設定
        /// </summary>
        public CustomerSealRangeSetting CustomerSealRangeSetting { get; set; }        
    }
}
