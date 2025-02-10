using EmployeeApp.Data.Interfaces.GroupRepo;
using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.GroupServiceFolder
{
    public class GroupService:IGroupService
    {
        private readonly IGroupRepository _groupRepository;

        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<Group> CreateGroupAsync(Group group)
        {
            return await _groupRepository.Create(group);
        }

        public async Task<bool> DeleteGroupAsync(int id)
        {
            return await _groupRepository.Delete(id);
        }

        public async Task<Group> EditGroupAsync(Group group)
        {
            return await _groupRepository.Edit(group);
        }

        public async Task<Group> GetGroupByIdAsync(int id)
        {
            return await _groupRepository.GetGroup(id);
        }

        public async Task<int> GetGroupIdByNameAsync(string name)
        {
            return _groupRepository.GetGroupId(name);
        }

        public async Task<string> GetGroupNameByIdAsync(int id)
        {
            return await _groupRepository.GetGroupName(id);
        }

        public async Task<List<string>> GetAllGroupNamesAsync()
        {
            return await _groupRepository.GetGroups();
        }
    }
}
