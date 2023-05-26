namespace DJScannerLib.Models
{
    /// <summary>
    /// 取得所有 TWAIN Drivers 結果
    /// </summary>
    public class AllDriversResult : BaseResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllDriversResult"/> class.
        /// </summary>
        public AllDriversResult() 
        {
            Success = false;
            DriverNames = new List<string>();
        }

        /// <summary>
        /// 驅動程式名稱
        /// </summary>
        public IList<string> DriverNames { get; set; }
    }
}
