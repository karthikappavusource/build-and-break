using EmployeeApp.Data.Interfaces.StatusRepo;
using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.StatusServiceFolder
{
    public class StatusService:IStatusService
    {
        private readonly IStatusRepository _statusRepository;

        public StatusService(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public async Task<IEnumerable<Status>> GetAllStatusesAsync()
        {
            return await _statusRepository.GetAllAsync();
        }

        public async Task<Status> GetStatusByIdAsync(int id)
        {
            return await _statusRepository.GetByIdAsync(id);
        }

        public async Task<Status> AddStatusAsync(Status status)
        {
            return await _statusRepository.AddAsync(status);
        }

        public async Task<Status> UpdateStatusAsync(Status status)
        {
            return await _statusRepository.UpdateAsync(status);
        }

        public async Task<bool> DeleteStatusAsync(int id)
        {
            return await _statusRepository.DeleteAsync(id);
        }

        public async Task<Status> GetByNameAsync(string name)
        {
            return await _statusRepository.GetByNameAsync(name);
        }
    }
}
