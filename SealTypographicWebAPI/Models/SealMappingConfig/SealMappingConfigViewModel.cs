using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.PublicModel;

namespace SealTypographicWebAPI.Models.SealMappingConfig
{
    /// <summary>
    /// 圖片群組 (使用印鑑、簽名、LOGO)
    /// </summary>
    public class SealMappingConfigViewModel : BaseData
    {
        /// <summary>
        /// Type(customer、accountant、letterhead)
        /// </summary>
        /// <example>customer</example>
        public SealType SealType { get; set; }

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
        /// 5.其他
        /// 6.會計印鑑
        /// 7.中文簽名
        /// 8.英文簽名
        /// 9.舊式簽名   
        /// 10.其他
        /// 11.信頭
        /// </summary>
        /// <example>公司章</example>
        public string Name { get; set; }
    }
}
