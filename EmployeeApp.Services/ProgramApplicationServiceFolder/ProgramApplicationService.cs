using EmployeeApp.Data.Interfaces.ProgramApplicationRepo;
using EmployeeApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.ProgramApplicationServiceFolder
{
    public class ProgramApplicationService:IProgramApplicationService
    {
        private readonly IProgramApplicationRepository _programApplicationRepository;

        public ProgramApplicationService(IProgramApplicationRepository programApplicationRepository)
        {
            _programApplicationRepository = programApplicationRepository;
        }

        public async Task<IEnumerable<ProgramApplication>> GetAllProgramApplicationsAsync()
        {
            return await _programApplicationRepository.GetAllAsync();
        }

        public async Task<ProgramApplication> GetProgramApplicationByIdAsync(int id)
        {
            return await _programApplicationRepository.GetByIdAsync(id);
        }

        public async Task<ProgramApplication> AddProgramApplicationAsync(ProgramApplication programApplication)
        {
            return await _programApplicationRepository.AddAsync(programApplication);
        }

        public async Task<ProgramApplication> UpdateProgramApplicationAsync(ProgramApplication programApplication)
        {
            return await _programApplicationRepository.UpdateAsync(programApplication);
        }

        public async Task<bool> DeleteProgramApplicationAsync(int id)
        {
            return await _programApplicationRepository.DeleteAsync(id);
        }
        public async Task<List<ProgramApplication>> GetProgramApplicationsByUserId(int userId)
        {

            return await _programApplicationRepository.GetProgramApplicationsByUserId(userId);
        }
    }
}
