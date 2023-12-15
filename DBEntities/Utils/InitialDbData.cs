using DBEntities.Consts;
using DBEntities.Entities;
using DBEntities.Entities.AccountantModels;
using DBEntities.Entities.ImageRangeModels;

namespace DBEntities.Utils
{
    public class InitialDbData
    {
        private const int Years = 50;

        public static void Initialize(SealTypographicDbContext dbContext)
        {
            try
            {
                if (!dbContext.ApplicationUsers.Any())
                {
                    //建立User資料
                    ApplicationUser user = new()
                    {
                        Id = 1,
                        UserName = "admin",                        
                        CreateDate = DateTime.Now
                    };
                    dbContext.ApplicationUsers.Add(user);
                    dbContext.SaveChanges();
                }

                if (!dbContext.Companys.Any())
                {
                    //公司基本資料
                    Company company = new()
                    {
                        Id = 1,
                        Code = "AAA001",
                        BAN = "12345678",
                        Name = "映像有限公司",
                        AccountantGroups = new List<AccountantGroup>(),
                        ApplicationUsers = new List<ApplicationUser>(),
                        ImageRangeSettings = new List<ImageRangeSetting>()
                    };

                    //建立User資料
                    ApplicationUser user = new()
                    {
                        UserName = "ImageAdmin",                        
                        CreateDate = DateTime.Now
                    };

                    //建立DB前先建置AccountantGroup無群組資料
                    AccountantGroup accountantGroup = new()
                    {
                        Id = 1,
                        Code = "Default",
                        Name = "預設群組"
                    };
                    //客戶印鑑截取設定
                    ImageRangeSetting imageCaptureWithCustomer = new()
                    {
                        PageSize = PageSize.A4,
                        PaperOrientation = PapeOrientation.Portrait,
                        ImageRangeLocations = new List<ImageRangeLocation>()
                        {
                            new ()
                            {
                                Left = 0,
                                Top = 0,
                                Width = 400,
                                Height = 400,
                                SealType = SealType.Customer,
                                SubSealType = SubSealType.Company
                            },
                            new ()
                            {
                                Left = 0,
                                Top = 400,
                                Width = 400,
                                Height = 400,
                                SealType = SealType.Customer,
                                SubSealType = SubSealType.President
                            },
                            new ()
                            {
                                Left = 0,
                                Top = 400,
                                Width = 400,
                                Height = 400,
                                SealType = SealType.Customer,
                                SubSealType = SubSealType.Manager
                            },
                            new ()
                            {
                                Left = 400,
                                Top = 400,
                                Width = 400,
                                Height = 400,
                                SealType = SealType.Customer,
                                SubSealType = SubSealType.AccountingDirector
                            },
                            new ()
                            {
                                Left = 0,
                                Top = 0,
                                Width = 0,
                                Height = 0,
                                SealType = SealType.Customer,
                                SubSealType = SubSealType.Other
                            }
                        }
                    };
                    //會計師簽印截取設定
                    ImageRangeSetting imageCaptureWithAccountant = new()
                    {
                        PageSize = PageSize.A4,
                        PaperOrientation = PapeOrientation.Portrait,
                        ImageRangeLocations = new List<ImageRangeLocation>()
                        {
                            new ()
                            {
                                Left = 0,
                                Top = 0,
                                Width = 400,
                                Height = 400,
                                SealType = SealType.Accountant,
                                SubSealType = SubSealType.Seal
                            },
                            new ()
                            {
                                Left = 0,
                                Top = 400,
                                Width = 400,
                                Height = 400,
                                SealType = SealType.Accountant,
                                SubSealType = SubSealType.CHSign
                            },
                            new ()
                            {
                                Left = 0,
                                Top = 400,
                                Width = 400,
                                Height = 400,
                                SealType = SealType.Accountant,
                                SubSealType = SubSealType.ENSign
                            },
                            new ()
                            {
                                Left = 400,
                                Top = 400,
                                Width = 400,
                                Height = 400,
                                SealType = SealType.Accountant,
                                SubSealType = SubSealType.OldSign
                            },
                            new ()
                            {
                                Left = 0,
                                Top = 0,
                                Width = 0,
                                Height = 0,
                                SealType = SealType.Accountant,
                                SubSealType = SubSealType.Other
                            }
                        }
                    };
                    InputUtil.Set(company, true, 1);
                    InputUtil.Set(imageCaptureWithCustomer, true, 1);
                    InputUtil.Set(imageCaptureWithAccountant, true, 1);
                    InputUtil.Set(accountantGroup, true, 1);

                    company.AccountantGroups.Add(accountantGroup);
                    company.ApplicationUsers.Add(user);
                    company.ImageRangeSettings!.Add(imageCaptureWithCustomer);
                    company.ImageRangeSettings!.Add(imageCaptureWithAccountant);


                    dbContext.Companys.Add(company);
                }

                if (!dbContext.QuarterYears.Any())
                {
                    List<QuarterYear> quarterYears = new();
                    int nowGregorianYear = DateTime.Now.Year;
                    for (int i = 0; i <= Years; i++)
                    {
                        int gregorianYear = nowGregorianYear - Years + i;

                        //(財報季度列表)
                        for (int period = 1; period <= 4; period++)
                        {
                            QuarterYear quarter = new()
                            {
                                GregorianYear = gregorianYear,
                                Period = $"Q{period}",
                                Type = TypographyType.FinancialReport
                            };
                            quarterYears.Add(quarter);
                        }

                        //(稅報年度列表)
                        QuarterYear quarterYear = new()
                        {
                            GregorianYear = gregorianYear,
                            Type = TypographyType.TaxReport
                        };
                        quarterYears.Add(quarterYear);
                    }
                    dbContext.QuarterYears.AddRange(quarterYears);
                }

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
