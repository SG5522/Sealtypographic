using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        public CustomerSealCaptureResponse GetCustomerSealCapture(int companyId = 1)
        {
            logger.LogInformation("GetCustomerSealCapture input userId: {@userId}", companyId);

            CustomerSealCaptureResponse result = new ();

            try
            {
                CustomerSealCaptureSetting? ImageCaptureSetting = dbContext.ImageCaptureSettings
                                                                .Include(x => x.ImageCaptureLocations)
                                                                .Where(x => x.Company.Id == companyId && x.ImageCaptureLocations.Any(location => location.SealType == SealType.Customer))
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
        public AccountantSignCaptureResponse GetAccountantSignCapture(int companyId = 1)
        {
            logger.LogInformation("GetAccountantSignCapturee input companyId: {@companyId}", companyId);

            AccountantSignCaptureResponse result = new();

            try
            {
                AccountantSignCaptureSetting? ImageCaptureSetting = dbContext.ImageCaptureSettings
                                                                    .Include(x => x.ImageCaptureLocations)
                                                                    .Where(x => x.User.Id == companyId && x.ImageCaptureLocations.Any(location => location.SealType == SealType.Accountant))
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
        public ResponseViewModel New<T>(T captureSetting, int companyId = 1)
        {
            logger.LogInformation("New input {@captureSetting} companyId: {@companyId}", captureSetting, companyId);

            ResponseViewModel response = new ();

            try
            {                
                ImageCaptureSetting imageCaptureSetting = mapper.Map<ImageCaptureSetting>(captureSetting);
                imageCaptureSetting.Company = dbContext.Companys.Single(x => x.Id == companyId);
                InputUtil.Base(imageCaptureSetting, true, companyId);
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

        ///<inheritdoc />
        public ResponseViewModel Update<T>(T captureSetting, SealType sealType, int companyId = 1)
        {
            logger.LogInformation("Update input {@captureSetting} userId: {@userId}", captureSetting, companyId);

            ResponseViewModel response = new();

            try
            {
                ImageCaptureSetting? imageCaptureSetting = dbContext.ImageCaptureSettings
                                                                    .Include(x => x.ImageCaptureLocations)
                                                                    .Where(
                                                                        x => x.Company.Id == companyId
                                                                        && x.ImageCaptureLocations.Any(g => g.SealType == sealType)
                                                                    ).FirstOrDefault();
                if (imageCaptureSetting != null)
                {
                    mapper.Map(captureSetting, imageCaptureSetting);
                    InputUtil.Base(imageCaptureSetting, false, companyId);                    
                    dbContext.SaveChanges();
                    logger.LogInformation("Update output {@output}", response);
                }
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogError("Update error {@error}", ex.Message);
            }
            return response;
        }
    }
}
