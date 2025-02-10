using EmployeeApp.Data.Models;

namespace EmployeeApp.PortalWithAuth.Models
{
    public class LeaveUserRole
    {
        public Leave leave { get; set; }
        public User user { get; set; }
        public Role role { get; set; }
    }
}
