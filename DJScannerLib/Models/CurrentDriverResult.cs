namespace DJScannerLib.Models
{
    /// <summary>
    /// 預設Driver結果
    /// </summary>
    public class CurrentDriverResult : BaseResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentDriverResult"/> class.
        /// </summary>
        public CurrentDriverResult() 
        {
            Success = false;
        }

        /// <summary>
        /// 預設驅動程式
        /// </summary>
        public string Default { get; set; }
    }
}
