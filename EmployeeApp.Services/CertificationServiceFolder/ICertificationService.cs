using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.CertificationServiceFolder
{
    public interface ICertificationService
    {
        Task<Certification> CreateCertificationAsync(Certification certification);
        Task<bool> DeleteCertificationAsync(int id);
        Task<Certification> GetCertificationByIdAsync(int id);
        Task<Certification> UpdateCertificationAsync(Certification certification);
        Task<List<Certification>> GetCertificationsByUserId(int userId);
    }
}
