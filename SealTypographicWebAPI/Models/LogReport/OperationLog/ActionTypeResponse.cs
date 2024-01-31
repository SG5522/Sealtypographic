namespace SealTypographicWebAPI.Models.LogReport.OperationLog
{
    /// <summary>
    /// 取得上傳類別並回應訊息
    /// </summary>
    public class ActionTypeResponse : ResponseViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public ActionTypeResponse()
        {
            ViewModels = new();
        }

        /// <summary>
        /// 上傳類別
        /// </summary>
        public List<ActionTypeViewModel> ViewModels { get; set; }
    }
}
