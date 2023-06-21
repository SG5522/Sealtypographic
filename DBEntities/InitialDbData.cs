using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBEntities
{
    public class InitialDbData : DbContext
    {
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
                dbContext.SaveChanges();
            }

            if (!dbContext.Quarters.Any())
            {
                List<Quarter> quarters = new();
                int nowGregorianYear = 2023;
                int years = 50;
                for (int i = 1; i <= years; i++)
                {
                    string gregorianYear = $"{nowGregorianYear - years + i}";
                    string taiwanYear = $"{nowGregorianYear - 1911 - years + i}";
                    for (int period = 1; period <= 4; period++)
                    {
                        Quarter quarter = new()
                        {
                            GregorianYear = gregorianYear,
                            TaiwanYear = taiwanYear,
                            Period = $"Q{period}"
                        };
                        quarters.Add(quarter);
                    }
                }
                dbContext.Quarters.AddRange(quarters);
                dbContext.SaveChanges();
            }

            if (!dbContext.AccountantGroups.Any())
            {
                //建立DB前先建置AccountantGroup無群組資料
                AccountantGroup accountantGroup = new()
                {
                    Id = 1,
                    Company = dbContext.Companys.Single(x => x.Id == 1),
                    AccountantGroupNumber = "Default",
                    CreateUserId = 0,
                    UpdateUserId = 0,
                    DeleteStatus = 0,
                    Name = "預設群組"
                };
                dbContext.AccountantGroups.Add(accountantGroup);
                dbContext.SaveChanges();
            }
        }
    }
}
