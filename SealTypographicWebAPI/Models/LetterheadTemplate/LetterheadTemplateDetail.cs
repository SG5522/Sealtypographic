using DBEntities.Consts;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.LetterheadTemplate
{
    /// <summary>
    /// 信頭樣板座標
    /// </summary>
    public class LetterheadTemplateLocationViewModel : BaseLocationViewModel
    {
        /// <summary>
        /// 樣板座標Id
        /// </summary>
        public int Id { get; set; }

    }

    /// <summary>
    /// 信頭樣本詳細
    /// </summary>
    public class LetterheadTemplateDetailViewModel : BaseTemplateWithResponse
    {
        /// <summary>
        /// New ViewModels
        /// </summary>
        public LetterheadTemplateDetailViewModel() 
        {
            LocaltionViewModels = new ();
        }

        /// <summary>
        /// 樣板Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 信頭樣板座標
        /// </summary>
        public List<LetterheadTemplateLocationViewModel> LocaltionViewModels{ get; set; }
    }
}
