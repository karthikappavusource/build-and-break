using EmployeeApp.Data.Data;
using Microsoft.AspNetCore.Authentication;
using SQLitePCL;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
namespace EmployeeApp.PortalWithAuth
{
    public class CustomClaimsTransformation : IClaimsTransformation
    {
        private readonly EmployeeDB2Context _context;
        private readonly ILogger<CustomClaimsTransformation> _logger;
        public CustomClaimsTransformation(EmployeeDB2Context context, ILogger<CustomClaimsTransformation> logger)
        {
            _context = context;
            _logger = logger;

        }
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {

            var email = principal.FindFirst(ClaimTypes.Email).Value;
            var doesEmailExist = _context.Users.FirstOrDefault(u => u.Email == email);
            if (doesEmailExist == null)
            {
                var claimsIdentity = principal.Identity as ClaimsIdentity;
                claimsIdentity.AddClaim(new Claim("role", "Client"));
                return Task.FromResult(principal);
            }
            else
            {
                var claimsIdentity = principal.Identity as ClaimsIdentity;
                var userWithRole = (from user in _context.Users
                                    join userRole in _context.UserRoles on user.Id equals userRole.UserId
                                    join role in _context.Roles on userRole.RoleId equals role.Id
                                    where user.Email == email
                                    select new { UserId = user.Id, Role = role.RoleName }).FirstOrDefault();
                var userId = userWithRole.UserId;
                var roleName = userWithRole.Role;
                claimsIdentity.AddClaim(new Claim("role", roleName));
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, doesEmailExist.Name));
                claimsIdentity.AddClaim(new Claim("Id", doesEmailExist.Id.ToString()));
                return Task.FromResult(principal);
            }
             
        }
    }

}
