using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.RoleServiceFolder
{
    public interface IRoleService
    {
        Task<Role> CreateRole(Role role);
        Task<Role> GetRoleById(int id);
        Task<IEnumerable<Role>> GetAllRoles();
        Task<Role> UpdateRole(Role role);
        Task<bool> DeleteRole(int id);
    }
}
