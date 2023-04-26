using AutoMapper;
using DBEntities;
using DBEntities.Consts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.LetterheadTemplate;
using SealTypographicWebAPI.Utils;
using Serilog;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 會計師簽印樣板管理
    /// </summary>
    public class LetterheadImageTemplateService : ILetterheadImageTemplateService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ImageService imageSharpService;
        private readonly TemplateImagePathOption templateImagePathOption;
        private readonly IMapper mapper;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext">EF Core SealTypographic DbContext</param>        
        /// <param name="mapper">AutoMapper</param>
        /// <param name="imageSharpService">取得圖像資料</param>
        /// <param name="option">匯入AppSetting資料</param>
        public LetterheadImageTemplateService(SealTypographicDbContext dbContext, IMapper mapper, ImageService imageSharpService, IOptionsSnapshot<TemplateImagePathOption> option)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.imageSharpService = imageSharpService;
            this.templateImagePathOption = option.Value;
        }

        /// <summary>
        /// 信頭樣本詳細
        /// </summary>
        /// <returns></returns>
        public LetterheadImageTemplateDetailViewModel GetDetail(int Id)
        {
            LetterheadImageTemplateDetailViewModel letterheadImageTemplateDetailViewModel = new ();

            LetterheadImageTemplate? letterheadImageTemplateQuery = dbContext.LetterheadImageTemplates
                                                                      .Include(x => x.LetterheadImageTemplateLocations)
                                                                      .FirstOrDefault(x => x.Id == Id);

            if(letterheadImageTemplateQuery != null) 
            {
                letterheadImageTemplateDetailViewModel = mapper.Map<LetterheadImageTemplateDetailViewModel>(letterheadImageTemplateQuery);
                letterheadImageTemplateDetailViewModel.LocaltionViewModels = mapper.Map<List<LetterheadImageTemplateLocationViewModel>>
                                                                                (letterheadImageTemplateQuery.LetterheadImageTemplateLocations);
                letterheadImageTemplateDetailViewModel.Success();

            }

            return letterheadImageTemplateDetailViewModel;
        }

        /// <summary>
        /// 信頭樣板分頁顯示
        /// </summary>
        /// <param name="letterheadImageTemplateSearch">信頭樣板分頁搜尋</param>
        /// <returns></returns>
        public LetterheadImageTemplatePaginate GetPaginate(LetterheadImageTemplateSearch letterheadImageTemplateSearch)
        {
            LetterheadImageTemplatePaginate letterheadImageTemplatePaginate = new ();            
            int companyId = 1;

            IQueryable<LetterheadImageTemplate> letterheadImageTemplateQuery = dbContext.LetterheadImageTemplates
                                                                    .Where
                                                                    (
                                                                        letterheadImageTemplate => letterheadImageTemplate.Company.Id == companyId                                                                        
                                                                        && letterheadImageTemplate.DeleteStatus == DeleteStatus.No
                                                                    );

            if (!string.IsNullOrEmpty(letterheadImageTemplateSearch.KeyWord))
            {
                letterheadImageTemplateQuery = letterheadImageTemplateQuery
                                            .Where
                                            (
                                                letterheadImageTemplate => letterheadImageTemplate.Name.Contains(letterheadImageTemplateSearch.KeyWord)                
                                            );
            }
            letterheadImageTemplateQuery = letterheadImageTemplateQuery.OrderBy(temporarySealGroup => temporarySealGroup.Id);

            if (letterheadImageTemplateQuery.Any())
            {
                List<LetterheadImageTemplateViewModel> thisPageLetterheadImageTemplate = letterheadImageTemplateQuery                                                                      
                                                                    .Skip((letterheadImageTemplateSearch.PageNumber - 1) * letterheadImageTemplateSearch.PageSize)
                                                                    .Take(letterheadImageTemplateSearch.PageSize)
                                                                    .Select(letterheadImageTemplate => new LetterheadImageTemplateViewModel()
                                                                    {
                                                                        Id = letterheadImageTemplate.Id,
                                                                        Name = letterheadImageTemplate.Name,
                                                                        ImageFullPath = letterheadImageTemplate.ThumbnailFullPath,
                                                                        ThumbnailBase64 = imageSharpService.GetPathToBase64(letterheadImageTemplate.ThumbnailFullPath)
                                                                    })
                                                                    .ToList();

                letterheadImageTemplatePaginate.ViewModels = thisPageLetterheadImageTemplate;
                letterheadImageTemplatePaginate.PageNumber = letterheadImageTemplateSearch.PageNumber;
                letterheadImageTemplatePaginate.PageSize = letterheadImageTemplateSearch.PageSize;
                //計算總頁數
                letterheadImageTemplatePaginate.TotalPage = TotalPageUtil.GetTotalPage(letterheadImageTemplateQuery.Count(), letterheadImageTemplateSearch.PageSize);
                letterheadImageTemplatePaginate.TotalCount = letterheadImageTemplateQuery.Count();
                letterheadImageTemplatePaginate.Success();
            }
            LetterheadImageTemplatePaginateLog letterheadImageTemplatePaginateLog = mapper.Map<LetterheadImageTemplatePaginateLog>(letterheadImageTemplatePaginate);
            letterheadImageTemplatePaginateLog.LogModels = mapper.Map<List<LetterheadImageTemplateLogModel>>(letterheadImageTemplatePaginate.ViewModels);            
            Log.Information("LetterheadImageTemplate paginate output {@Output}", letterheadImageTemplatePaginate);
            return letterheadImageTemplatePaginate;
        }

        /// <summary>
        /// 信頭簽印樣板
        /// </summary>
        /// <param name="letterheadImageTemplateForm">信頭樣板</param>
        /// <returns></returns>
        public async Task<ResponseViewModel> New(LetterheadImageTemplateForm letterheadImageTemplateForm)
        {
            ResponseViewModel response = new();            
            int userid = 0; //帳號驗證取得ID
            int companyId = 1; //公司ID

            //尋找公司並與會計師簽印關聯
            Company? companyQuery = dbContext.Companys
                                    .Include(x => x.LetterheadImageTemplates)
                                    .Select(x => new Company 
                                    { 
                                        Id = x.Id , 
                                        Code = x.Code ,
                                        LetterheadImageTemplates = new List<LetterheadImageTemplate>()
                                    })
                                    .FirstOrDefault(x => x.Id == companyId);

            if (companyQuery != null) 
            {                
                LetterheadImageTemplate letterheadImageTemplate = mapper.Map<LetterheadImageTemplate>(letterheadImageTemplateForm);
                List<LetterheadImageTemplateLocation> letterheadImageTemplateLocations = new();

                //letterheadImageTemplate.ImageViewFullPath = await FormFileUtil.UploadFileReturnPath(letterheadImageTemplateForm.ImageBase64, companyQuery.Code, templateImagePathOption.Letterhead);
                //letterheadImageTemplate.ThumbnailFullPath = await FormFileUtil.UploadFileReturnPath(letterheadImageTemplateForm.ImageBase64Thumbnail, companyQuery.Code, templateImagePathOption.Letterhead);                
                letterheadImageTemplateLocations.Add(mapper.Map<LetterheadImageTemplateLocation>(letterheadImageTemplateForm.LetterheadTemplateLocationForm));       
                
                BaseInputLetterheadImageTemplate(letterheadImageTemplate, true, userid);
                letterheadImageTemplate.LetterheadImageTemplateLocations = letterheadImageTemplateLocations;                
                companyQuery.LetterheadImageTemplates.Add(letterheadImageTemplate);
                dbContext.Entry(companyQuery).State = EntityState.Unchanged;
                dbContext.LetterheadImageTemplates.Add(letterheadImageTemplate);                
                await dbContext.SaveChangesAsync();
                response.Success();
            }
            return response;
        }

        /// <summary>
        /// 更新信頭樣板
        /// </summary>        
        /// <param name="letterheadImageTemplateUpdateForm">信頭樣板</param>
        /// <returns></returns>
        public async Task<ResponseViewModel> Update(LetterheadImageTemplateUpdateForm letterheadImageTemplateUpdateForm)
        {
            ResponseViewModel response = new ();
            int userid = 1;
            LetterheadImageTemplate? letterheadImageTemplateQuery = dbContext.LetterheadImageTemplates.Include(x => x.LetterheadImageTemplateLocations)
                                                             .Include(x => x.Company)
                                                             .Select
                                                             (
                                                                x => new LetterheadImageTemplate()
                                                                {
                                                                    Id = x.Id,
                                                                    Company = new Company { Code = x.Company.Code},
                                                                    ImageViewFullPath = x.ImageViewFullPath,
                                                                    ThumbnailFullPath = x.ThumbnailFullPath,
                                                                    LetterheadImageTemplateLocations = x.LetterheadImageTemplateLocations,
                                                                }
                                                             )
                                                             .FirstOrDefault(x => x.Id == letterheadImageTemplateUpdateForm.Id);

            if (letterheadImageTemplateQuery != null)
            {                
                //更新圖片與縮圖
                //await FormFileUtil.SaveUpdata(letterheadImageTemplateUpdateForm.ImageBase64, letterheadImageTemplateQuery.ImageViewFullPath);
                //await FormFileUtil.SaveUpdata(letterheadImageTemplateUpdateForm.ImageBase64Thumbnail, letterheadImageTemplateQuery.ThumbnailFullPath);     
                
                mapper.Map(letterheadImageTemplateUpdateForm, letterheadImageTemplateQuery);
                BaseInputLetterheadImageTemplate(letterheadImageTemplateQuery, false, userid);

                LetterheadImageTemplateLocation? letterheadImageTemplateLocation = letterheadImageTemplateQuery.LetterheadImageTemplateLocations
                                                                                .FirstOrDefault(x => x.Id == letterheadImageTemplateUpdateForm.LocationUpdateForm.Id);
                if (letterheadImageTemplateLocation != null)
                {
                    mapper.Map(letterheadImageTemplateUpdateForm.LocationUpdateForm, letterheadImageTemplateLocation);
                }

                dbContext.Entry(letterheadImageTemplateQuery).State = EntityState.Modified;                
                await dbContext.SaveChangesAsync();
                response.Success();
            }

            return response;
        }
        /// <summary>
        /// 刪除信頭樣板
        /// </summary>
        /// <param name="Id">會計師簽印樣板Id</param>
        /// <returns></returns>
        public ResponseViewModel Delete (int Id)
        {
            ResponseViewModel response = new();
            int userId = 0;

            LetterheadImageTemplate? letterheadImageTemplateQuery = dbContext.LetterheadImageTemplates.Find(Id);

            if(letterheadImageTemplateQuery != null) 
            {
                letterheadImageTemplateQuery.DeleteStatus = DeleteStatus.Yes;
                BaseInputLetterheadImageTemplate(letterheadImageTemplateQuery, false, userId);
                response.Success();
            }
            else
            {
                response.DeleteLetterImageTemplateNoData();
            }

            return response;
        }

        /// <summary>
        /// 資料新增修改時基本資料輸入
        /// </summary>
        /// <param name="letterheadImageTemplate">DB上的樣板資料</param>
        /// <param name="isCreate">確認是否新增還是更新的動作</param>
        /// <param name="userid">使用者ID</param>
        private static void BaseInputLetterheadImageTemplate(LetterheadImageTemplate letterheadImageTemplate, bool isCreate, int userid)
        {
            if (isCreate)
            {
                letterheadImageTemplate.CreateUserId = userid;
                letterheadImageTemplate.CreateDate = DateTime.Now;
                letterheadImageTemplate.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                letterheadImageTemplate.UpdateUserId = userid;
                letterheadImageTemplate.UpdateDate = DateTime.Now;
            }
        }
    }
}
