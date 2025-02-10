using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Interfaces.SectionRepo
{
    public interface ISectionRepository
    {
        Task<IEnumerable<Section>> GetAllAsync();
        Task<Section> GetByIdAsync(int id);
        Task<Section> AddAsync(Section section);
        Task AddSectionsAsync(List<Section> sections);
        Task<Section> UpdateAsync(Section section);
        Task<bool> DeleteAsync(int id);
    }
}
