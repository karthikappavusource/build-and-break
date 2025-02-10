using EmployeeApp.Data.Data;
using EmployeeApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Interfaces.SectionRepo
{
    public class SectionRepository:ISectionRepository
    {
        private readonly EmployeeDB2Context _context;

        public SectionRepository(EmployeeDB2Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Section>> GetAllAsync()
        {
            return await _context.Set<Section>()
                .Include(s => s.Program)  // Include related Program
                .Include(s => s.Topics)   // Include related Topics
                .ToListAsync();
        }

        public async Task<Section> GetByIdAsync(int id)
        {
            return await _context.Set<Section>()
                .Include(s => s.Program)  // Include related Program
                .Include(s => s.Topics)   // Include related Topics
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Section> AddAsync(Section section)
        {
            _context.Set<Section>().Add(section);
            await _context.SaveChangesAsync();
            return section;
        }

        public async Task<Section> UpdateAsync(Section section)
        {
            var existingSection = await _context.Set<Section>().FindAsync(section.Id);

            if (existingSection == null)
            {
                return null;
            }

            _context.Entry(existingSection).CurrentValues.SetValues(section);
            await _context.SaveChangesAsync();
            return existingSection;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var section = await _context.Set<Section>().FindAsync(id);

            if (section == null)
            {
                return false;
            }

            _context.Set<Section>().Remove(section);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task AddSectionsAsync(List<Section> sections)
        {
            await _context.Sections.AddRangeAsync(sections);
            await _context.SaveChangesAsync();
        }
    }
}
