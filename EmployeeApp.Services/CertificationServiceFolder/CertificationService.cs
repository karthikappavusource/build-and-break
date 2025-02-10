using EmployeeApp.Data.Interfaces.CertificationRepo;
using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.CertificationServiceFolder
{
    public class CertificationService:ICertificationService
    {
        private readonly ICertificationRepository _certificationRepository;

        public CertificationService(ICertificationRepository certificationRepository)
        {
            _certificationRepository = certificationRepository;
        }

        public async Task<Certification> CreateCertificationAsync(Certification certification)
        {
            return await _certificationRepository.Create(certification);
        }

        public async Task<bool> DeleteCertificationAsync(int id)
        {
            return await _certificationRepository.Delete(id);
        }

        public async Task<Certification> GetCertificationByIdAsync(int id)
        {
            return await _certificationRepository.Get(id);
        }

        public async Task<List<Certification>> GetCertificationsByUserId(int userId)
        {
            return await _certificationRepository.GetCertificationsByUserId(userId);
        }

        public async Task<Certification> UpdateCertificationAsync(Certification certification)
        {
            return await _certificationRepository.Update(certification);
        }
    }
}
