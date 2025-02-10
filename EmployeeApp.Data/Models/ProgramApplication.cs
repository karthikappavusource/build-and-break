using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Models
{
    public class ProgramApplication
    {
        public int Id { get; set; }
        public int ProgramId {  get; set; }
        public int ApplicantId {  get; set; }
        [ForeignKey("ProgramId")]
        public Program program { get; set; }
        [ForeignKey("ApplicantId")]
        public User user { get; set; }
    }
}
