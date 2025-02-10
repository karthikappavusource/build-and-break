using EmployeeApp.Data.HelperModels;
using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.LeaveServiceFolder
{
    public interface ILeaveService
    {
        Task<Leave> CreateLeaveAsync(Leave leave);
        Task<bool> DeleteLeaveAsync(int id);
        Task<Leave> GetLeaveByIdAsync(int id);
        Task<Leave> UpdateLeaveAsync(Leave leave);
        Task<IEnumerable<Leave>> GetLeavesByUserId(int userId);
        Task<IEnumerable<Leave>> UpcomingLeaves();
    }
}
