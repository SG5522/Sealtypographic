using DBEntities.Consts;
using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Models.LogReport.OperationLog;

/// <summary>
/// 上傳類別
/// </summary>
public class ActionTypeViewModel
{
    /// <summary>
    /// 動作類別
    /// </summary>
    public ActionType ActionType { get; set; }

    /// <summary>
    /// 名稱
    /// </summary>
    public string? Name { get; set; }
}
