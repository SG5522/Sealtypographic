using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using System.ComponentModel;

namespace SealTypographicWebAPI.Consts
{
    /// <summary>
    /// 客戶、會計師、印鑑、簽名的狀態
    /// </summary>
    public enum ReviewStatus : sbyte
    {
        /// <summary>
        /// 全部
        /// </summary>
        All = -1,

        /// <summary>
        /// 通過(審核完成)(啟用)
        /// </summary>
        [Description("通過")]
        Approval = 0,

        /// <summary>
        /// 啟用
        /// </summary>
        [Description("啟用")]
        Activated = 1,

        /// <summary>
        /// 未啟用
        /// </summary>
        [Description("未啟用")]
        NotActivated = 2,

        /// <summary>
        /// 退件
        /// </summary>
        [Description("退件")]
        Reject = 10,

        /// <summary>
        /// 草稿
        /// </summary>
        [Description("草稿")]
        Draft = 20,

        /// <summary>
        /// 待審
        /// </summary>
        [Description("待審")]
        Pending = 30,

        /// <summary>
        /// 作廢
        /// </summary>
        [Description("作廢")]
        Invalid = 40,
    }

    /// <summary>
    /// 刪除狀態
    /// </summary>
    public enum DeleteStatus : byte
    {
        /// <summary>
        /// 無標記
        /// </summary>
        NO = 0,

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
        Max = 20
    }

    /// <summary>
    /// 啟用日期
    /// </summary>
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
    }

    /// <summary>
    /// 客戶印鑑類別
    /// </summary>
    public enum CustomerSealConfigType
    {
        /// <summary>
        /// 公司章
        /// </summary>
        [Description("CompanySeal")]
        //[Description("公司章")]
        CompanySeal = 1,

        /// <summary>
        /// 負責人
        /// </summary>
        [Description("CeoSeal")]
        //[Description("負責人")]
        CeoSeal = 2,

        /// <summary>
        /// 經理
        /// </summary>
        [Description("ManagerSeal")]
       //[Description("經理")]
        ManagerSeal = 3,

        /// <summary>
        /// 會計主管
        /// </summary>
        [Description("AccountantDirectorSeal")]
        //[Description("會計主管")]
        AccountantDirectorSeal = 4,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("CustomerOther")]
        //[Description("其他")]
        CustomerOther = 5,
    }

    /// <summary>
    /// 會計師簽印類別
    /// </summary>
    public enum AccountantSignConfigType
    {
        /// <summary>
        /// 會計師印鑑
        /// </summary>
        [Description("AccountantSeal")]
        //[Description("會計師印鑑")]
        AccountantSeal = 1,

        /// <summary>
        /// 中文簽名
        /// </summary>
        [Description("AccountantCHSign")]
        //[Description("中文簽名")]
        AccountantCHSign = 2,

        /// <summary>
        /// 英文簽名
        /// </summary>
        [Description("AccountantENSign")]
        ///[Description("英文簽名")]
        AccountantENSign = 3,

        /// <summary>
        /// 舊式簽名
        /// </summary>
        [Description("AccountantOldSign")]
        //[Description("舊式簽名")]
        AccountantOldSign = 4,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("AccountantOther")]
        //[Description("其他")]
        AccountantOther = 5,

        /// <summary>
        /// 會計師印鑑
        /// </summary>
        //[Description("AccountantSeal")]
        [Description("會計師印鑑")]
        test1 = 6,

        /// <summary>
        /// 中文簽名
        /// </summary>
        //[Description("AccountantCHSign")]
        [Description("中文簽名")]
        test2 = 7,

        /// <summary>
        /// 英文簽名
        /// </summary>
        //[Description("AccountantENSign")]
        [Description("英文簽名")]
        test3 = 8,

        /// <summary>
        /// 舊式簽名
        /// </summary>
        //[Description("AccountantOldSign")]
        [Description("舊式簽名")]
        test4 = 9,

        /// <summary>
        /// 其他
        /// </summary>
        //[Description("AccountantOther")]
        [Description("其他")]
        test5 = 10,
    }
}
