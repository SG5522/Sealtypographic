using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Utils;
using DBEntities;
using DBEntities.Consts;
using DJLib.Models;
using SealTypographicWebAPI.Models.Accountant;
using DJLib;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 信頭圖片管理
    /// </summary>
    public class LetterheadImageService : ILetterheadImageService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ImageService imageService;                
        private readonly IStringLocalizer<LetterheadImageService> localizer;
        private readonly ILogger<LetterheadImageService> logger;

        /// <summary>
        /// 取得DB與ResponseService
        /// </summary>
        /// <param name="dbContext"></param>                
        /// <param name="imageSharpService"></param>
        /// <param name="localizer"></param>
        /// <param name="logger"></param>        
        public LetterheadImageService(SealTypographicDbContext dbContext, ImageService imageSharpService, IStringLocalizer<LetterheadImageService> localizer
            , ILogger<LetterheadImageService> logger)
        {
            this.dbContext = dbContext;
            this.imageService = imageSharpService;
            this.localizer = localizer;
            this.logger = logger;
        }

        ///<inheritdoc />
        public LetterheadImageStatusResponse GetStatus()
        {           
            LetterheadImageStatusResponse letterheadImageStatusResponse = new();

            try
            {
                foreach (LetterheadImageStatus letterheadImageStatus in (LetterheadImageStatus[])Enum.GetValues(typeof(LetterheadImageStatus)))
                {
                    LetterheadImageStatusViewModel letterheadImageStatusViewModel = new()
                    {
                        Id = (int)letterheadImageStatus,
                        Name = localizer[letterheadImageStatus.GetDescription()]
                    };
                    letterheadImageStatusResponse.ViewModels.Add(letterheadImageStatusViewModel);
                }
                letterheadImageStatusResponse.Success();
                logger.LogInformation("GetStatus output {@output}", letterheadImageStatusResponse);
            }
            catch (Exception ex) 
            {
                letterheadImageStatusResponse.Error();
                logger.LogInformation("GetStatus error {@error}", ex.Message);
            }            

            return letterheadImageStatusResponse;
        }

        ///<inheritdoc />
        public LetterheadImageCreateDateViews GetNameAndCreateDate(int letterheadId, int userId)
        {
            logger.LogInformation("GetNameAndCreateDate input letterheadId: {@letterheadId} userId: {@userId}", letterheadId, userId);

            LetterheadImageCreateDateViews letterheadImageCreateDateViews = new();

            try
            {
                Letterhead? letterhead = dbContext.Letterheads.Include(x => x.TypographicResources)
                                    .FirstOrDefault(letterhead => letterhead.Id == letterheadId);

                if (letterhead != null)
                {
                    letterheadImageCreateDateViews.Name = letterhead.Name;
                    letterheadImageCreateDateViews.CreateDateViews = letterhead.TypographicResources
                                                                    .Where(x => x.DeleteStatus == DeleteStatus.No)
                                                                    .Select(typographyResource => new LetterheadImageCreateDateView()
                                                                    {
                                                                        Id = typographyResource.Id,
                                                                        GroupCreateDate = letterhead.CreateDate,
                                                                        Status = letterhead.Status
                                                                    })
                                                                    .OrderByDescending(x => x.Id)
                                                                    .ToList();
                    letterheadImageCreateDateViews.Success();
                }
                else
                {
                    letterheadImageCreateDateViews.DbNoData();
                }
                logger.LogInformation("GetNameAndCreateDate output {@output}", letterheadImageCreateDateViews);
            }
            catch (Exception ex)
            {
                letterheadImageCreateDateViews.Error();
                logger.LogInformation("GetNameAndCreateDate error {@error}", ex.Message);
            }            
            return letterheadImageCreateDateViews;
        }

        ///<inheritdoc />
        public LetterheadImageViewModel GetImageViewModel(int id, bool isTransparent, int userId)
        {
            logger.LogInformation("GetImageViewModel input id: {@letterheadId} isTransparent: {@isTransparent} userId: {@userId}"
                , id, isTransparent, userId);

            LetterheadImageViewModel? letterheadImageViewModel;

            try
            {
                letterheadImageViewModel = dbContext.TypographicResources
                                        .Where(x => x.Id == id && x.DeleteStatus == DeleteStatus.No)
                                        .Select(x => new LetterheadImageViewModel
                                        {
                                            Id = x.Id,
                                            ImageBase64 = ImageSharpUtil.PathImageFileToBase64(x.ImageFullPath)
                                        }).FirstOrDefault();

                if (letterheadImageViewModel != null)
                {
                    if (isTransparent)
                    {
                        ImageInfo imageInfo = ImageInfo.FromImageBase64(letterheadImageViewModel.ImageBase64);
                        letterheadImageViewModel.ImageBase64 = imageInfo.TransparentToImageBase64();
                    }
                    letterheadImageViewModel.Success();
                }
                else
                {
                    letterheadImageViewModel = new();
                    letterheadImageViewModel.LetterheadImageNoData();
                }
                logger.LogInformation("GetPaginate output {@output}", letterheadImageViewModel);
            }
            catch (Exception ex)
            {
                letterheadImageViewModel = new();
                letterheadImageViewModel.Error();
                logger.LogInformation("GetImageViewModel error {@error}", ex.Message);
            }
                 
            return letterheadImageViewModel;            
        }

        ///<inheritdoc />
        public async Task<ResponseViewModel> New(LetterheadImageForm letterheadImageForm, int userId = 1)
        {
            logger.LogInformation("New input {@letterheadImageForm} userId: {@userId}", letterheadImageForm, userId);

            ResponseViewModel response = new();            
            int companyId = 1;//之後規劃從帳號取得公司ID

            try
            {
                //尋找公司並與客戶關聯
                Company? companyQuery = dbContext.Companys.Include(x => x.Letterheads).FirstOrDefault(x => x.Id == companyId);

                if (companyQuery != null)
                {
                    Letterhead letterhead = new()
                    {
                        TypographicResources = new List<TypographicResource>()
                    };

                    ImageBase64Info imageBase64Info = imageService.SetImageBase64InfoWithSeal(companyQuery.Code, SealType.Letterhead);

                    //信頭基本資料
                    letterhead.Name = letterheadImageForm.Name;
                    BaseInputLetterhead(letterhead, true, userId);

                    await NewTypographyResource(letterheadImageForm.ImageBase64, letterhead.TypographicResources, imageBase64Info, userId);

                    companyQuery.Letterheads.Add(letterhead);
                    dbContext.SaveChanges();
                    response.Success();
                }
                else
                {
                    response.DbNoData();
                }
                logger.LogInformation("New output {@output}", response);
            }
            catch (DbUpdateException ex)
            {
                response.DbError();
                logger.LogInformation("New dberror {@dberror}", ex.Message);
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogInformation("New error {@error}", ex.Message);
            }            
            return response;
        }

        ///<inheritdoc />
        public async Task<ResponseViewModel> Update(LetterheadImageUpdate letterheadImageUpdate, int userId = 1)
        {
            logger.LogInformation("Update output {@letterheadImageUpdate} userId: {@userId}", letterheadImageUpdate, userId);

            ResponseViewModel response = new();

            try
            {
                TypographicResource? updateImageQuery = dbContext.TypographicResources
                                                        .Include(typographyResource => typographyResource.Letterhead)
                                                        .ThenInclude(letterhead => letterhead!.Company)
                                                        .FirstOrDefault(typographyResource => typographyResource.Id == letterheadImageUpdate.Id);

                if (updateImageQuery != null)
                {
                    List<TypographicResource> typographyResources = new();
                    ImageBase64Info imageBase64Info = imageService.SetImageBase64InfoWithSeal(updateImageQuery.Letterhead!.Company.Code, SealType.Letterhead);

                    //原圖片狀態變更停用(刪除)
                    updateImageQuery.DeleteStatus = DeleteStatus.Yes;
                    TypographicResourceUtil.BaseInputTypographyResource(updateImageQuery, false, userId);

                    await NewTypographyResource(letterheadImageUpdate.ImageBase64, typographyResources, imageBase64Info, userId);

                    //變更信頭名稱
                    updateImageQuery.Letterhead.Name = letterheadImageUpdate.LetterheadName;
                    BaseInputLetterhead(updateImageQuery.Letterhead, false, userId);
                    
                    dbContext.TypographicResources.AddRange(typographyResources);
                    dbContext.SaveChanges();
                    response.Success();
                }
                else
                {
                    response.UpdateAccountantSignNoData();
                    response.ErrorItem = $"Update updateLetterheadImageId:{letterheadImageUpdate.Id}";
                }
                logger.LogInformation("Update output {@output}", response);
            }
            catch (DbUpdateException ex) 
            {
                response.DbError();
                logger.LogInformation("Update dberror {@dberror}", ex.Message);
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogInformation("Update error {@error}", ex.Message);
            }
            
            return response;
        }        

        /// <summary>
        /// 新增印鑑、簽印、圖片資料
        /// </summary>
        /// <param name="imageBase64">輸入圖片</param>
        /// <param name="typographyResources">要輸入資料庫的資源</param>
        /// <param name="imageBase64Info">圖檔資訊</param>
        /// <param name="userId">使用者Id</param>
        /// <returns></returns>
        private async Task NewTypographyResource(string imageBase64, IList<TypographicResource> typographyResources, ImageBase64Info imageBase64Info, int userId)
        {
            TypographicResource typographicResource = new()
            {
                SealType = SealType.Letterhead,
                //輸入model之後要修正為新的db
                SubSealType = SubSealType.Letterhead,
            };
            //ImageBase64轉圖檔並存到指定資料夾
            imageBase64Info.ImageBase64 = imageBase64;
            typographicResource.ImageFullPath = await imageService.GetSavedImageFilePath(imageBase64Info);
            typographicResource.ThumbnailFullPath = await imageService.GetSavedImageThumbnailFilePath(imageBase64Info, true);

            TypographicResourceUtil.BaseInputTypographyResource(typographicResource, true, userId);
            typographyResources.Add(typographicResource);
        }

        /// <summary>
        /// 信頭基本輸入
        /// </summary>
        /// <param name="letterhead"></param>
        /// <param name="isCreate"></param>
        /// <param name="userid"></param>
        private static void BaseInputLetterhead(Letterhead letterhead, bool isCreate, int userid)
        {
            if (isCreate)
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
