using AutoMapper;
using DBEntities;
using DBEntities.Consts;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantSignTemplate;
using SealTypographicWebAPI.Models.BaseModels;
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
        private readonly ImageService imageService;        
        private readonly IMapper mapper;

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="dbContext">EF Core SealTypographic DbContext</param>        
        /// <param name="mapper">AutoMapper</param>
        /// <param name="imageSharpService">取得圖像資料</param>        
        public LetterheadImageTemplateService(SealTypographicDbContext dbContext, IMapper mapper, ImageService imageSharpService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.imageService = imageSharpService;            
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
        /// 會計師簽印樣板圖片顯示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LetterheadImageTemplateImageView GetImage(int id)
        {
            LetterheadImageTemplateImageView viewImage = new();

            string? imagePath = dbContext.LetterheadImageTemplates.Where(x => x.Id == id)
                                                                  .Select(x => x.ImageViewFullPath).FirstOrDefault();

            if (imagePath != null)
            {
                viewImage.ImageBase64 = imageService.GetPathToBase64(imagePath);
                viewImage.Success();
            }
            return viewImage;
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
                letterheadImageTemplatePaginate.ViewModels = LoadPaginatedData(letterheadImageTemplateQuery, letterheadImageTemplateSearch.PageNumber, letterheadImageTemplateSearch.PageSize);
                letterheadImageTemplatePaginate.PageNumber = letterheadImageTemplateSearch.PageNumber;
                letterheadImageTemplatePaginate.PageSize = letterheadImageTemplateSearch.PageSize;
                //計算總頁數
                letterheadImageTemplatePaginate.TotalPage = TotalPageUtil.GetTotalPage(letterheadImageTemplateQuery.Count(), letterheadImageTemplateSearch.PageSize);
                letterheadImageTemplatePaginate.TotalCount = letterheadImageTemplateQuery.Count();
                letterheadImageTemplatePaginate.Success();
            }
            SavePaginateLog(letterheadImageTemplatePaginate);
            return letterheadImageTemplatePaginate;
        }

        /// <summary>
        /// 取得樣板分頁(排板使用)
        /// </summary>
        /// <param name="paginateSearch">分頁搜尋</param>
        /// <returns></returns>
        public LetterheadImageTemplatePaginate GetPaginateWithTypographic(PaginateSearch paginateSearch)
        {
            LetterheadImageTemplatePaginate letterheadImageTemplatePaginate = new();
            int companyId = 1;

            IQueryable<LetterheadImageTemplate> letterheadImageTemplateQuery = dbContext.LetterheadImageTemplates
                                                                            .Where
                                                                            (
                                                                                letterheadImageTemplate => letterheadImageTemplate.Company.Id == companyId
                                                                                && letterheadImageTemplate.DeleteStatus == DeleteStatus.No
                                                                            ).OrderBy(letterheadImageTemplate => letterheadImageTemplate.Id);

            if (letterheadImageTemplateQuery.Any())
            {
                letterheadImageTemplatePaginate.ViewModels = LoadPaginatedData(letterheadImageTemplateQuery, paginateSearch.PageNumber, paginateSearch.PageSize);
                letterheadImageTemplatePaginate.PageNumber = paginateSearch.PageNumber;
                letterheadImageTemplatePaginate.PageSize = paginateSearch.PageSize;
                //計算總頁數
                letterheadImageTemplatePaginate.TotalPage = TotalPageUtil.GetTotalPage(letterheadImageTemplateQuery.Count(), paginateSearch.PageSize);
                letterheadImageTemplatePaginate.TotalCount = letterheadImageTemplateQuery.Count();
                letterheadImageTemplatePaginate.Success();
            }
            SavePaginateLog(letterheadImageTemplatePaginate);
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

                //儲存圖片(原圖)
                ImageBase64Info imageBase64Info = imageService.SetImageBase64InfoWithTemplate(companyQuery.Code, SealType.Letterhead);
                imageBase64Info.ImageBase64 = letterheadImageTemplateForm.ImageBase64;
                letterheadImageTemplate.ImageViewFullPath = await imageService.GetSavedImageFilePath(imageBase64Info);
                //儲存縮圖
                imageBase64Info.ImageBase64 = letterheadImageTemplateForm.ImageBase64Thumbnail;
                letterheadImageTemplate.ThumbnailFullPath = await imageService.GetSavedImageThumbnailFilePath(imageBase64Info, false);

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
                                                                                                      .FirstOrDefault(x => x.Id == letterheadImageTemplateUpdateForm.Id);

            if (letterheadImageTemplateQuery != null)
            {
                //儲存圖片(原圖)                
                await imageService.SaveImageAsync(letterheadImageTemplateUpdateForm.ImageBase64, letterheadImageTemplateQuery.ImageViewFullPath, false);
                //儲存縮圖                
                await imageService.SaveImageAsync(letterheadImageTemplateUpdateForm.ImageBase64Thumbnail, letterheadImageTemplateQuery.ThumbnailFullPath, false);

                mapper.Map(letterheadImageTemplateUpdateForm, letterheadImageTemplateQuery);
                BaseInputLetterheadImageTemplate(letterheadImageTemplateQuery, false, userid);

                LetterheadImageTemplateLocation? letterheadImageTemplateLocation = letterheadImageTemplateQuery.LetterheadImageTemplateLocations
                                                                                .FirstOrDefault(x => x.Id == letterheadImageTemplateUpdateForm.LocationUpdateForm.Id);
                if (letterheadImageTemplateLocation != null)
                {
                    mapper.Map(letterheadImageTemplateUpdateForm.LocationUpdateForm, letterheadImageTemplateLocation);
                }

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
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.DeleteLetterImageTemplateNoData();
            }

            return response;
        }

        /// <summary>
        /// 讀取分頁資料
        /// </summary>        
        /// <param name="letterheadImageTemplateQuery">信頭樣板</param>
        /// <param name="pageNumber">頁次</param>
        /// <param name="pageSize">頁面大小</param>        
        private List<LetterheadImageTemplateViewModel> LoadPaginatedData(IQueryable<LetterheadImageTemplate> letterheadImageTemplateQuery, int pageNumber, int pageSize)
        {
            return
                letterheadImageTemplateQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(letterheadImageTemplate => new LetterheadImageTemplateViewModel()
                {
                    Id = letterheadImageTemplate.Id,
                    Name = letterheadImageTemplate.Name,
                    ImageFullPath = letterheadImageTemplate.ThumbnailFullPath,
                    ThumbnailBase64 = imageService.GetPathToBase64(letterheadImageTemplate.ThumbnailFullPath)
                }).ToList();
        }

        /// <summary>
        /// 紀錄分頁Log
        /// </summary>
        /// <param name="letterheadImageTemplatePaginate">分頁列表</param>
        private void SavePaginateLog(LetterheadImageTemplatePaginate letterheadImageTemplatePaginate)
        {            
            LetterheadImageTemplatePaginateLog letterheadImageTemplatePaginateLog = mapper.Map<LetterheadImageTemplatePaginateLog>(letterheadImageTemplatePaginate);
            letterheadImageTemplatePaginateLog.LogModels = mapper.Map<List<LetterheadImageTemplateLogModel>>(letterheadImageTemplatePaginate.ViewModels);
            Log.Information("LetterheadImageTemplate paginate output {@Output}", letterheadImageTemplatePaginateLog);            
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
