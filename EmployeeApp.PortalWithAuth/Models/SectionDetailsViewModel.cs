using EmployeeApp.Data.Models;

namespace EmployeeApp.PortalWithAuth.Models
{
    public class SectionDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Topic> topics { get; set; }
    }
}
