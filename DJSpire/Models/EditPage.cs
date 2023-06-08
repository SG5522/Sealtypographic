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
        /// 圖像與座標
        /// </summary>
        public List<EditImage> EditImages { get; set; }
    }
}
