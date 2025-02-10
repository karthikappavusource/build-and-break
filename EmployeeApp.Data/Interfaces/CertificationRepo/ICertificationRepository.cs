using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Interfaces.CertificationRepo
{
    public interface ICertificationRepository
    {
        Task<Certification> Create(Certification certification);
        Task<Certification> Update(Certification certification);
        Task<bool> Delete(int id);
        Task<Certification> Get(int id);
        Task<List<Certification>> GetCertificationsByUserId(int userId);

    }
}
