using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.ProgramServiceFolder
{
    public interface IProgramService
    {
        Task<IEnumerable<Program>> GetAllProgramsAsync();
        Task<Program> GetProgramByIdAsync(int id);
        Task AddProgramAsync(Program program);
        Task AddProgramsAsync(List<Program> programs);
        Task UpdateProgramAsync(Program program);
        Task DeleteProgramAsync(int id);
        Task<bool> ProgramExistsAsync(int id);
    }
}
