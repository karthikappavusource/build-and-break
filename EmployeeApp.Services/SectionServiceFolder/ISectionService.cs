using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.SectionServiceFolder
{
    public interface ISectionService
    {
        Task<IEnumerable<Section>> GetAllSectionsAsync();
        Task<Section> GetSectionByIdAsync(int id);
        Task<Section> AddSectionAsync(Section section);
        Task AddSectionsAsync(List<Section> sections);
        Task<Section> UpdateSectionAsync(Section section);
        Task<bool> DeleteSectionAsync(int id);
    }
}
