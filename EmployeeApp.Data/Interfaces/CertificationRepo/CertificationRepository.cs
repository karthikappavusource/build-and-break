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

namespace EmployeeApp.Data.Interfaces.CertificationRepo
{
    public class CertificationRepository : ICertificationRepository
    {
        private readonly EmployeeDB2Context _context;
        private readonly ILogger<UserRepository> _logger;
        public CertificationRepository(EmployeeDB2Context context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Certification> Create(Certification certification)
        {
            await _context.Certifications.AddAsync(certification);
            await _context.SaveChangesAsync();
            return certification;
        }

        public async Task<bool> Delete(int id)
        {
            var cert=await _context.Certifications.FindAsync(id);
            _context.Certifications.Remove(cert);
            var deletedobj = _context.Certifications.FirstOrDefault(m => m.Id == id);
            if (deletedobj == null) return true;
            else return false;
        }

        public async Task<Certification> Get(int id)
        {
            return await _context.Certifications.Include(x => x.status).FirstOrDefaultAsync(l => l.Id == id);
            

        }

        public async Task<List<Certification>> GetCertificationsByUserId(int userId)
        {
            
            return await _context.Certifications.Select(x => x).Include(x => x.status).Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Certification> Update(Certification certification)
        {
            _context.Certifications.Update(certification);
            await _context.SaveChangesAsync();
            return certification;
        }
    }
}
