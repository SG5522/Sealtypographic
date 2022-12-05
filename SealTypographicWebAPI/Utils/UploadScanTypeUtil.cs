using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// 掃描檔案文件類別
    /// </summary>
    public class UploadScanTypeUtil
    {
        /// <summary>
        /// 待審
        /// </summary>
        /// <returns></returns>
        public static string Customer()
        {
            return Get(UploadScanType.Customer);
        }

        /// <summary>
        /// 通過(審核完成)
        /// </summary>
        /// <returns></returns>
        public static string Accountant()
        {
            return Get(UploadScanType.Accountant);
        }

        /// <summary>
        /// 退件
        /// </summary>
        /// <returns></returns>
        public static string Letterhead()
        {
            return Get(UploadScanType.Letterhead);
        }


        /// <summary>
        /// 取得UploadScanType名稱
        /// </summary>
        /// <returns></returns>
        public static string Get(UploadScanType uploadScanType)
        {
            return uploadScanType switch
            {
                UploadScanType.Customer => "客戶",
                UploadScanType.Accountant => "會計師",
                UploadScanType.Letterhead => "信頭",
                _ => "",
            };
        }
    }
}
