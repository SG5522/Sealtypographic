using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Util;
using SealTypographicWebAPI.Utils;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 勤業用的顧客資料
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
        /// 取得信頭資料
        /// </summary>
        /// <param name="litterheadId">信頭ID</param>
        /// <returns></returns>
        public LetterheadResponse GetLetterheadViewModel(int litterheadId)
        {
            LetterheadResponse letterheadResponse = new();
            Letterhead? letterheadQuery = dbContext.Letterheads.Find(litterheadId);

            if (letterheadQuery != null)
            {
                letterheadResponse.ViewModel = mapper.Map<LetterheadViewModel>(letterheadQuery);
                letterheadResponse.Success();                
            }
            else
            {
                letterheadResponse.DbNoData();                
            }
            return letterheadResponse;
        }

        /// <summary>
        /// 取得顧客印鑑組
        /// </summary>
        /// <param name="letterheadSearch"></param>
        /// <returns></returns>
        public LetterheadViewModels GetLetterheadViewModels(LetterheadSearch letterheadSearch)
        {
            LetterheadViewModels letterheadViewModels = new ();
            List<LetterheadViewModel> viewModels = new();
            ResponseViewModel response = new();
            int totalPage = 0;
            int totalCount = 0;
            IQueryable<Letterhead> letterheadQuery = dbContext.Letterheads;
            if (!string.IsNullOrWhiteSpace(letterheadSearch.Name))
            {
                letterheadQuery = letterheadQuery.Where
                                (
                                    letterhead =>
                                    letterhead.Name.Contains(letterheadSearch.Name)
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
                    viewModels.Add(mapper.Map<LetterheadViewModel>(letterheadData));
                }

                letterheadViewModels.ViewModels = viewModels;
                letterheadViewModels.PageNumber = letterheadSearch.PageNumber;
                letterheadViewModels.PageSize = letterheadSearch.PageSize;
                //計算總頁數
                letterheadViewModels.TotalPage = TotalPageUtil.GetTotalPage(letterheadQuery.Count(), letterheadQuery.Count());
                letterheadViewModels.TotalCount = letterheadQuery.Count();
                letterheadViewModels.Success();
            }
            else
            {
                letterheadViewModels.DbNoData();                
            }

            return letterheadViewModels;
        }

        /// <summary>
        /// 建立信頭資料
        /// </summary>
        /// <param name="letterheadForm">基本資料</param>
        public LetterheadCreateResronse CreateLetterhead(LetterheadForm letterheadForm)
        {
            LetterheadCreateResronse resronse = new();
            IQueryable<Letterhead> letterheadQuery = dbContext.Letterheads
                    .Where(letterhead => letterhead.LetterheadNumber == letterheadForm.LetterheadNumber);

            if(!letterheadQuery.Any())
            {
                Letterhead dbLetterhead = mapper.Map<Letterhead>(letterheadForm);
                dbContext.Letterheads.Add(dbLetterhead);
                dbContext.SaveChanges();

                //回傳剛建立的客戶基本資料 使建立客戶印鑑找到該ID
                Letterhead? letterhead = dbContext.Letterheads.Where(letterhead => letterhead.LetterheadNumber == letterheadForm.LetterheadNumber).FirstOrDefault();
                resronse.LetterheadId = letterhead.Id;
                resronse.Success();
            }



            return resronse;
        }

        /// <summary>
        /// 更新建立信頭資料
        /// </summary>
        /// <param name="letterheadFormUpdate">基本資料</param>
        public ResponseViewModel UpdateLetterhead(LetterheadFormUpdate letterheadFormUpdate)
        {
            ResponseViewModel response = new();
            Letterhead? letterheadQuery = dbContext.Letterheads.Find(letterheadFormUpdate.Id);

            if (letterheadQuery != null)
            {                
                mapper.Map(letterheadFormUpdate, letterheadQuery);
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
        /// 刪除信頭資料(變更狀態使其一般USER無法看到)
        /// </summary>
        /// <param name="litterheadID"></param>
        public ResponseViewModel DeleteLetterhead(int litterheadID)
        {
            ResponseViewModel response = new();
            Letterhead? letterheadQuery = dbContext.Letterheads.Find(litterheadID);

            if (letterheadQuery != null)
            {
                letterheadQuery.DeleteStatus = DeleteStatus.Yes;
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.DbNoData();
            }
            return response;            
        }
    }


}