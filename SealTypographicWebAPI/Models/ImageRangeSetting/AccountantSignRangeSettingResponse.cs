namespace SealTypographicWebAPI.Models.ImageRangeSetting
{
    /// <summary>
    /// 客戶印鑑分離截取設定
    /// </summary>
    public class AccountantSignRangeSettingResponse : ResponseViewModel
    {
        /// <summary>
        /// 客戶印鑑截取設定
        /// </summary>
        public AccountantSignRangeSetting AccountantSignRangeSetting { get; set; }        
    }
}
