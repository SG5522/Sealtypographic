using System.ComponentModel;

namespace DBEntities.Consts
{
    /// <summary>
    /// 客戶印鑑、會計師簽印的狀態
    /// 0 : 通過
    /// 10 : 草搞   
    /// 20 : 待審
    /// 30 : 退件
    /// 40 : 停用
    /// 50 : 作廢
    /// 60 : 不受理
    /// </summary>
    //[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ReviewStatus : sbyte
    {
        /// <summary>
        /// 通過(審核完成)(啟用)
        /// </summary>
        [Description("Approval")]
        Approval = 0,

        /// <summary>
        /// 草稿
        /// </summary>
        [Description("Draft")]
        Draft = 10,

        /// <summary>
        /// 待審
        /// </summary>
        [Description("Pending")]
        Pending = 20,

        /// <summary>
        /// 退件
        /// </summary>
        [Description("Reject")]
        Reject = 30,

        /// <summary>
        /// 停用
        /// </summary>
        [Description("Disabled")]
        Disabled = 40,

        /// <summary>
        /// 作廢
        /// </summary>
        [Description("Invalid")]
        Invalid = 50,

        /// <summary>
        /// 不受理(拒絕)
        /// 由主管發出的作廢處理。
        /// </summary>
        [Description("Refuse")]
        Refuse = 60,
    }

    /// <summary>
    /// 信頭圖片狀態
    /// </summary>
    //[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LetterheadImageStatus : sbyte
    {
        /// <summary>
        /// 啟用
        /// </summary>
        [Description("啟用")]
        Enable = 0,

        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        Disabled = 10,
    }

    /// <summary>
    /// 刪除狀態
    /// </summary>    
    public enum DeleteStatus : byte
    {
        /// <summary>
        /// 無標記
        /// </summary>
        No = 0,

        /// <summary>
        /// 隱藏或標記刪除
        /// </summary>
        Yes = 1,
    }

    /// <summary>
    /// 每頁資料上限
    /// </summary>
    public enum PageSizeLimit : byte
    {
        /// <summary>
        /// 最小值
        /// </summary>
        Min = 5,
        /// <summary>
        /// 最大值
        /// </summary>
        Max = 50
    }

    /// <summary>
    /// 啟用日期
    /// </summary>
    //[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Available : byte
    {
        /// <summary>
        /// 未啟用
        /// </summary>
        NotActivated = 0,
        /// <summary>
        /// 啟用
        /// </summary>
        Activated = 1,
    }

    /// <summary>
    /// 印鑑類型
    /// </summary>
    //[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SealType : byte
    {
        /// <summary>
        /// 客戶
        /// </summary>        
        [Description("Customer")]
        Customer = 1,

        /// <summary>
        /// 會計師
        /// </summary>
        [Description("Accountant")]
        Accountant = 2,

        /// <summary>
        /// 信頭
        /// </summary>
        [Description("Letterhead")]
        Letterhead = 3,

        /// <summary>
        /// 臨時章
        /// </summary>
        [Description("TemporarySeal")]
        TemporarySeal = 4,
    }

    /// <summary>
    /// 上傳檔案類別
    /// 1.客戶印鑑授權書 
    /// 2.會計印鑑簽名授權書 
    /// 3.信頭 
    /// 4.PDF 
    /// 5.會計師證明書 
    /// 6.臨時檔
    /// </summary>
    //[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UploadType : byte
    {
        /// <summary>
        /// 客戶授權書
        /// </summary>        
        [Description("Customer Seal Authorization")]
        CustomerSealAuthorization = 1,

        /// <summary>
        /// 會計師授權書
        /// </summary>
        [Description("Accountant Sign Authorization")]
        AccountantSignAuthorization = 2,

        /// <summary>
        /// 信頭圖片
        /// </summary>
        [Description("Letterhead Image")]
        LetterheadImage = 3,

        /// <summary>
        /// PDF檔(報表)
        /// </summary>
        [Description("PDF")]
        PDF = 4,

        /// <summary>
        /// 會計師證明書
        /// </summary>
        [Description("Accountant Sign Certificate")]
        AccountantSignCertificate = 5,

        /// <summary>
        /// 臨時檔
        /// </summary>
        [Description("Temporary")]
        Temporary = 6,
    }

