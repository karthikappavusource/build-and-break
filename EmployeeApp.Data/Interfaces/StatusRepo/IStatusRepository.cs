using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Interfaces.StatusRepo
{
    public interface IStatusRepository
    {
        Task<IEnumerable<Status>> GetAllAsync();
        Task<Status> GetByIdAsync(int id);
        Task<Status> AddAsync(Status status);
        Task<Status> UpdateAsync(Status status);
        Task<bool> DeleteAsync(int id);
        Task<Status> GetByNameAsync(string name);
    }
}
