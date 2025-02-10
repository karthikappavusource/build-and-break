using EmployeeApp.Data.Interfaces.RoleRepo;
using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.RoleServiceFolder
{
    public class RoleService:IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Role> CreateRole(Role role)
        {
            return await _roleRepository.Create(role);
        }

        public async Task<Role> GetRoleById(int id)
        {
            return await _roleRepository.Get(id);
        }

        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await _roleRepository.GetAll();
        }

        public async Task<Role> UpdateRole(Role role)
        {
            return await _roleRepository.Update(role);
        }

        public async Task<bool> DeleteRole(int id)
        {
            return await _roleRepository.Delete(id);
        }
    }
}

