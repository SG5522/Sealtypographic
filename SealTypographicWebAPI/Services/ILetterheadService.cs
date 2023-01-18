using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.Letterhead;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 管理信頭資料的interface
    /// </summary>
    public interface ILetterheadService
    {
        /// <summary>
        /// 取得信頭資料列表(分頁)
        /// </summary>
        /// <param name="letterheadSearch">信頭分頁搜尋</param>         
        /// <returns></returns>
        LetterheadPaginateViewModel GetPaginate(LetterheadSearch letterheadSearch);


        /// <summary>        
        /// 此刪除為更動狀態使其一般使用者看不到資料，
        /// 而不是真正的刪除。
        /// </summary>
        /// <param name="Id">信頭Id</param>
        ResponseViewModel Delete(int Id);
    }
}
