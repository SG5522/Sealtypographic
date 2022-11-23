namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 圖片群組 (使用印鑑、簽名、LOGO)
    /// </summary>
    public class SealMappingConfigViewModel
    {
        /// <summary>
        /// Type(customer、accountant、letterhead)
        /// </summary>
        /// <example>customer</example>
        public string Type { get; set; }

        /// <summary>
        /// 群組ID
        /// </summary>
        /// <example></example>
        public string SubId { get; set; }

        /// <summary>
        /// 群組名稱 (預設)
        /// 1.公司章
        /// 2.負責人
        /// 3.經理
        /// 4.會計主管   
        /// 5.會計印鑑
        /// 6.中文簽名
        /// 7.英文簽名
        /// 8.舊式簽名   
        /// 9.信頭
        /// </summary>
        /// <example>公司章</example>
        public string Name { get; set; }
    }
}
