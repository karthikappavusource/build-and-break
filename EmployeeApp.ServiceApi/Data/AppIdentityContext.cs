using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace EmployeeApp.ServiceApi.Data
{
    public class AppIdentityContext:IdentityDbContext
    {
        public AppIdentityContext(DbContextOptions<AppIdentityContext> options)
         : base(options)
        {
        }
    }
}
