using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Util;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 勤業用的顧客資料
    /// </summary>
    public class LetterheadDeloitteService : ILetterheadService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly IMapper mapper;

        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public LetterheadDeloitteService(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得信頭資料
        /// </summary>
        /// <param name="litterheadID">信頭ID</param>
        /// <returns></returns>
        public LetterheadResponse GetLetterheadViewModel(string litterheadID)
        {
            LetterheadViewModel letterheadViewModel = new();
            ResponseViewModel response;
            Letterhead? letterheadQuery = dbContext.Letterheads
                                    .Where(letterhead => letterhead.Id == litterheadID)
                                    .FirstOrDefault();

            if (letterheadQuery != null)
            {
                letterheadViewModel = mapper.Map<LetterheadViewModel>(letterheadQuery);

                response = ResponseUtil.Success();
            }
            else
            {                
                response = ResponseUtil.NoData();
            }
            return new()
            {
                //回傳結果訊息用
                Code = response.Code,
                Message = response.Message,

                ViewModel = letterheadViewModel
            };
        }

        /// <summary>
        /// 取得顧客印鑑組
        /// </summary>
        /// <param name="letterheadSearch"></param>
        /// <returns></returns>
        public LetterheadViewModels GetLetterheadViewModels(LetterheadSearch letterheadSearch)
        {
            List<LetterheadViewModel> letterheadViewModels = new();
            ResponseViewModel response = new();
            int totalPage = 0;
            int totalCount = 0;
            IQueryable<Letterhead> letterheadQuery = dbContext.Letterheads;
            if (!string.IsNullOrWhiteSpace(letterheadSearch.LetterheadIdOrName))
            {
                letterheadQuery = letterheadQuery.Where
                                (
                                    letterhead =>
                                    letterhead.Id.Contains(letterheadSearch.LetterheadIdOrName)
                                    || letterhead.Name.Contains(letterheadSearch.LetterheadIdOrName)
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
                //計算總頁數
                totalPage = letterheadQuery.Count() / letterheadSearch.PageSize + (letterheadQuery.Count() % letterheadSearch.PageSize == 0 ? 0 : 1);
                totalCount = letterheadQuery.Count();
                foreach (Letterhead letterheadData in pageNumberLetterheads)
                {
                    letterheadViewModels.Add(mapper.Map<LetterheadViewModel>(letterheadData));
                }
                //取得成功訊息
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.NoData();
            }

            return new()
            {
                PageNumber = letterheadSearch.PageNumber,
                PageSize = letterheadSearch.PageSize,
                TotalCount = totalCount,
                TotalPage = totalPage,
                ViewModels = letterheadViewModels,
                //回傳結果訊息用
                Code = response.Code,
                Message = response.Message
            };            
        }

        /// <summary>
        /// 建立信頭資料
        /// </summary>
        /// <param name="letterheadPostData">基本資料</param>
        public ResponseViewModel CreateLetterhead(LetterheadForm letterheadPostData)
        {
            ResponseViewModel response = new();
            IQueryable<Letterhead> letterheadQuery = dbContext.Letterheads
                                                    .Where(letterhead => letterhead.Id == letterheadPostData.Id);

            if (!letterheadQuery.Any())
            {
                Letterhead dbLetterhead = mapper.Map<Letterhead>(letterheadPostData);
                dbContext.Letterheads.Add(dbLetterhead);
                dbContext.SaveChanges();
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.UniqueConstraintFailed();
            }
            return response;
        }

        /// <summary>
        /// 更新建立信頭資料
        /// </summary>
        /// <param name="letterheadPostData">基本資料</param>
        public ResponseViewModel UpdateLetterhead(LetterheadForm letterheadPostData)
        {
            ResponseViewModel response = new();
            Letterhead? letterheadQuery = dbContext.Letterheads
                                .Where(letterhead => letterhead.Id == letterheadPostData.Id)
                                .FirstOrDefault();

            if (letterheadQuery != null)
            {                
                mapper.Map(letterheadPostData, letterheadQuery);
                dbContext.SaveChanges();
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.NoData();
            }

            return response;
        }

        /// <summary>
        /// 刪除信頭資料(變更狀態使其一般USER無法看到)
        /// </summary>
        /// <param name="litterheadID"></param>
        public ResponseViewModel DeleteLetterhead(string litterheadID)
        {
            ResponseViewModel response = new();
            Letterhead? letterheadQuery = dbContext.Letterheads
                    .Where(letterhead => letterhead.Id == litterheadID)
                    .FirstOrDefault();

            if (letterheadQuery != null)
            {                
                letterheadQuery.Status = 2;
                dbContext.SaveChanges();
                response = ResponseUtil.Success();
            }
            else
            {
                response = ResponseUtil.NoData();
            }
            return response;            
        }
    }


}