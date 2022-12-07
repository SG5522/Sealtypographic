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
            return Get(SealType.Customer);
        }

        /// <summary>
        /// 通過(審核完成)
        /// </summary>
        /// <returns></returns>
        public static string Accountant()
        {
            return Get(SealType.Accountant);
        }

        /// <summary>
        /// 退件
        /// </summary>
        /// <returns></returns>
        public static string Letterhead()
        {
            return Get(SealType.Letterhead);
        }


        /// <summary>
        /// 取得UploadScanType名稱
        /// </summary>
        /// <returns></returns>
        public static string Get(SealType uploadScanType)
        {
            return uploadScanType switch
            {
                SealType.Customer => "客戶",
                SealType.Accountant => "會計師",
                SealType.Letterhead => "信頭",
                _ => "",
            };
        }
    }
}
