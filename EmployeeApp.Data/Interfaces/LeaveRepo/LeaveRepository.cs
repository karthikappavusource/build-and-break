using EmployeeApp.Data.Data;
using EmployeeApp.Data.HelperModels;
using EmployeeApp.Data.Interfaces.UserRepo;
using EmployeeApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Interfaces.LeaveRepo
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly EmployeeDB2Context _context;
        private readonly ILogger<UserRepository> _logger;
        public LeaveRepository(EmployeeDB2Context context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Leave> Create(Leave leave)
        {
            await _context.Leaves.AddAsync(leave);
            await _context.SaveChangesAsync();
            return leave;
        }

        public async Task<bool> Delete(int id)
        {
            var leave=await _context.Leaves.FindAsync(id);
            _context.Leaves.Remove(leave);
            _context.SaveChanges();
            var deletedobj = _context.Leaves.FirstOrDefault(m => m.Id == id);
            if (deletedobj == null) return true;
            else return false;
        }

        public async Task<Leave> Get(int id)
        {
            return await _context.Leaves.FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Leave>> GetLeavesByUserId(int userId)
        {
            return await _context.Leaves.Where(x => x.UserId == userId).Include(x => x.status).Select(x => x).ToListAsync();
        }

        public async Task<IEnumerable<Leave>> UpcomingLeaves()
        {
            DateTime today = DateTime.Today;
            DateTime twentyDaysFromNow = today.AddDays(20);
            var upcomingLeaves=_context.Leaves.
                Where(x=>x.user.role.role.RoleName=="Associate"&&x.From>=today&&x.From<=twentyDaysFromNow)
                .OrderBy(x => x.From)
                .Select(x => x)
                .ToList();

            List<LeaveUserRole> result =_context.Leaves
            .Join(
                _context.Users,
                userLeave => userLeave.UserId,
                user => user.Id,
                (userLeave, user) => new { userLeave, user }
            )
            .Join(
                _context.UserRoles,
                userUserLeave => userUserLeave.user.Id,
                userRole => userRole.UserId,
                (userUserLeave, userRole) => new { userUserLeave.userLeave, userUserLeave.user, userRole }
            )
            .Join(
                _context.Roles,
                userRoleUser => userRoleUser.userRole.RoleId,
                role => role.Id,
                (userRoleUser, role) => new { userRoleUser.userLeave, userRoleUser.user, role }
            )
            .Where(x => x.role.RoleName == "Associate" && x.userLeave.From >= today && x.userLeave.From <= twentyDaysFromNow)
            .OrderBy(x => x.userLeave.From)
            .Select(x => new LeaveUserRole
            {
                leave=x.userLeave,
                user=x.user,
                role=x.role,
            })
            .ToList();
            return upcomingLeaves;
        }

        public async Task<Leave> Update(Leave leave)
        {
             _context.Leaves.Update(leave);
            await _context.SaveChangesAsync();
            return leave;
        }
    }
}
