using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeeApp.Data.Models
{
    public partial class Role
    {
        [Key]
        public int Id { get; set; }
        public string RoleName { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public int CreatedPersonId { get; set; }
        public DateTime LastModified { get; set; }
        public int LastModifiedPersonId { get; set; }
    }
}
