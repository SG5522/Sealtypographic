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
        Customer = 1,

        /// <summary>
        /// 會計師
        /// </summary>
        Accountant = 2,

        /// <summary>
        /// 信頭
        /// </summary>
        Letterhead = 3,
    }

    /// <summary>
    /// 印鑑Or 簽印種類
    /// </summary>
    public enum SealMappingConfigType 
    {
        /// <summary>
        /// 公司章
        /// </summary>
        [Description("CompanySeal")]
        CompanySeal = 1,

        /// <summary>
        /// 負責人
        /// </summary>
        [Description("CeoSeal")]
        CeoSeal = 2,

        /// <summary>
        /// 經理
        /// </summary>
        [Description("ManagerSeal")]
        ManagerSeal = 3,

        /// <summary>
        /// 負責人
        /// </summary>
        [Description("AccountantDirectorSeal")]
        AccountantDirectorSeal = 4,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("CustomerOther")]
        CustomerOther = 5,



        /// <summary>
        /// 會計師印鑑
        /// </summary>
        [Description("AccountantSeal")]
        AccountantSeal = 6,

        /// <summary>
        /// 中文簽名
        /// </summary>
        [Description("AccountantCHSign")]
        AccountantCHSign = 7,

        /// <summary>
        /// 英文簽名
        /// </summary>
        [Description("AccountantENSign")]
        AccountantENSign = 8,

        /// <summary>
        /// 舊式簽名
        /// </summary>
        [Description("AccountantOldSign")]
        AccountantOldSign = 9,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("AccountantOther")]
        AccountantOther = 10,
    }
}
