using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBEntities;
using DBEntities.Consts;
using DBEntities.Utils;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.BaseModels;
using SealTypographicWebAPI.Models.ImageRangeSetting;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 印鑑截取範圍設定
    /// </summary>
    public class ImageRangeSettingService : IImageRangeSettingService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private readonly ILogger<ImageRangeSettingService> logger;

        /// <summary>
        /// 建置
        /// </summary>
        public ImageRangeSettingService(SealTypographicDbContext dbContext, IMapper mapper, ILogger<ImageRangeSettingService> logger)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;
            this.logger = logger;
        }

        ///<inheritdoc />
        public CustomerSealRangeSettingResponse GetCustomerSealRangeSetting(int companyId = 1)
        {
            logger.LogInformation("GetCustomerSealCapture input userId: {@userId}", companyId);

            CustomerSealRangeSettingResponse result = new ();

            try
            {
                CustomerSealRangeSetting? ImageCaptureSetting = dbContext.ImageRangeSettings
                                                                .Include(x => x.ImageRangeLocations)
                                                                .Where(x => x.Company!.Id == companyId && x.ImageRangeLocations.Any(location => location.SealType == SealType.Customer))
                                                                .ProjectTo<CustomerSealRangeSetting>(configurationProvider)
                                                                .FirstOrDefault();

                if (ImageCaptureSetting != null) 
                {
                    result.CustomerSealRangeSetting = ImageCaptureSetting;
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
        public AccountantSignRangeSettingResponse GetAccountantSignRangeSetting(int companyId = 1)
        {
            logger.LogInformation("GetAccountantSignCapturee input companyId: {@companyId}", companyId);

            AccountantSignRangeSettingResponse result = new();

            try
            {
                AccountantSignRangeSetting? ImageCaptureSetting = dbContext.ImageRangeSettings
                                                                    .Include(x => x.ImageRangeLocations)
                                                                    .Where(x => x.Company!.Id == companyId && x.ImageRangeLocations.Any(location => location.SealType == SealType.Accountant))
                                                                    .ProjectTo<AccountantSignRangeSetting>(configurationProvider)
                                                                    .FirstOrDefault();

                if (ImageCaptureSetting != null)
                {
                    result.AccountantSignRangeSetting = ImageCaptureSetting;
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
                ImageRangeSetting imageCaptureSetting = mapper.Map<ImageRangeSetting>(captureSetting);
                imageCaptureSetting.Company = dbContext.Companys.Single(x => x.Id == companyId);
                InputUtil.Base(imageCaptureSetting, true, companyId);
                dbContext.ImageRangeSettings.Add(imageCaptureSetting);
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
        public ResponseViewModel Update<T>(int id, T captureSetting, SealType sealType, int userId = 1) where T : BaseLocation
        {
            logger.LogInformation("Update input {@captureSetting} sealType: {@sealType} userId: {@userId}", captureSetting, sealType, userId);

            ResponseViewModel response = new();

            try
            {
                ImageRangeSetting? imageCaptureSetting = dbContext.ImageRangeSettings
                                                                    .Include(x => x.ImageRangeLocations)
                                                                    .Where(x => x.Id == id)                                                                        
                                                                    .FirstOrDefault();
                if (imageCaptureSetting != null)
                {
                    mapper.Map(captureSetting, imageCaptureSetting);
                    InputUtil.Base(imageCaptureSetting, false, userId);                    
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
