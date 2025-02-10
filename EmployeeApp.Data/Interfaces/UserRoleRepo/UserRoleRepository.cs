using EmployeeApp.Data.Data;
using EmployeeApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Interfaces.UserRoleRepo
{
    public class UserRoleRepository:IUserRoleRepository
    {
        private readonly EmployeeDB2Context _context;

        public UserRoleRepository(EmployeeDB2Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserRole>> GetAllAsync()
        {
            return await _context.UserRoles
                .Include(ur => ur.user)  // Include related User
                .Include(ur => ur.role)  // Include related Role
                .ToListAsync();
        }

        public async Task<UserRole> GetByIdAsync(int id)
        {
            return await _context.UserRoles
                .Include(ur => ur.user)  // Include related User
                .Include(ur => ur.role)  // Include related Role
                .FirstOrDefaultAsync(ur => ur.UserId == id);
        }

        public async Task<UserRole> AddAsync(UserRole userRole)
        {
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
            return userRole;
        }

        public async Task<UserRole> UpdateAsync(UserRole userRole)
        {
            var existingUserRole = await _context.UserRoles.FindAsync(userRole.Id);

            if (existingUserRole == null)
            {
                return null;
            }

            _context.Entry(existingUserRole).CurrentValues.SetValues(userRole);
            await _context.SaveChangesAsync();
            return existingUserRole;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var userRole = await _context.UserRoles.FindAsync(id);

            if (userRole == null)
            {
                return false;
            }

            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
