using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.GroupServiceFolder
{
    public interface IGroupService
    {
        Task<Group> CreateGroupAsync(Group group);
        Task<bool> DeleteGroupAsync(int id);
        Task<Group> EditGroupAsync(Group group);
        Task<Group> GetGroupByIdAsync(int id);
        Task<int> GetGroupIdByNameAsync(string name);
        Task<string> GetGroupNameByIdAsync(int id);
        Task<List<string>> GetAllGroupNamesAsync();
    }
}
