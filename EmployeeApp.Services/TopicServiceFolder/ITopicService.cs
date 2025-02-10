using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.TopicServiceFolder
{
    public interface ITopicService
    {
        Task<IEnumerable<Topic>> GetAllTopicsAsync();
        Task<Topic> GetTopicByIdAsync(int id);
        Task<Topic> AddTopicAsync(Topic topic);
        Task AddTopicsAsync(List<Topic> topics);
        Task<Topic> UpdateTopicAsync(Topic topic);
        Task<bool> DeleteTopicAsync(int id);
    }
}
