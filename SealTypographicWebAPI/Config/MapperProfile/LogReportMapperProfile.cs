using AutoMapper;
using DBEntities.Consts;
using DBEntities.Entities.AccountantModels;
using DBEntities.Entities.CustomerModels;
using DBEntities.Entities.TypographicModels;
using DJKeycloakAPI.Models.Groups;
using DJKeycloakAPI.Models.Users;
using DJKeycloakLib.Models.Group;
using DJKeycloakLib.Models.User;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.AccountantSignReview;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.CustomerSeal;
using SealTypographicWebAPI.Models.CustomerSealReview;
using SealTypographicWebAPI.Models.LogReport.AccountantList;
using SealTypographicWebAPI.Models.LogReport.AccountantMember;
using SealTypographicWebAPI.Models.LogReport.AccountantSignLog;
using SealTypographicWebAPI.Models.LogReport.CustomerSealEventLog;
using SealTypographicWebAPI.Models.LogReport.OperationLog;
using SealTypographicWebAPI.Models.LogReport.TypographicReport;
using SealTypographicWebAPI.Models.LogReport.UserMember;
using SealTypographicWebAPI.Models.MongoDBModel;
using SealTypographicWebAPI.Utils;
using static Azure.Core.HttpHeader;

namespace SealTypographicWebAPI.Config.MapperProfile
{
    /// <summary>
    /// AutoMapper用的LIST
    /// </summary>
    public class LogReport : Profile
    {
        /// <summary>
        /// 建置
        /// </summary>
        public LogReport()
        {
            int logReportQueryValue = 0;

            //會計師分頁顯示Map
            CreateMap<TypographicPDF, TypographicReportViewModel>()
                    ////TODO:透過keycloak取得使用者Id
                    //.ForMember(dst => dst.CreateUserId, opt => opt.MapFrom(src => src.CreateUserId))
                    ////TODO:透過keycloak取得使用者名稱
                    //.ForMember(dst => dst.CreateUserName, opt => opt.MapFrom(src => src.CreateUserId.ToString()))
                    .ForMember(dst => dst.UserName, opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.UpdateUser!.UserName) ? src.UpdateUser.UserName : string.Empty))
                    .ForMember(dst => dst.UserNickName, opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.UpdateUser!.LastName) ? src.UpdateUser.LastName : string.Empty))
                    .ForMember(dst => dst.CustomerCode, opt => opt.MapFrom(src => src.Customer.Code))
                    .ForMember(dst => dst.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
                    .ForMember(dst => dst.RecordDate, opt => opt.MapFrom(src => src.UpdateDate))
                    .ForMember(dst => dst.EditFileName, opt => opt.MapFrom(src => src.OriginFileName))
                    .ForMember(dst => dst.EditPageCount, opt => opt.MapFrom(src => src.TypographicPages.Where(x => x.BlankCheck == false).Count()))
                    .ForMember(dst => dst.BlankPageCount, opt => opt.MapFrom(src =>
                        //判斷是否是財報，如果是就提供空白頁次，否則null
                            src.TypographyType == TypographyType.FinancialReport ?
                            src.TypographicPages.Where(x => x.BlankCheck).Count() : (int?)null
                    ));

            //操作紀錄Map
            CreateMap<OperationLog, OperationLogViewModel>()                    
                    .ForMember(dst => dst.ActionType, opt => opt.MapFrom(src => src.Data!.ActionType))
                    .ForMember(dst => dst.TargetName, opt => opt.MapFrom(src => 
                        !string.IsNullOrWhiteSpace(src.Data!.AccountantName) ? src.Data.AccountantName : src.Data.CustomerName
                    ))
                    .ForMember(dst => dst.DisplayQuarterYear, opt => opt.MapFrom(src =>
                        !string.IsNullOrWhiteSpace(src.Data!.DisplayQuarterYear) ? src.Data.DisplayQuarterYear : null
                    ));

            //操作紀錄Map客戶資料
            CreateMap<CustomerDetail, OperationLogSave>()
                    .ForMember(dst => dst.ActionType, opt => opt.MapFrom(src => ActionType.CustomerQuery))
                    .ForMember(dst => dst.CustomerId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.CustomerName, opt => opt.MapFrom(src => src.Name));                    

            //操作紀錄Map客戶印鑑資料
            CreateMap<CustomerSealViewModels, OperationLogSave>()                    
                    .ForMember(dst => dst.CustomerSealGroupId, opt => opt.MapFrom(src => src.CustomerSealQuarterId))
                    .ForMember(dst => dst.ActionType, opt => opt.MapFrom(src => 
                        src.TypographyType == TypographyType.FinancialReport ? 
                        ActionType.FinancialReportSealQuery : ActionType.TaxReportSealQuery
                    ));

            //操作紀錄Map會計簽印資料
            CreateMap<AccountantViewModel, OperationLogSave>()
                    .ForMember(dst => dst.ActionType, opt => opt.MapFrom(src => ActionType.AccountantQuery))
                    .ForMember(dst => dst.AccountantId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.AccountantName, opt => opt.MapFrom(src => src.Name));

            CreateMap<CustomerSealGroupReviewViewModel, OperationLogSave>()
                    .ForMember(dst => dst.ActionType, opt => opt.MapFrom(src =>
                        src.TypographyType == TypographyType.FinancialReport ?
                        ActionType.FinancialReportSealQuery : ActionType.TaxReportSealQuery
                    ))                    
                    .ForMember(dst => dst.CustomerName, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dst => dst.CustomerSealGroupId, opt => opt.MapFrom(src => src.Id));

            //客戶印鑑審核查詢操作紀錄
            CreateMap<CustomerSealGroupDetailReviewViewModel, OperationLogSave>()
                    .ForMember(dst => dst.ActionType, opt => opt.MapFrom(src =>
                        src.TypographyType == TypographyType.FinancialReport ?
                        ActionType.FinancialReportSealQuery : ActionType.TaxReportSealQuery
                    ))
                    .ForMember(dst => dst.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                    .ForMember(dst => dst.CustomerName, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dst => dst.CustomerSealGroupId, opt => opt.MapFrom(src => src.Id));

            //操作紀錄Map會計簽印資料
            CreateMap<AccountantSignViewModels, OperationLogSave>()
                    .ForMember(dst => dst.ActionType, opt => opt.MapFrom(src => ActionType.AccountantSignQuery))
                    .ForMember(dst => dst.AccountantSignGroupCreateDate, opt => opt.MapFrom(src => src.GroupCreateDate));

            //會計師審核查詢操作紀錄
            CreateMap<AccountantSignGroupReviewViewModel, OperationLogSave>()
                    .ForMember(dst => dst.ActionType, opt => opt.MapFrom(src => ActionType.AccountantSignQuery))
                    .ForMember(dst => dst.AccountantId, opt => opt.MapFrom(src => src.AccountantId))
                    .ForMember(dst => dst.AccountantName, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dst => dst.AccountantSignGroupId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.AccountantSignGroupCreateDate, opt => opt.MapFrom(src => src.GroupCreateDate));

            //會計師審核查詢操作紀錄
            CreateMap<AccountantSignGroupDetailReviewViewModel, OperationLogSave>()
                    .ForMember(dst => dst.ActionType, opt => opt.MapFrom(src => ActionType.AccountantSignQuery))                    
                    .ForMember(dst => dst.AccountantName, opt => opt.MapFrom(src => src.Name))                    
                    .ForMember(dst => dst.AccountantSignGroupId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.AccountantSignGroupCreateDate, opt => opt.MapFrom(src => src.GroupCreateDate));

            //印鑑異動建檔紀錄存檔Map
            CreateMap<CustomerSealGroup, CustomerSealEventLogSave>()                    
                    .ForMember(dst => dst.CustomerId, opt => opt.MapFrom(src => src.Customer.Id))
                    .ForMember(dst => dst.CustomerCode, opt => opt.MapFrom(src => src.Customer.Code))
                    .ForMember(dst => dst.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))                    
                    .ForMember(dst => dst.CustomerSealGroupId, opt => opt.MapFrom(src => src.Id))                    
                    .ForMember(dst => dst.QuarterYearId, opt => opt.MapFrom(src => src.QuarterYear.Id))
                    .ForMember(dst => dst.GregorainQuarterYear, opt => opt.MapFrom(src =>
                            src.TypographyType == TypographyType.FinancialReport ?
                            QuarterUtil.GetGregorainQuarter(src.QuarterYear) : QuarterUtil.GetGregorainYear(src.QuarterYear)
                    ))
                    .ForMember(dst => dst.DisplayQuarterYear, opt => opt.MapFrom(src =>
                            src.TypographyType == TypographyType.FinancialReport ?
                            QuarterUtil.GetTaiwanYearQuarter(src.QuarterYear) : QuarterUtil.GetTaiwanYear(src.QuarterYear)
                    ));

            //會計師簽印異動建檔紀錄存檔Map
            CreateMap<AccountantSignGroup, AccountantSignEventLogSave>()
                    .ForMember(dst => dst.AccountantId, opt => opt.MapFrom(src => src.Accountant.Id))
                    .ForMember(dst => dst.AccountantCode, opt => opt.MapFrom(src => src.Accountant.Code))
                    .ForMember(dst => dst.AccountantName, opt => opt.MapFrom(src => src.Accountant.Name))
                    .ForMember(dst => dst.AccountantSignGroupId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.AccountantSignGroupCreateDate, opt => opt.MapFrom(src => src.CreateDate));

            //印鑑異動紀錄Map(分頁顯示)
            CreateMap<CustomerSealEventLog, CustomerSealEventLogViewModel>()
                    .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Data!.CustomerCode))
                    .ForMember(dst => dst.ReviewStatus, opt => opt.MapFrom(src => src.Data!.ReviewStatus))
                    .ForMember(dst => dst.CustomerName, opt => opt.MapFrom(src => src.Data!.CustomerName))
                    .ForMember(dst => dst.DisplayQuarterYear, opt => opt.MapFrom(src => src.Data!.DisplayQuarterYear));

            //印鑑異動紀錄Map(分頁顯示)
            CreateMap<AccountantSignEventLog, AccountantSignEventLogViewModel>()
                    .ForMember(dst => dst.AccountantCode, opt => opt.MapFrom(src => src.Data!.AccountantCode))
                    .ForMember(dst => dst.ReviewStatus, opt => opt.MapFrom(src => src.Data!.ReviewStatus))
                    .ForMember(dst => dst.AccountantName, opt => opt.MapFrom(src => src.Data!.AccountantName));


            //會計師成員列表
            CreateMap<Accountant, AccountantMemberViewModel>()
                    .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.Code))
                    .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                    //.ForMember(dst => dst.Groups, opt => opt.MapFrom(src =>
                    //        //logReportQueryValue 目前在這裡當作AccountantGroupId來搜尋
                    //        logReportQueryValue == 0 ?
                    //        src.AccountantGroups.Select(group => group.Name).FirstOrDefault() ?? string.Empty :
                    //        src.AccountantGroups.Where(group => group.Id == logReportQueryValue).Select(group => group.Name).FirstOrDefault() ?? string.Empty
                    //))
                    .ForMember(dst => dst.Groups, opt => opt.MapFrom(src => src.AccountantGroups.Select(x => x.Name)));

            CreateMap<UserRepresentation, UserMemberViewModel>()
                .ForMember(dst => dst.CreatedDate, opt => opt.MapFrom(src => src.CreatedTimestamp.HasValue ?
                DateTimeOffset.FromUnixTimeMilliseconds(src.CreatedTimestamp.Value).DateTime.ToLocalTime() : (DateTime?)null ));
            
        }
    }
}
