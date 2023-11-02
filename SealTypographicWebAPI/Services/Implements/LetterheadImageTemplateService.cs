using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBEntities;
using DBEntities.Consts;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.BaseModels;
using SealTypographicWebAPI.Models.LetterheadImageTemplate;
using SealTypographicWebAPI.Utils;
using Serilog;
using System.Drawing.Printing;

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
        private readonly AutoMapper.IConfigurationProvider configurationProvider;

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
            configurationProvider = mapper.ConfigurationProvider;
            this.imageService = imageSharpService;
            
        }

        /// <summary>
        /// 信頭樣本詳細
        /// </summary>
        /// <returns></returns>
        public LetterheadImageTemplateDetailViewModel GetDetail(int Id)
        {
            LetterheadImageTemplateDetailViewModel? letterheadImageTemplateDetailViewModel = dbContext.Templates
                                                                                            .Include(x => x.TemplateLocations)
                                                                                            .Where(x => x.Id == Id)
                                                                                            .ProjectTo<LetterheadImageTemplateDetailViewModel>(configurationProvider)
                                                                                            .FirstOrDefault();



            if(letterheadImageTemplateDetailViewModel != null) 
            {
                letterheadImageTemplateDetailViewModel.Success();

            }
            else
            {
                letterheadImageTemplateDetailViewModel = new();
                letterheadImageTemplateDetailViewModel.DbNoData();
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

            string? imagePath = dbContext.Templates.Where(x => x.Id == id)
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

            IQueryable<Template> templateQuery = dbContext.Templates
                                                          .Where
                                                          (
                                                              template => template.Company.Id == companyId                                                                        
                                                              && template.DeleteStatus == DeleteStatus.No
                                                              && template.TemplateLocations.Any(x => x.SealType == SealType.Letterhead)
                                                          );

            if (!string.IsNullOrEmpty(letterheadImageTemplateSearch.KeyWord))
            {
                templateQuery = templateQuery.Where
                                (
                                    letterheadImageTemplate => letterheadImageTemplate.Name.Contains(letterheadImageTemplateSearch.KeyWord)                
                                );
            }
            templateQuery = templateQuery.OrderBy(temporarySealGroup => temporarySealGroup.Id);

            if (templateQuery.Any())
            {
                letterheadImageTemplatePaginate.ViewModels = templateQuery
                                                            .Skip((letterheadImageTemplateSearch.PageNumber - 1) * letterheadImageTemplateSearch.PageSize)
                                                            .Take(letterheadImageTemplateSearch.PageSize)
                                                            .ProjectTo<LetterheadImageTemplateViewModel>(configurationProvider)
                                                            .ToList();

                letterheadImageTemplatePaginate.PageNumber = letterheadImageTemplateSearch.PageNumber;
                letterheadImageTemplatePaginate.PageSize = letterheadImageTemplateSearch.PageSize;
                //計算總頁數
                //letterheadImageTemplatePaginate.TotalPage = PageUtil.GetTotalPage(templateQuery.Count(), letterheadImageTemplateSearch.PageSize);
                letterheadImageTemplatePaginate.TotalCount = templateQuery.Count();
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
                                    .Include(x => x.Templates)
                                    .FirstOrDefault(x => x.Id == companyId);

            if (companyQuery != null) 
            {                
                Template template = mapper.Map<Template>(letterheadImageTemplateForm);
                List<TemplateLocation> templateLocations = new();

                //儲存圖片(原圖)
                ImageBase64Info imageBase64Info = imageService.SetImageBase64InfoWithTemplate(companyQuery.Code, SealType.Letterhead);
                imageBase64Info.ImageBase64 = letterheadImageTemplateForm.ImageBase64;
                template.ImageViewFullPath = await imageService.GetSavedImageFilePath(imageBase64Info);
                //儲存縮圖
                imageBase64Info.ImageBase64 = letterheadImageTemplateForm.ImageBase64Thumbnail;
                template.ThumbnailFullPath = await imageService.GetSavedImageThumbnailFilePath(imageBase64Info, false);

                NewTemplateLoction(letterheadImageTemplateForm.LetterheadTemplateLocationForm, templateLocations);                                     
                BaseInputLetterheadImageTemplate(template, true, userid);
                template.TemplateLocations = templateLocations;
                template.Company = companyQuery;
                dbContext.Templates.Add(template);                      
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
            Template? template = dbContext.Templates.Include(x => x.TemplateLocations)    
                                .Include(x => x.Company)
                                .FirstOrDefault(x => x.Id == letterheadImageTemplateUpdateForm.Id);

            if (template != null)
            {
                //刪除原圖與縮圖
                FileUtil.DeleteImage(template.ImageViewFullPath);
                FileUtil.DeleteImage(template.ThumbnailFullPath);
                //儲存圖片(原圖)                
                ImageBase64Info imageBase64Info = imageService.SetImageBase64InfoWithTemplate(template.Company.Code, SealType.Letterhead);
                imageBase64Info.ImageBase64 = letterheadImageTemplateUpdateForm.ImageBase64;
                template.ImageViewFullPath = await imageService.GetSavedImageFilePath(imageBase64Info);
                //儲存縮圖
                imageBase64Info.ImageBase64 = letterheadImageTemplateUpdateForm.ImageBase64Thumbnail;
                template.ThumbnailFullPath = await imageService.GetSavedImageThumbnailFilePath(imageBase64Info, false);

                mapper.Map(letterheadImageTemplateUpdateForm, template);
                BaseInputLetterheadImageTemplate(template, false, userid);

                TemplateLocation? templateLocation = template.TemplateLocations.FirstOrDefault(x => x.Id == letterheadImageTemplateUpdateForm.LocationUpdateForm.Id);
                if (templateLocation != null)
                {
                    mapper.Map(letterheadImageTemplateUpdateForm.LocationUpdateForm, templateLocation);
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

            Template? templateQuery = dbContext.Templates.Find(Id);

            if(templateQuery != null) 
            {
                templateQuery.DeleteStatus = DeleteStatus.Yes;
                BaseInputLetterheadImageTemplate(templateQuery, false, userId);
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
        /// <param name="template">DB上的樣板資料</param>
        /// <param name="isCreate">確認是否新增還是更新的動作</param>
        /// <param name="userid">使用者ID</param>
        private static void BaseInputLetterheadImageTemplate(Template template, bool isCreate, int userid)
        {
            if (isCreate)
            {
                template.CreateUserId = userid;
                template.CreateDate = DateTime.Now;
                template.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                template.UpdateUserId = userid;
                template.UpdateDate = DateTime.Now;
            }
        }

        /// <summary>
        /// 新增樣版位置
        /// </summary>
        /// <param name="letterheadImageTemplateLocationForm"></param>
        /// <param name="templateLocations"></param>
        private void NewTemplateLoction(LetterheadImageTemplateLocationForm letterheadImageTemplateLocationForm, List<TemplateLocation> templateLocations)
        {
            TemplateLocation templateLocation = mapper.Map<TemplateLocation>(letterheadImageTemplateLocationForm);
            templateLocation.SealType = SealType.Letterhead;
            //之後要調整為不用轉型
            templateLocation.SubSealType = SubSealType.Letterhead;
            templateLocations.Add(templateLocation);            
        }
    }
}
