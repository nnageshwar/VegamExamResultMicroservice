using FinalExamResult.DBContext;
using FinalExamResult.DTO;
using FinalExamResult.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalExamResult.API.Controllers
{
    [Route("api/exam-results")]
    [ApiController]
    public class StudentMarksController : ControllerBase
    {
        #region Construction
        private IStudentMarksRepository studentMarksRepository;
        private ILogger<StudentMarksController> logger;
        private FinalYearResultDBContext dBContext;
        public StudentMarksController(IStudentMarksRepository _studentMarksRepository, FinalYearResultDBContext _db, ILogger<StudentMarksController> _logger)
        {
            studentMarksRepository = _studentMarksRepository;
            dBContext = _db;
            logger = _logger;
        }
        #endregion

        #region API End points
        /// <summary>
        /// Gets exam results with total marks and pass/fail status.
        /// </summary>
        /// <returns>List of student results with total marks.</returns>
        [HttpGet]
        [Route("total")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StudentMarksDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStudentMarksWithTotalsAsync()
        {
            var result = await Task.Run(() => studentMarksRepository.GetExamResultsTotalAsync());
            return Ok(result);
        }

        /// <summary>
        /// Gets exam results with grace marks applied if eligible.
        /// </summary>
        /// <returns>List of student results with remarks about grace marks.</returns>
        [HttpGet]
        [Route("with-grace-marks")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StudentMarksWithTotalDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetExamResultsWithGraceMarksAsync()
        {
            var result = await Task.Run(() => studentMarksRepository.GetExamResultsWithGraceMarksAsync());
            return Ok(result);
        } 
        #endregion

    }
}
