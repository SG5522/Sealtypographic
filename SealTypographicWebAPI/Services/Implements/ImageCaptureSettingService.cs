using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure;
using DBEntities;
using DBEntities.Consts;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.SealCaptureRange;
using SealTypographicWebAPI.Utils;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 印鑑截取範圍設定
    /// </summary>
    public class ImageCaptureSettingService : IImageCaptureSettingService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private readonly ILogger<ImageCaptureSettingService> logger;

        /// <summary>
        /// 建置
        /// </summary>
        public ImageCaptureSettingService(SealTypographicDbContext dbContext, IMapper mapper, ILogger<ImageCaptureSettingService> logger)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;
            this.logger = logger;
        }

        ///<inheritdoc />
        public CustomerSealCaptureResponse GetCustomerSealCapture(int userId = 1)
        {
            logger.LogInformation("GetCustomerSealCapture input userId: {@userId}", userId);

            CustomerSealCaptureResponse result = new ();

            try
            {
                CustomerSealCaptureSetting? ImageCaptureSetting = dbContext.ImageCaptureSettings
                                                            .Include(x => x.ImageCaptureLocations)
                                                            .Where(x => x.User.Id == userId && x.ImageCaptureLocations.Any(location => location.SealType == SealType.Customer))
                                                            .ProjectTo<CustomerSealCaptureSetting>(configurationProvider)
                                                            .FirstOrDefault();

                if (ImageCaptureSetting != null) 
                {
                    result.CustomerSealCaptureSetting = ImageCaptureSetting;
                    result.Success();
                }

                logger.LogInformation("GetCustomerSealCapture output {@output}", result);
            }
            catch (Exception ex) 
            {
                result.Error();
                logger.LogError("GetCustomerSealCapture error {@error}", ex.Message);
            }
            return result;
        }

        ///<inheritdoc />
        public AccountantSignCaptureResponse GetAccountantSignCapture(int userId = 1)
        {
            logger.LogInformation("GetAccountantSignCapturee input userId: {@userId}", userId);

            AccountantSignCaptureResponse result = new();

            try
            {
                AccountantSignCaptureSetting? ImageCaptureSetting = dbContext.ImageCaptureSettings
                                                                    .Include(x => x.ImageCaptureLocations)
                                                                    .Where(x => x.User.Id == userId && x.ImageCaptureLocations.Any(location => location.SealType == SealType.Customer))
                                                                    .ProjectTo<AccountantSignCaptureSetting>(configurationProvider)
                                                                    .FirstOrDefault();

                if (ImageCaptureSetting != null)
                {
                    result.AccountantSignCaptureSetting = ImageCaptureSetting;
                    result.Success();
                }

                logger.LogInformation("GetAccountantSignCapturee output {@output}", result);
            }
            catch (Exception ex)
            {
                result.Error();
                logger.LogError("GetAccountantSignCapturee error {@error}", ex.Message);
            }
            return result;
        }

        ///<inheritdoc />
        public ResponseViewModel New<T>(T captureSetting, int userId = 1)
        {
            logger.LogInformation("New input {@captureSetting} userId: {@userId}", captureSetting, userId);

            ResponseViewModel response = new ();

            try
            {                
                ImageCaptureSetting imageCaptureSetting = mapper.Map<ImageCaptureSetting>(captureSetting);
                imageCaptureSetting.User = dbContext.Users.Single(x => x.Id == userId);
                InputUtil.Base(imageCaptureSetting, true, userId);
                dbContext.ImageCaptureSettings.Add(imageCaptureSetting);
                dbContext.SaveChanges();
                logger.LogInformation("New output {@output}", response);
            }
            catch (DbUpdateException ex)
            {
                response.DbError();
                logger.LogError("New dberror {@dberror}", ex.Message);
            }
            catch (Exception ex) 
            {
                response.Error();
                logger.LogError("New error {@error}", ex.Message);
            }
            return response;
        }
    }
}
