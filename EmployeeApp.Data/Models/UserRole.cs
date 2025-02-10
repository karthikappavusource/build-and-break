using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public int RoleId { get; set; }
        [ForeignKey("UserId")]
        public User user {  get; set; }
        [ForeignKey("RoleId")]
        public Role role { get; set; }
    }
}
