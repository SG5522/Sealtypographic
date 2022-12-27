using AutoMapper;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Letterhead;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 信頭圖片管理
    /// </summary>
    public class LetterheadImageService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly IMapper mapper;

        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>        
        /// <param name="mapper"></param>
        public LetterheadImageService(SealTypographicDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得信頭圖片群組創建日期列表
        /// </summary>
        /// <param name="letterheadId">信頭Id</param>
        /// <returns></returns>
        public LetterheadGroupCreateDateViews GetLetterheadGroupCreateDateViews(int letterheadId)
        {
            LetterheadGroupCreateDateViews letterheadGroupCreateDateViews = new();
            //List<LetterheadGroupCreateDateView> letterheadGroupCreateDateViews = dbContext.LetterheadImageJournals
            //                               .Where
            //                               (
            //                                    letterheadImageJournal => letterheadImageJournal.LetterheadId == letterheadId
            //                                    && letterheadImageJournal.ReviewStatus <= ReviewStatus.Draft //暂存以下狀態過濾用
            //                               )
            //                               .Select(letterheadImageJournal => new LetterheadGroupCreateDateView()
            //                               {
            //                                   LetterheadId = letterheadImageJournal.LetterheadId,
            //                                   GroupCreateDate = letterheadImageJournal.GroupCreateDate,
            //                                   ReviewStatus = letterheadImageJournal.ReviewStatus
            //                               })
            //                               .GroupBy(letterheadImageJournal => letterheadImageJournal.GroupCreateDate)
            //                               .OrderByDescending(g => g.Key)
            //                               .Select(letterheadImageJournal => letterheadImageJournal.First())
            //                               .ToList();

            //if (letterheadGroupCreateDateViews.Any())
            //{
            //    letterheadGroupCreateDateViews.GroupCreateDates = letterheadGroupCreateDateViews;
            //    letterheadGroupCreateDateViews.Success();
            //}
            //else
            //{
            //    letterheadGroupCreateDateViews.AccountantNoData();
            //}
            return letterheadGroupCreateDateViews;
        }

        /// <summary>
        /// 取得信頭圖片
        /// </summary>
        /// <returns></returns>

        public LetterheadImageViewModels GetLetterheadImages (LetterheadGroupCreateDateSearch letterheadGroupCreateDateSearch)
        {
            return new();
        }
    }
}
