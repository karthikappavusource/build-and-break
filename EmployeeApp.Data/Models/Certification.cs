using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace EmployeeApp.Data.Models
{
    public class Certification
    {
        public int Id { get; set; }
        public DateTime CertifiedOn {  get; set; }
        public string QualifiedFor {  get; set; }
        public DateTime Expiration { get; set; }
        public byte[] file { get; set; }
        
        public int statusId { get; set; }
        public int UserId {  get; set; }

        [ForeignKey("statusId")]
        public Status status { get; set; }
    }
}
