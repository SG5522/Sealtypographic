using DBEntities.Consts;
using Microsoft.EntityFrameworkCore;

namespace DBEntities
{
    public class InitialDbData : DbContext
    {
        private const int Years = 50;

        public static void Initialize(SealTypographicDbContext dbContext)
        {
            if (!dbContext.Companys.Any())
            {
                Company company = new()
                {
                    Id = 1,
                    Code = "AAA001",
                    BAN = "12345678",
                    Name = "映像有限公司"
                };
                dbContext.Companys.Add(company);                
            }

            if (!dbContext.QuarterYears.Any())
            {
                List<QuarterYear> quarterYears = new();
                int nowGregorianYear = DateTime.Now.Year;                
                for (int i = 0 ; i <= Years; i++)
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

            if (!dbContext.AccountantGroups.Any())
            {
                //建立DB前先建置AccountantGroup無群組資料
                AccountantGroup accountantGroup = new()
                {
                    Id = 1,
                    Company = dbContext.Companys.Single(x => x.Id == 1),
                    Code = "Default",
                    CreateUserId = 0,
                    UpdateUserId = 0,
                    DeleteStatus = 0,
                    Name = "預設群組"
                };
                dbContext.AccountantGroups.Add(accountantGroup);                
            }

            if(!dbContext.Users.Any())
            {
                User user = new()
                {
                    Id = 1,
                    Company = dbContext.Companys.Single(x => x.Id == 1),
                    UserName = "Admin",
                    KeycloakUserId = "00001"
                };
                dbContext.Users.Add(user);
            }
            dbContext.SaveChanges();
        }
    }
}
