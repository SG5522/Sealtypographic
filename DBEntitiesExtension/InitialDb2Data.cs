using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBEntitiesExtension
{
    public class InitialDb2Data : DbContext
    {
        public static void Initialize(SealTypographicExtensionDbContext dbContext)
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

            if (!dbContext.AccountantGroups.Any())
            {
                //建立DB前先建置AccountantGroup無群組資料
                AccountantGroup accountantGroup = new()
                {
                    Id = 1,       
                    Company = dbContext.Companys.Single(x => x.Id == 1),
                    AccountantGroupNumber = "",
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
