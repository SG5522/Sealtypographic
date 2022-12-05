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
