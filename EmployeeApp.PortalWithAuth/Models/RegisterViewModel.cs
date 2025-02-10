using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace EmployeeApp.PortalWithAuth.Models
{
    public class RegisterViewModel
    {
        public string Email {  get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
