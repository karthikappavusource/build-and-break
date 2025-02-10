using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Interfaces.RoleRepo
{
    public interface IRoleRepository
    {
        Task<Role> Create(Role role);
        Task<Role> Get(int id);
        Task<IEnumerable<Role>> GetAll();
        Task<Role> Update(Role role);
        Task<bool> Delete(int id);
    }
}
