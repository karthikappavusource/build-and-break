using EmployeeApp.Data.Interfaces.TopicRepo;
using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.TopicServiceFolder
{
    public class TopicService:ITopicService
    {
        private readonly ITopicRepository _topicRepository;

        public TopicService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public async Task<IEnumerable<Topic>> GetAllTopicsAsync()
        {
            return await _topicRepository.GetAllAsync();
        }

        public async Task<Topic> GetTopicByIdAsync(int id)
        {
            return await _topicRepository.GetByIdAsync(id);
        }

        public async Task<Topic> AddTopicAsync(Topic topic)
        {
            return await _topicRepository.AddAsync(topic);
        }

        public async Task<Topic> UpdateTopicAsync(Topic topic)
        {
            return await _topicRepository.UpdateAsync(topic);
        }

        public async Task<bool> DeleteTopicAsync(int id)
        {
            return await _topicRepository.DeleteAsync(id);
        }

        public async Task AddTopicsAsync(List<Topic> topics)
        {
            await _topicRepository.AddTopicsAsync(topics);
        }
    }
}
