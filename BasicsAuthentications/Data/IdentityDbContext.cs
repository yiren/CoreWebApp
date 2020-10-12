using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BasicsAuthentications.Data
{
    /// <summary>
    /// 1.IdentityDbContext is a special DbContext that contains IdentityUser and IdentityRole implmentations.
    /// 2.DbContextOptions param is requried to initialize DB connection.
    /// </summary>
    public class UserDbContext:IdentityDbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options):base(options)
        {
            
        }
        
    }
}