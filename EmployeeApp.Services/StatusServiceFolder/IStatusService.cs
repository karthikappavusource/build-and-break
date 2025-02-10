using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.StatusServiceFolder
{
    public interface IStatusService
    {
        Task<IEnumerable<Status>> GetAllStatusesAsync();
        Task<Status> GetStatusByIdAsync(int id);
        Task<Status> GetByNameAsync(string name);
        Task<Status> AddStatusAsync(Status status);
        Task<Status> UpdateStatusAsync(Status status);
        Task<bool> DeleteStatusAsync(int id);
    }
}
