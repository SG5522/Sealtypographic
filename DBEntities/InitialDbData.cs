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
            if (!dbContext.AccountantGroups.Any())
            {
                //建立DB前先建置AccountantGroup無群組資料
                AccountantGroup accountantGroup = new()
                {
                    Id = 1,
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
