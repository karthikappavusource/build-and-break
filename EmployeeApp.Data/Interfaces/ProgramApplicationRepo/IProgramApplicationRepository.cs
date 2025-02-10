using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Interfaces.ProgramApplicationRepo
{
    public interface IProgramApplicationRepository
    {
        Task<IEnumerable<ProgramApplication>> GetAllAsync();
        Task<ProgramApplication> GetByIdAsync(int id);
        Task<ProgramApplication> AddAsync(ProgramApplication programApplication);
        Task<ProgramApplication> UpdateAsync(ProgramApplication programApplication);
        Task<bool> DeleteAsync(int id);
        Task<List<ProgramApplication>> GetProgramApplicationsByUserId(int userId);
    }
}
