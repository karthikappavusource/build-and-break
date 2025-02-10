using EmployeeApp.Data.Interfaces.UserRoleRepo;
using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.UserRoleServiceFolder
{
    public class UserRoleService:IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task<IEnumerable<UserRole>> GetAllUserRolesAsync()
        {
            return await _userRoleRepository.GetAllAsync();
        }

        public async Task<UserRole> GetUserRoleByIdAsync(int id)
        {
            return await _userRoleRepository.GetByIdAsync(id);
        }

        public async Task<UserRole> AddUserRoleAsync(UserRole userRole)
        {
            return await _userRoleRepository.AddAsync(userRole);
        }

        public async Task<UserRole> UpdateUserRoleAsync(UserRole userRole)
        {
            return await _userRoleRepository.UpdateAsync(userRole);
        }

        public async Task<bool> DeleteUserRoleAsync(int id)
        {
            return await _userRoleRepository.DeleteAsync(id);
        }
    }
}
