
namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師簽印是否重複建立確認用
    /// </summary>
    public class AccountantSignCheck
    {
        /// <summary>
        /// 客戶ID
        /// </summary>
        public int AccountantId { get; set; }

        /// <summary>
        /// 客戶印鑑群組ID 
        /// 6.會計印鑑
        /// 7.中文簽名
        /// 8.英文簽名
        /// 9.舊式簽名    
        /// 10.其他(會計)
        /// </summary>
        public int SealMappingConfigId { get; set; }

        /// <summary>
        /// 啟用日期
        /// </summary>
        public DateTime GroupCreateDate { get; set; }
    }
}
