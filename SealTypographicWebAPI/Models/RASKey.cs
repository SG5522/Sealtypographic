namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// RAS Key
    /// </summary>
    public class RASKey
    {
        /// <summary>
        /// RAS 私鑰
        /// </summary>
        public string PrivateKeyBase64 { get; set; }

        /// <summary>
        /// RAS 公鑰
        /// </summary>
        public string PublicKeyBase64 { get; set; }
    }
}
