namespace EmployeeApp.PortalWithAuth.Models
{
    public class UpcomingLeaveModel
    {
        public int? leaveId {  get; set; }
        public string Name {  get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string PurposeOfLeave { get; set; }
        public string status { get; set; }
    }
}
