namespace DJScannerLib.Models
{
    /// <summary>
    /// 取得所有 TWAIN Drivers 結果
    /// </summary>
    public class GetDriversResult : BaseResult
    {
        public GetDriversResult() 
        {
            Success = false;
            Drivers = new List<TWAINDriver>();
            DriverNames = new List<string>();
        }

        /// <summary>
        /// 驅動程式
        /// </summary>
        public IList<TWAINDriver> Drivers { get; set; }

        /// <summary>
        /// 驅動程式字串
        /// </summary>
        public IList<string> DriverNames { get; set; }
    }
}
