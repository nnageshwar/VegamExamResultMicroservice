using FinalExamResult.DBContext;
using FinalExamResult.DTO;
using FinalExamResult.IRepository;
using FinalExamResult.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FinalExamResult.Repository
{
    public class StudentMarksRepository : IStudentMarksRepository
    {
        private FinalYearResultDBContext dbContext;
        public StudentMarksRepository(FinalYearResultDBContext _dbContext)
        {
            dbContext = _dbContext;
        }

        #region Repository Implementation
        public async Task<List<StudentMarksDTO>> GetExamResultsTotalAsync()
        {
            var studentMarksData = await GetStudentMarksData();

            var result = studentMarksData
                .GroupBy(s => s.StudentName)
                .Select(g => new StudentMarksDTO {
                    StudentName = g.Key,
                    Marks = g.ToDictionary(s => s.SubjectName, s => s.Marks),
                    Total = g.Sum(s => s.Marks),
                    Result = g.Any(s => s.Marks < 30) ? "Fail" : "Pass" // If any subject has marks < 30, set "Fail"; otherwise, "Pass"
                })
                .OrderByDescending(s => s.Total) // Order by Total marks in descending order
                .ToList();

            return result;
        }

        public async Task<List<StudentMarksWithTotalDTO>> GetExamResultsWithGraceMarksAsync()
        {
            var studentMarksData = await GetStudentMarksData();

            var result = studentMarksData
                .GroupBy(s => s.StudentName)
                .Select(g =>
                {
                    var marksDict = g.ToDictionary(s => s.SubjectName, s => s.Marks);
                    var failedSubjects = marksDict.Where(m => m.Value < 30).ToList();

                    string resultStatus;
                    string remarks = "";

                    if(failedSubjects.Count > 2) {
                        // If more than 2 subjects are failed, student is failed
                        resultStatus = "Fail";
                        remarks = "Failed (more than 2 subjects failed)";
                    }
                    else {
                        // Calculate the total grace marks needed
                        int totalGraceMarksNeeded = failedSubjects.Sum(f => 30 - f.Value);

                        if(totalGraceMarksNeeded <= 6) {
                            // Apply grace marks
                            foreach(var subject in failedSubjects) {
                                marksDict[subject.Key] = 30; // Update the marks to 30
                            }

                            resultStatus = "Pass";
                            remarks = totalGraceMarksNeeded > 0 ? "Passed with " + totalGraceMarksNeeded + " grace marks" : "";
                        }
                        else {
                            // If total grace marks exceed 6, student is failed
                            resultStatus = "Fail";
                            remarks = "Failed (grace marks exceeded 6)";
                        }
                    }

                    return new StudentMarksWithTotalDTO {
                        StudentName = g.Key,
                        Marks = marksDict,
                        Total = marksDict.Values.Sum(),
                        Result = resultStatus,
                        Remarks = remarks
                    };
                })
                .OrderByDescending(s => s.Total) // Order by Total marks in descending order
                .ToList();

            return result;
        }
        #endregion

        #region Data Access
        private Task<List<StudentMarks>> GetStudentMarksData()
        {
            ////Use this if you have a database like SQL Server
            //var studentMarksData = await dbContext.StudentMarks.ToListAsync();
            //return studentMarksData;

            var studentMarksList = new List<StudentMarks>
            {
                new StudentMarks { StudentMarksPk = 1, StudentID = 1, StudentName = "Savita", SubjectName = "EC1", Marks = 50 },
                new StudentMarks { StudentMarksPk = 2, StudentID = 1, StudentName = "Savita", SubjectName = "EC2", Marks = 55 },
                new StudentMarks { StudentMarksPk = 3, StudentID = 1, StudentName = "Savita", SubjectName = "EC3", Marks = 28 },
                new StudentMarks { StudentMarksPk = 4, StudentID = 1, StudentName = "Savita", SubjectName = "EC4", Marks = 30 },
                new StudentMarks { StudentMarksPk = 5, StudentID = 1, StudentName = "Savita", SubjectName = "EC5", Marks = 35 },
                new StudentMarks { StudentMarksPk = 6, StudentID = 2, StudentName = "Harish", SubjectName = "EC1", Marks = 55 },
                new StudentMarks { StudentMarksPk = 7, StudentID = 2, StudentName = "Harish", SubjectName = "EC2", Marks = 60 },
                new StudentMarks { StudentMarksPk = 8, StudentID = 2, StudentName = "Harish", SubjectName = "EC3", Marks = 30 },
                new StudentMarks { StudentMarksPk = 9, StudentID = 2, StudentName = "Harish", SubjectName = "EC4", Marks = 35 },
                new StudentMarks { StudentMarksPk = 10, StudentID = 2, StudentName = "Harish", SubjectName = "EC5", Marks = 30 },
                new StudentMarks { StudentMarksPk = 11, StudentID = 3, StudentName = "Mahesh", SubjectName = "EC1", Marks = 28 },
                new StudentMarks { StudentMarksPk = 12, StudentID = 3, StudentName = "Mahesh", SubjectName = "EC2", Marks = 27 },
                new StudentMarks { StudentMarksPk = 13, StudentID = 3, StudentName = "Mahesh", SubjectName = "EC3", Marks = 50 },
                new StudentMarks { StudentMarksPk = 14, StudentID = 3, StudentName = "Mahesh", SubjectName = "EC4", Marks = 41 },
                new StudentMarks { StudentMarksPk = 15, StudentID = 3, StudentName = "Mahesh", SubjectName = "EC5", Marks = 51 },
                new StudentMarks { StudentMarksPk = 16, StudentID = 4, StudentName = "Manjunath", SubjectName = "EC1", Marks = 25 },
                new StudentMarks { StudentMarksPk = 17, StudentID = 4, StudentName = "Manjunath", SubjectName = "EC2", Marks = 28 },
                new StudentMarks { StudentMarksPk = 18, StudentID = 4, StudentName = "Manjunath", SubjectName = "EC3", Marks = 42 },
                new StudentMarks { StudentMarksPk = 19, StudentID = 4, StudentName = "Manjunath", SubjectName = "EC4", Marks = 35 },
                new StudentMarks { StudentMarksPk = 20, StudentID = 4, StudentName = "Manjunath", SubjectName = "EC5", Marks = 30 },
                new StudentMarks { StudentMarksPk = 21, StudentID = 5, StudentName = "Shruthi", SubjectName = "EC1", Marks = 27 },
                new StudentMarks { StudentMarksPk = 22, StudentID = 5, StudentName = "Shruthi", SubjectName = "EC2", Marks = 28 },
                new StudentMarks { StudentMarksPk = 23, StudentID = 5, StudentName = "Shruthi", SubjectName = "EC3", Marks = 29 },
                new StudentMarks { StudentMarksPk = 24, StudentID = 5, StudentName = "Shruthi", SubjectName = "EC4", Marks = 45 },
                new StudentMarks { StudentMarksPk = 25, StudentID = 5, StudentName = "Shruthi", SubjectName = "EC5", Marks = 30 }
            };
            return Task.FromResult(studentMarksList);
        } 
        #endregion
    }
}
