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
            Drivers = null;
        }

        /// <summary>
        /// 預設驅動程式CSV字串
        /// </summary>
        public IList<string>? Drivers { get; set; }
    }
}
