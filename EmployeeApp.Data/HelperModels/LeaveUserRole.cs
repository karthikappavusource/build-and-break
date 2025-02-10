using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.HelperModels
{
    public class LeaveUserRole
    {
       public Leave leave {  get; set; }
       public User user { get; set; }
       public Role role { get; set; } 
    }
}
