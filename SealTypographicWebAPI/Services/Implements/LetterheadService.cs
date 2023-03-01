using AutoMapper;
using Microsoft.EntityFrameworkCore;
using DBEntities.Consts;
using DBEntities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Utils;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 信頭管理
    /// </summary>
    public class LetterheadService : ILetterheadService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly IMapper mapper;

        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public LetterheadService(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得信頭資料列表(分頁)
        /// </summary>
        /// <param name="letterheadSearch">信頭分頁搜尋</param>
        /// <returns></returns>
        public LetterheadPaginateViewModel GetPaginate(LetterheadSearch letterheadSearch)
        {
            LetterheadPaginateViewModel letterheadPaginateViewModel = new ();
            List<LetterheadViewModel> letterheadViewModels = new();
            ResponseViewModel response = new();

            IQueryable<Letterhead> letterheadQuery = dbContext.Letterheads.Where(letterhead => letterhead.DeleteStatus == DeleteStatus.No)
                                                    .Include(letterhead => letterhead.LetterheadImageJournals);

            if (!string.IsNullOrWhiteSpace(letterheadSearch.Name))
            {
                letterheadQuery = letterheadQuery.Where
                                (
                                    letterhead => letterhead.Name.Contains(letterheadSearch.Name)
                                );
            }

            letterheadQuery = letterheadQuery.OrderBy(letterhead => letterhead.Id);

            if (letterheadQuery.Any())
            {
                //取得該頁            
                List<Letterhead> pageNumberLetterheads = letterheadQuery
                                                      .Skip((letterheadSearch.PageNumber - 1) * letterheadSearch.PageSize)
                                                      .Take(letterheadSearch.PageSize)
                                                      .ToList();                                

                foreach (Letterhead letterheadData in pageNumberLetterheads)
                {
                    LetterheadViewModel letterheadViewModel = mapper.Map<LetterheadViewModel>(letterheadData);

                    if (letterheadData.LetterheadImageJournals.Count > 0)
                    {
                        letterheadViewModel.LetterheadImageId = dbContext.LetterheadImageJournals.Where
                                                                (
                                                                    x => x.Letterhead.Id == letterheadData.Id
                                                                    && x.Status == LetterheadImageStatus.Enable
                                                                )
                                                                .Max(x => x.Id);
                    }
                    letterheadViewModels.Add(letterheadViewModel);
                }

                letterheadPaginateViewModel.ViewModels = letterheadViewModels;
                letterheadPaginateViewModel.PageNumber = letterheadSearch.PageNumber;
                letterheadPaginateViewModel.PageSize = letterheadSearch.PageSize;
                //計算總頁數
                letterheadPaginateViewModel.TotalPage = TotalPageUtil.GetTotalPage(letterheadQuery.Count(), letterheadSearch.PageSize);
                letterheadPaginateViewModel.TotalCount = letterheadQuery.Count();
                letterheadPaginateViewModel.Success();
            }
            else
            {
                letterheadPaginateViewModel.DbNoData();                
            }
            return letterheadPaginateViewModel;
        }


        /// <summary>
        /// 此刪除為更動狀態使其一般使用者看不到資料，
        /// 而不是真正的刪除。
        /// 另外連同關聯的信頭圖片標記刪除(只是標記刪除不是真正刪除)
        /// </summary>
        /// <param name="id">信頭Id</param>
        public ResponseViewModel Delete(int id)
        {
            ResponseViewModel response = new();
            int userId = 0;//帳號驗證取得Id
            Letterhead? letterheadQuery = dbContext.Letterheads.Include(x => x.LetterheadImageJournals)
                                          .FirstOrDefault(x => x.Id == id);

            if (letterheadQuery != null)
            {
                letterheadQuery.DeleteStatus = DeleteStatus.Yes;
                BaseInputLetterhead(letterheadQuery, false, userId);

                foreach(LetterheadImageJournal letterheadImageJournal in letterheadQuery.LetterheadImageJournals)
                {
                    letterheadImageJournal.DeleteStatus = DeleteStatus.Yes;
                    letterheadImageJournal.Status = LetterheadImageStatus.Disabled;
                }
                
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.DbNoData();
            }
            return response;            
        }

        /// <summary>
        /// 信頭資料新增修改時基本資料輸入
        /// </summary>
        /// <param name="letterhead">DB上的信頭資料</param>
        /// <param name="isCreate">確認是否新增的動作</param>
        /// <param name="userid">使用者ID</param>
        private static void BaseInputLetterhead(Letterhead letterhead, bool isCreate, int userid)
        {
            if(isCreate)
            {
                letterhead.CreateUserId = userid;
                letterhead.CreateDate = DateTime.Now;
                letterhead.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                letterhead.UpdateUserId = userid;
                letterhead.UpdateDate = DateTime.Now;
            }            
        }
    }


}