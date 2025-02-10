using EmployeeApp.Data.Data;
using EmployeeApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Interfaces.ProgramApplicationRepo
{
    public class ProgramApplicationRepository:IProgramApplicationRepository
    {
        private readonly EmployeeDB2Context _context;

        public ProgramApplicationRepository(EmployeeDB2Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProgramApplication>> GetAllAsync()
        {
            return await _context.Set<ProgramApplication>()
                .Include(pa => pa.program)  // Include related Program
                .Include(pa => pa.user)     // Include related User
                .ToListAsync();
        }

        public async Task<ProgramApplication> GetByIdAsync(int id)
        {
            return await _context.Set<ProgramApplication>()
                .Include(pa => pa.program)  // Include related Program
                .Include(pa => pa.user)     // Include related User
                .FirstOrDefaultAsync(pa => pa.Id == id);
        }

        public async Task<ProgramApplication> AddAsync(ProgramApplication programApplication)
        {
            _context.Set<ProgramApplication>().Add(programApplication);
            await _context.SaveChangesAsync();
            return programApplication;
        }

        public async Task<ProgramApplication> UpdateAsync(ProgramApplication programApplication)
        {
            var existingProgramApplication = await _context.Set<ProgramApplication>().FindAsync(programApplication.Id);

            if (existingProgramApplication == null)
            {
                return null;
            }

            _context.Entry(existingProgramApplication).CurrentValues.SetValues(programApplication);
            await _context.SaveChangesAsync();
            return existingProgramApplication;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var programApplication = await _context.Set<ProgramApplication>().FindAsync(id);

            if (programApplication == null)
            {
                return false;
            }

            _context.Set<ProgramApplication>().Remove(programApplication);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<ProgramApplication>> GetProgramApplicationsByUserId(int userId)
        {
               
            return _context.ProgramApplications.Include(x=>x.program).Include(x=>x.user).Select(x => x).Where(x => x.ApplicantId == userId).ToList();
        }

    }
}
