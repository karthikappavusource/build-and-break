using EmployeeApp.Data.HelperModels;
using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Interfaces.LeaveRepo
{
    public interface ILeaveRepository
    {
        Task<Leave> Create(Leave leave);
        Task<Leave> Update(Leave leave);
        Task<bool> Delete(int id);
        Task<Leave> Get(int id);
        Task<IEnumerable<Leave>> GetLeavesByUserId(int userId);
        Task<IEnumerable<Leave>> UpcomingLeaves();
    }
}
