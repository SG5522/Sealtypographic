namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 圖片群組 (使用印鑑、簽名、LOGO)
    /// </summary>
    public class ImageGroupViewModel
    {
        /// <summary>
        /// 群組ID
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

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

        /// <summary>
        /// Type(customer、accountant、letterhead)
        /// </summary>
        /// <example>customer</example>
        public string Type { get; set; }

        /// <summary>
        /// 說明
        /// </summary>
        /// <example>des...</example>
        public string Description { get; set; }
    }
}
