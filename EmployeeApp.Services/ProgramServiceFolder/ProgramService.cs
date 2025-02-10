using EmployeeApp.Data.Interfaces.ProgramRepo;
using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.ProgramServiceFolder
{
    public class ProgramService:IProgramService
    {
        private readonly IProgramRepository _programRepository;

        public ProgramService(IProgramRepository programRepository)
        {
            _programRepository = programRepository;
        }

        public async Task<IEnumerable<Program>> GetAllProgramsAsync()
        {
            return await _programRepository.GetAllProgramsAsync();
        }

        public async Task<Program> GetProgramByIdAsync(int id)
        {
            return await _programRepository.GetProgramByIdAsync(id);
        }

        public async Task AddProgramAsync(Program program)
        {
            await _programRepository.AddProgramAsync(program);
        }

        public async Task UpdateProgramAsync(Program program)
        {
            await _programRepository.UpdateProgramAsync(program);
        }

        public async Task DeleteProgramAsync(int id)
        {
            await _programRepository.DeleteProgramAsync(id);
        }

        public async Task<bool> ProgramExistsAsync(int id)
        {
            return await _programRepository.ProgramExistsAsync(id);
        }

        public async Task AddProgramsAsync(List<Program> programs)
        {
             await _programRepository.AddProgramsAsync(programs);
        }
    }
}
