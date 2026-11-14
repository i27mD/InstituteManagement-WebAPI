using InstituteManagementSystem.DTOs;
using InstituteManagementSystem.Models;
using InstituteManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstituteManagementSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly EnrollmentService _enrollmentService;

        public EnrollmentsController(EnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var enrollments = _enrollmentService.GetAllEnrollments();
            var response = enrollments.Select(e => new EnrollmentResponseDto
            {
                EnrollmentId = e.EnrollmentId,
                StudentId = e.StudentId,
                CourseId = e.CourseId,
                EnrollmentDate = e.EnrollmentDate,
                Status = e.Status
            }).ToList();
            return Ok(response);
        }

        [HttpGet("{id:int:min(1)}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var enrollment = _enrollmentService.GetEnrollmentById(id);
            if (enrollment == null) return NotFound($"Enrollment with ID {id} was not found.");

            var response = new EnrollmentResponseDto
            {
                EnrollmentId = enrollment.EnrollmentId,
                StudentId = enrollment.StudentId,
                CourseId = enrollment.CourseId,
                EnrollmentDate = enrollment.EnrollmentDate,
                Status = enrollment.Status
            };
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Add([FromBody] EnrollmentCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newEnrollment = new Enrollment
            {
                StudentId = dto.StudentId,
                CourseId = dto.CourseId
            };

            _enrollmentService.AddEnrollment(newEnrollment);
            return Ok(new { message = "Enrollment added successfully." });
        }

        [HttpPut("{id:int:min(1)}")]
        public IActionResult Update([FromRoute] int id, [FromBody] EnrollmentUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingEnrollment = _enrollmentService.GetEnrollmentById(id);
            if (existingEnrollment == null) return NotFound($"Enrollment with ID {id} was not found.");

            var updatedEnrollment = new Enrollment
            {
                EnrollmentId = id,
                StudentId = dto.StudentId,
                CourseId = dto.CourseId
            };

            _enrollmentService.UpdateEnrollment(updatedEnrollment);
            return Ok(new { message = "Enrollment Updated successfully." });
        }

        [HttpDelete("{id:int:min(1)}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var existingEnrollment = _enrollmentService.GetEnrollmentById(id);
            if (existingEnrollment == null) return NotFound($"Enrollment with ID {id} was not found.");

            _enrollmentService.DeleteEnrollment(id);
            return Ok(new { message = "Enrollment Deleted successfully." });
        }

        // GET: api/enrollments/search?keyword=Active
        [HttpGet("search")]
        public IActionResult SearchEnrollments([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return BadRequest("Search keyword cannot be empty.");

            var enrollments = _enrollmentService.SearchEnrollments(keyword);
            var response = enrollments.Select(e => new EnrollmentResponseDto
            {
                EnrollmentId = e.EnrollmentId,
                StudentId = e.StudentId,
                CourseId = e.CourseId,
                EnrollmentDate = e.EnrollmentDate,
                Status = e.Status
            }).ToList();

            return Ok(response);
        }

        // GET: api/enrollments/paged?pageIndex=1&pageSize=10
        [HttpGet("paged")]
        public IActionResult GetPaged([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            if (pageIndex < 1 || pageSize < 1) return BadRequest("PageIndex and PageSize must be greater than 0.");

            var pagedEnrollments = _enrollmentService.GetPagedEnrollments(pageIndex, pageSize);
            var response = pagedEnrollments.Select(e => new EnrollmentResponseDto
            {
                EnrollmentId = e.EnrollmentId,
                StudentId = e.StudentId,
                CourseId = e.CourseId,
                EnrollmentDate = e.EnrollmentDate,
                Status = e.Status
            }).ToList();

            return Ok(response);
        }

        // -------------------------------------------------------------
        // Custom Update Endpoint لتحديث حالة التسجيل فقط كما في المحاضرة
        // -------------------------------------------------------------
        // PUT: api/enrollments/5/status
        [HttpPut("{id:int:min(1)}/status")]
        public IActionResult UpdateStatus([FromRoute] int id, [FromBody] UpdateEnrollmentStatusDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Status))
            {
                return BadRequest("New status cannot be empty."); // مطابق لتمرين الدكتورة
            }

            var existingEnrollment = _enrollmentService.GetEnrollmentById(id);
            if (existingEnrollment == null)
            {
                return NotFound($"Enrollment with ID {id} was not found.");
            }

            _enrollmentService.UpdateEnrollmentStatus(id, dto.Status);

            // إرجاع Response مطابق لما ورد في المحاضرة 
            return Ok(new
            {
                Message = "Enrollment status updated successfully.",
                EnrollmentId = id,
                UpdatedStatus = dto.Status
            });
        }
    }
}