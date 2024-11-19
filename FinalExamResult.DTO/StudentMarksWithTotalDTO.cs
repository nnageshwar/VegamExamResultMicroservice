using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExamResult.DTO
{
    public class StudentMarksWithTotalDTO
    {
        public string StudentName { get; set; }
        public Dictionary<string, int> Marks { get; set; }
        public int Total { get; set; }
        public string Result {  get; set; }
        public string Remarks { get; set; }
    }
}
