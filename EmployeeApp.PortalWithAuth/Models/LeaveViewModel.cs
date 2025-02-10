namespace EmployeeApp.PortalWithAuth.Models
{
    public class LeaveViewModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string PurposeOfLeave { get; set; }
        public string? Note {  get; set; }
    }
}
