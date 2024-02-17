namespace SealTypographicWebAPI.Consts
{
    /// <summary>
    /// 各Excel的標頭
    /// </summary>
    public class ExcelHearderConsts
    {
        /// <summary>
        /// 操作紀錄的標頭
        /// </summary>
        public static readonly List<string> OperationLogHeaders = new()
        {
            "使用者ID",
            "使用者名稱",
            "紀錄日期",
            "動作",
            "查詢對象",
            "印鑑季度/年度"
        };

        /// <summary>
        /// 財報紀錄的標頭
        /// </summary>
        public static readonly List<string> FinancialReportSealEventLogHeaders = new()
        {
            "編號",
            "名稱",
            "季度",
            "使用者ID",
            "使用者名稱",
            "日期",
            "事件"
        };

        /// <summary>
        /// 稅報紀錄的標頭
        /// </summary>
        public static readonly List<string> TaxReportSealEventLogHeaders = new()
        {
            "編號",
            "名稱",
            "年度",
            "使用者ID",
            "使用者名稱",
            "日期",
            "事件"
        };

        /// <summary>
        /// 會計師簽印紀錄的標頭
        /// </summary>
        public static readonly List<string> AccountantSignEventLogHeaders = new()
        {
            "編號",
            "姓名 ",            
            "使用者ID",
            "使用者名稱",
            "日期",
            "事件"
        };

        /// <summary>
        /// 財報排版紀錄的標頭
        /// </summary>
        public static readonly List<string> FinancialTypographicLogHeaders = new()
        {                        
            "使用者ID",
            "使用者名稱",
            "客戶編號",
            "客戶名稱",
            "紀錄日期",
            "檔案名稱",
            "排版頁數",
            "空白頁數"
        };

        /// <summary>
        /// 稅報排版紀錄的標頭
        /// </summary>
        public static readonly List<string> TaxTypographicLogHeaders = new()
        {
            "使用者ID",
            "使用者名稱",
            "客戶編號",
            "客戶名稱",
            "紀錄日期",
            "檔案名稱",
            "排版頁數"            
        };

        /// <summary>
        /// 會計師清單的標頭
        /// </summary>
        public static readonly List<string> AccountantHeaders = new()
        {
            "編號",
            "姓名",
            "群組"            
        };

        /// <summary>
        /// 使用者清單的標頭
        /// </summary>
        public static readonly List<string> UserHeaders = new()
        {
            "使用者ID",
            "使用者名稱",
            "建立日期",
            "密碼有效日期",
            "使用者群組",
        };
    }
}
