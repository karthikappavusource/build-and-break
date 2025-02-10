using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Interfaces.TopicRepo
{
    public interface ITopicRepository
    {
        Task<IEnumerable<Topic>> GetAllAsync();
        Task<Topic> GetByIdAsync(int id);
        Task<Topic> AddAsync(Topic topic);
        Task AddTopicsAsync(List<Topic> topics);
        Task<Topic> UpdateAsync(Topic topic);
        Task<bool> DeleteAsync(int id);
    }
}
