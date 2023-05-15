using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantSignTemplate;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 會計師簽印樣板管理
    /// </summary>
    public interface IAccountantSignTemplateService
    {
        /// <summary>
        /// 會計師簽印樣板詳細資料
        /// </summary>
        /// <param name="id">會計師簽印樣本Id</param>
        /// <returns></returns>
        AccountantSignTemplateDetailViewModel GetDetail(int id);

        /// <summary>
        /// 會計師簽印樣板圖片顯示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AccountantSignTemplateImageView GetImage(int id);

        /// <summary>
        /// 會計師簽印樣板分頁顯示
        /// </summary>
        /// <param name="accountantSignTemplateSearch">會計師簽印樣板分頁搜尋</param>
        /// <returns></returns>
        AccountantSignTemplatePaginate GetPaginate(AccountantSignTemplateSearch accountantSignTemplateSearch);

        /// <summary>
        /// 取得樣板分頁(排板使用)
        /// </summary>
        /// <param name="paginateSearch"></param>
        /// <returns></returns>
        AccountantSignTemplatePaginate GetPaginateWithTypographic(PaginateSearch paginateSearch);

        /// <summary>
        /// 新增會計師簽印樣板
        /// </summary>
        /// <param name="accountantSignTemplateForm">會計師簽印樣板</param>
        /// <returns></returns>
        Task<ResponseViewModel> New(AccountantSignTemplateForm accountantSignTemplateForm);

        /// <summary>
        /// 更新會計師簽印樣板
        /// </summary>        
        /// <param name="accountantSignTemplateUpdateForm"></param>
        /// <returns></returns>
        Task<ResponseViewModel> Update(AccountantSignTemplateUpdateForm accountantSignTemplateUpdateForm);

        /// <summary>
        /// 刪除會計師簽印樣板
        /// </summary>
        /// <param name="Id">會計師簽印樣板Id</param>
        /// <returns></returns>
        ResponseViewModel Delete(int Id);

    }
}
