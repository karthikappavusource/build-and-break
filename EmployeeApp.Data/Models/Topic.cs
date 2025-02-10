using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Models
{
    public class Topic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int ProgramId { get; set; }
        [ForeignKey("ProgramId")]
        public Program Program { get; set; }
        public int SectionId { get; set; }
        [ForeignKey("SectionId")]
        public Section Section { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedPersonId { get; set; }
        public DateTime LastModified { get; set; }
        public int LastModifiedPersonId { get; set; }
    }
}
