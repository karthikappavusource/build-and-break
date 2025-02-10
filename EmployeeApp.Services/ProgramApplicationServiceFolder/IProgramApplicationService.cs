using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.ProgramApplicationServiceFolder
{
    public interface IProgramApplicationService
    {
        Task<IEnumerable<ProgramApplication>> GetAllProgramApplicationsAsync();
        Task<ProgramApplication> GetProgramApplicationByIdAsync(int id);
        Task<ProgramApplication> AddProgramApplicationAsync(ProgramApplication programApplication);
        Task<ProgramApplication> UpdateProgramApplicationAsync(ProgramApplication programApplication);
        Task<bool> DeleteProgramApplicationAsync(int id);
        Task<List<ProgramApplication>> GetProgramApplicationsByUserId(int userId);
    }
}
