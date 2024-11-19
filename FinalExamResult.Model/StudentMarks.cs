using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExamResult.Model
{
    [Table("StudentMarks")]
    public class StudentMarks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentMarksPk { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public string SubjectName { get; set; }
        public int Marks { get; set; }
    }
}
