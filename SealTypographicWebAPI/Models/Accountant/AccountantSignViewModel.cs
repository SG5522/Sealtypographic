namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師印鑑簽名
    /// </summary>
    public class AccountantSignViewModel
    {
        /// <summary>
        /// 會計師ID
        /// </summary>
        public string AccountantID { get; set; } = null!;

        /// <summary>
        /// 啟用日(審查通過才有)
        /// </summary>
        public DateTime AvailableDate { get; set; }

        /// <summary>
        /// 會計師簽名群組
        /// 1.印鑑
        /// 2.中文簽名
        /// 3.英文簽名
        /// 4.舊式簽名(英文)        
        /// </summary>
        public int AccountantSignGroupID { get; set; }

        /// <summary>
        /// 會計印鑑群組名稱
        /// </summary>        
        public string SealMappingConfigName { get; set; }

        /// <summary>
        /// 圖檔
        /// </summary>
        public string ImageBase64 { get; set; }

    }
    /// <summary>
    /// 會計師印鑑簽名組
    /// </summary>
    public class AccountantSignViewModels : ResponseViewModel
    {
        /// <summary>
        /// 會計師印鑑簽名組
        /// </summary>
        public List<AccountantSignViewModel> SignViewModels { get; set; }
    }
}
