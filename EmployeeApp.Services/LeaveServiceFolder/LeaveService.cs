using EmployeeApp.Data.HelperModels;
using EmployeeApp.Data.Interfaces.LeaveRepo;
using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.LeaveServiceFolder
{
    public class LeaveService:ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;

        public LeaveService(ILeaveRepository leaveRepository)
        {
            _leaveRepository = leaveRepository;
        }

        public async Task<Leave> CreateLeaveAsync(Leave leave)
        {
            return await _leaveRepository.Create(leave);
        }

        public async Task<bool> DeleteLeaveAsync(int id)
        {
            return await _leaveRepository.Delete(id);
        }

        public async Task<Leave> GetLeaveByIdAsync(int id)
        {
            return await _leaveRepository.Get(id);
        }

        public async Task<IEnumerable<Leave>> GetLeavesByUserId(int userId)
        {
            return await _leaveRepository.GetLeavesByUserId(userId);
        }

        public async Task<IEnumerable<Leave>> UpcomingLeaves()
        {
            return await _leaveRepository.UpcomingLeaves();
        }

        public async Task<Leave> UpdateLeaveAsync(Leave leave)
        {
            return await _leaveRepository.Update(leave);
        }
    }
}