    /// <summary>
    /// 客戶印鑑類別    
    /// 1.公司章
    /// 2.負責人
    /// 3.經理
    /// 4.會計主管    
    /// 5.其他(客戶)
    /// </summary>
    //[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CustomerSealType
    {
        /// <summary>
        /// 公司
        /// </summary>
        [Description("Company")]
        Company = 1,

        /// <summary>
        /// 負責人
        /// </summary>
        [Description("President")]
        President = 2,

        /// <summary>
        /// 經理
        /// </summary>
        [Description("Manager")]
        Manager = 3,

        /// <summary>
        /// 會計主管
        /// </summary>
        [Description("Accounting Director")]
        AccountingDirector = 4,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("Other")]
        Other = 5,
    }

    /// <summary>
    /// 會計師簽印類別
    /// 1.印鑑
    /// 2.中文簽名
    /// 3.英文簽名
    /// 4.舊式簽名
    /// 5.其他(會計)  
    /// </summary>
    //[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AccountantSignType
    {
        /// <summary>
        /// 印鑑
        /// </summary>
        [Description("Seal")]
        Seal = 1,

        /// <summary>
        /// 中文簽名
        /// </summary>
        [Description("CHSign")]
        CHSign = 2,

        /// <summary>
        /// 英文簽名
        /// </summary>
        [Description("ENSign")]
        ENSign = 3,

        /// <summary>
        /// 舊式簽名
        /// </summary>
        [Description("OldSign")]
        OldSign = 4,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("Other")]
        Other = 5,
    }

    /// <summary>
    /// 印鑑子類別
    /// </summary>
    public enum SubSealType
    {
        /// <summary>
        /// 公司
        /// </summary>
        [Description("Company")]
        Company = 11,

        /// <summary>
        /// 負責人
        /// </summary>
        [Description("President")]
        President = 12,

        /// <summary>
        /// 經理
        /// </summary>
        [Description("Manager")]
        Manager = 13,

        /// <summary>
        /// 會計主管
        /// </summary>
        [Description("Accounting Director")]
        AccountingDirector = 14,

        /// <summary>
        /// 印鑑(會計師)
        /// </summary>
        [Description("Seal")]
        Seal = 21,

        /// <summary>
        /// 中文簽名
        /// </summary>
        [Description("CHSign")]
        CHSign = 22,

        /// <summary>
        /// 英文簽名
        /// </summary>
        [Description("ENSign")]
        ENSign = 23,

        /// <summary>
        /// 舊式簽名
        /// </summary>
        [Description("OldSign")]
        OldSign = 24,

        /// <summary>
        /// 信頭
        /// </summary>
        [Description("Letterhead")]
        Letterhead = 30,

        /// <summary>
        /// 臨時章
        /// </summary>
        [Description("TemporarySeal")]
        TemporarySeal = 40,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("Other")]
        Other = 99,
    }

    /// <summary>
    /// 檔案工作狀態
    /// 在使用客戶印鑑、會計師簽印、信頭、PDF檔案
    /// 為了不在重複使用同一份檔案所做的狀態區分
    /// </summary>
    //[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FileWorkStatus
    {
        /// <summary>
        /// 未處理
        /// </summary>
        Unprocessed = 0,

        /// <summary>
        /// 已處理
        /// </summary>
        Done = 1
    }

    /// <summary>
    /// 上傳重複檔名處理模式
    /// 0.無重複
    /// 1.保留
    /// 2.覆蓋
    /// </summary>
    //[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DuplicateFileProcessMode
    {
        /// <summary>
        /// 無重複
        /// </summary>
        [Description("No Repeat")]
        NoRepeat = 0,

        /// <summary>
        /// 保留
        /// </summary>
        [Description("Reserve")]
        Reserve = 1,

        /// <summary>
        /// 覆蓋
        /// </summary>
        [Description("Overlay")]
        Overlay = 2,
    }

    /// <summary>
    /// 樣板(客戶、會計師)疊放方式
    /// </summary>
    public enum StackMode
    {
        /// <summary>
        /// 直式疊放
        /// </summary>
        [Description("直式疊放")]
        Vertical = 0,

        /// <summary>
        /// 橫式疊放
        /// </summary>
        [Description("橫式疊放")]
        Horizontal = 1,
    }
    /// <summary>
    /// 頁面尺吋
    /// </summary>
    public enum PageSize
    {
        /// <summary>
        /// 
        /// </summary>
        A3 = 1,

        /// <summary>
        /// 
        /// </summary>
        A4 = 2,
    }

    /// <summary>
    /// 頁面方向
    /// </summary>
    public enum PapeOrientation
    {
        /// <summary>
        /// 直式
        /// </summary>
        [Description("直式")]
        Portrait = 0,

        /// <summary>
        /// 橫式
        /// </summary>
        [Description("橫式")]
        Landscape = 1,
    }

    /// <summary>
    /// 上傳檔案類別
    /// 1.客戶印鑑樣板
    /// 2.會計師簽印樣板
    /// 3.信頭圖片樣板
    /// </summary>    
    public enum TemplateType : byte
    {
        /// <summary>
        /// 客戶印鑑樣板
        /// </summary>        
        [Description("客戶印鑑樣板")]
        CustomerSealTemplate = 1,

        /// <summary>
        /// 會計師簽印樣板
        /// </summary>
        [Description("會計師簽印樣板")]
        AccountantSignTemplate = 2,

        /// <summary>
        /// 信頭圖片樣板
        /// </summary>
        [Description("信頭圖片樣板")]
        LetterheadImageTemplate = 3,
    }
}
