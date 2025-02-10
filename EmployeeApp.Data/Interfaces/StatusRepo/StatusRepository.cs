using EmployeeApp.Data.Data;
using EmployeeApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Interfaces.StatusRepo
{
    public class StatusRepository:IStatusRepository
    {
        private readonly EmployeeDB2Context _context;

        public StatusRepository(EmployeeDB2Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Status>> GetAllAsync()
        {
            return await _context.Statuses.ToListAsync();
        }

        public async Task<Status> GetByIdAsync(int id)
        {
            return await _context.Statuses.FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<Status> GetByNameAsync(string name)
        {
           var result= _context.Statuses.FirstOrDefault(x => x.Name == name);
            return result;
        }

        public async Task<Status> AddAsync(Status status)
        {
            _context.Statuses.Add(status);
            await _context.SaveChangesAsync();
            return status;
        }

        public async Task<Status> UpdateAsync(Status status)
        {
            var existingStatus = await _context.Statuses.FindAsync(status.Id);

            if (existingStatus == null)
            {
                return null;
            }

            _context.Entry(existingStatus).CurrentValues.SetValues(status);
            await _context.SaveChangesAsync();
            return existingStatus;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var status = await _context.Statuses.FindAsync(id);

            if (status == null)
            {
                return false;
            }

            _context.Statuses.Remove(status);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
