using EmployeeApp.Data.Data;
using EmployeeApp.Data.Interfaces.UserRepo;
using EmployeeApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Interfaces.ProgramRepo
{
    public class ProgramRepository:IProgramRepository
    {
        private readonly EmployeeDB2Context _context;
        private readonly ILogger<UserRepository> _logger;
        public ProgramRepository(EmployeeDB2Context context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IEnumerable<Program>> GetAllProgramsAsync()
        {
            return await _context.Programs.Include(x=>x.Sections).ToListAsync();
        }

        public async Task<Program> GetProgramByIdAsync(int id)
        {
            return await _context.Programs.Include(x =>x.Sections).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddProgramAsync(Program program)
        {
            await _context.Programs.AddAsync(program);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProgramAsync(Program program)
        {
            _context.Programs.Update(program);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProgramAsync(int id)
        {
            var program = await GetProgramByIdAsync(id);
            if (program != null)
            {
                _context.Programs.Remove(program);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ProgramExistsAsync(int id)
        {
            return await _context.Programs.Include(x => x.Sections).AnyAsync(p => p.Id == id);
        }

        

        public async Task AddProgramsAsync(List<Program> programs)
        {
           await _context.Programs.AddRangeAsync(programs);
           await _context.SaveChangesAsync();
        }
    }
}
