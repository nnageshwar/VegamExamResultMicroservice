using FinalExamResult.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExamResult.IRepository
{
    public interface IStudentMarksRepository
    {
        Task<List<StudentMarksDTO>> GetExamResultsTotalAsync();
        Task<List<StudentMarksWithTotalDTO>> GetExamResultsWithGraceMarksAsync();
    }
}
