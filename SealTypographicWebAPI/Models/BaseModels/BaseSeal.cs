namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 印鑑
    /// </summary>
    public class BaseSeal : BaseData
    {
        /// <summary>
        /// 印鑑簽印ID
        /// </summary>
        public int SealMappingConfigId { get; set; }

        /// <summary>         
        /// (目前暫定)
        /// 1.公司章
        /// 2.負責人
        /// 3.經理
        /// 4.會計主管    
        /// 5.其他(客戶)
        /// 6.會計印鑑
        /// 7.中文簽名
        /// 8.英文簽名
        /// 9.舊式簽名
        /// 10.其他(會計)
        /// 11.信頭LOGO
        /// 12.信頭地址
        /// </summary>
        /// <example>1</example>
        public string SealMappingConfigName { get; set; }

        /// <summary>
        /// SubId
        /// </summary>
        public string SealMappingConfigSubId { get; set; }


        /// <summary>
        /// 圖檔字串(Base64)
        /// </summary>
        /// <example>image/...</example>
        public string ImageBase64 { get; set; }
    }
}
