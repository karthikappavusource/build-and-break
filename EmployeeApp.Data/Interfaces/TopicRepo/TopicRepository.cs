using EmployeeApp.Data.Data;
using EmployeeApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Interfaces.TopicRepo
{
    public class TopicRepository:ITopicRepository
    {
        private readonly EmployeeDB2Context _context;

        public TopicRepository(EmployeeDB2Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Topic>> GetAllAsync()
        {
            return await _context.Set<Topic>()
                .Include(t => t.Program)  // Include related Program
                .Include(t => t.Section)  // Include related Section
                .ToListAsync();
        }

        public async Task<Topic> GetByIdAsync(int id)
        {
            return await _context.Set<Topic>()
                .Include(t => t.Program)  // Include related Program
                .Include(t => t.Section)  // Include related Section
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Topic> AddAsync(Topic topic)
        {
            _context.Set<Topic>().Add(topic);
            await _context.SaveChangesAsync();
            return topic;
        }

        public async Task<Topic> UpdateAsync(Topic topic)
        {
            var existingTopic = await _context.Set<Topic>().FindAsync(topic.Id);

            if (existingTopic == null)
            {
                return null;
            }

            _context.Entry(existingTopic).CurrentValues.SetValues(topic);
            await _context.SaveChangesAsync();
            return existingTopic;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var topic = await _context.Set<Topic>().FindAsync(id);

            if (topic == null)
            {
                return false;
            }

            _context.Set<Topic>().Remove(topic);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task AddTopicsAsync(List<Topic> topics)
        {
            await _context.Topics.AddRangeAsync(topics);
            await _context.SaveChangesAsync();
        }
    }
}
