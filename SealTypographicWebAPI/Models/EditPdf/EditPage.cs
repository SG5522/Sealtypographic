namespace SealTypographicWebAPI.Models.EditPdf
{
    /// <summary>
    /// PDF該頁的編輯內容
    /// </summary>
    public class EditPage
    {
        private int pageNumber;
        private string accountantCertificatePath;

        /// <summary>
        /// 建置
        /// </summary>
        public EditPage()
        {
            EditImages = new List<EditImage>();
        }

        /// <summary>
        /// 頁次
        /// </summary>
        public int PageNumber
        {
            get
            {
                return pageNumber;
            }
            set
            {
                pageNumber = value - 1;
            }
        }

        /// <summary>
        /// 空白頁確認
        /// </summary>
        public bool BlankCheck { get; set; }

        /// <summary>
        /// 刪除頁確認
        /// </summary>
        public bool DeleteCheck { get; set; }

        /// <summary>
        /// 下頁加入會計師證明書
        /// </summary>
        public string AccountantCertificatePath
        {
            get { return accountantCertificatePath; }
            set
            {
                accountantCertificatePath = value;
                if (accountantCertificatePath != string.Empty)
                {
                    AccountantCertificateStream = File.OpenRead(accountantCertificatePath);
                }
            }
        }

        /// <summary>
        /// 會計師證明書圖片轉成流
        /// </summary>
        public Stream AccountantCertificateStream { get; set; }

        /// <summary>
        /// 圖像與座標
        /// </summary>
        public List<EditImage> EditImages { get; set; }
    }
}
