using System.Collections.Generic;

namespace DJSpire.Models
{
    public class EditPage
    {
        private int pageNumber;
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
        /// 此頁是否加入會計師證明書
        /// </summary>
        public bool IsAccountantCertificate { get; set; }

        /// <summary>
        /// 圖像與座標
        /// </summary>
        public List<EditImage> EditImages { get; set; }
    }
}
