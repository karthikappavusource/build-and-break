using EmployeeApp.Data.Models;

namespace EmployeeApp.PortalWithAuth.Models
{
    public class ProgramDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description {  get; set; }
        public  List<Section> sections {  get; set; }
    }
}
