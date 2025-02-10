using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Models
{
    public class Leave
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string PurposeOfLeave { get; set; }
        public string? Note { get; set; }
        public int statusId { get; set; }
        public int UserId {  get; set; }

        [ForeignKey("statusId")]
        public Status status { get; set; }
        [ForeignKey("UserId")]
        public User user { get; set; }
    }
}
