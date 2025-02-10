using EmployeeApp.Data.Interfaces.SectionRepo;
using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.SectionServiceFolder
{
    public class SectionService:ISectionService
    {
        private readonly ISectionRepository _sectionRepository;

        public SectionService(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public async Task<IEnumerable<Section>> GetAllSectionsAsync()
        {
            return await _sectionRepository.GetAllAsync();
        }

        public async Task<Section> GetSectionByIdAsync(int id)
        {
            return await _sectionRepository.GetByIdAsync(id);
        }

        public async Task<Section> AddSectionAsync(Section section)
        {
            return await _sectionRepository.AddAsync(section);
        }

        public async Task<Section> UpdateSectionAsync(Section section)
        {
            return await _sectionRepository.UpdateAsync(section);
        }

        public async Task<bool> DeleteSectionAsync(int id)
        {
            return await _sectionRepository.DeleteAsync(id);
        }

        public async Task AddSectionsAsync(List<Section> sections)
        {
            await _sectionRepository.AddSectionsAsync(sections);
        }
    }
}
