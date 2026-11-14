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
    public class StudentsController : ControllerBase
    {
        private readonly StudentService _studentService;

        public StudentsController(StudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/students
        [HttpGet]
        public IActionResult GetAll()
        {
            var students = _studentService.GetAllStudents();
            // Manual Mapping: تحويل Model إلى ResponseDto
            var response = students.Select(s => new StudentResponseDto
            {
                StudentId = s.StudentId,
                FullName = s.FullName,
                Email = s.Email,
                Phone = s.Phone,
                JoinDate = s.JoinDate
            }).ToList();

            return Ok(response);
        }

        // GET: api/students/5
        [HttpGet("{id:int:min(1)}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var student = _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound($"Student with ID {id} was not found.");
            }

            var response = new StudentResponseDto
            {
                StudentId = student.StudentId,
                FullName = student.FullName,
                Email = student.Email,
                Phone = student.Phone,
                JoinDate = student.JoinDate
            };
            return Ok(response);
        }

        // POST: api/students
        [HttpPost]
        public IActionResult Add([FromBody] StudentCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Manual Mapping: تحويل CreateDto إلى Model
            var newStudent = new Student
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                JoinDate = dto.JoinDate
            };

            _studentService.AddStudent(newStudent);
            return Ok(new { message = "Student added successfully." });
        }

        // PUT: api/students/5
        [HttpPut("{id:int:min(1)}")]
        public IActionResult Update([FromRoute] int id, [FromBody] StudentUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingStudent = _studentService.GetStudentById(id);
            if (existingStudent == null)
            {
                return NotFound($"Student with ID {id} was not found.");
            }

            // Manual Mapping: تحويل UpdateDto إلى Model وتمرير الـ ID
            var updatedStudent = new Student
            {
                StudentId = id,
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone
            };

            _studentService.UpdateStudent(updatedStudent);
            return Ok(new { message = "Student updated successfully." });
        }

        // DELETE: api/students/5
        [HttpDelete("{id:int:min(1)}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var existingStudent = _studentService.GetStudentById(id);
            if (existingStudent == null) return NotFound($"Student with ID {id} was not found.");

            _studentService.DeleteStudent(id);
            return Ok(new { message = "Student deleted successfully." });
        }

        // GET: api/students/search?keyword=Ali
        [HttpGet("search")]
        public IActionResult SearchStudents([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("Search keyword cannot be empty."); // كما في التمرين بالضبط

            var students = _studentService.SearchStudents(keyword);
            var response = students.Select(s => new StudentResponseDto
            {
                StudentId = s.StudentId,
                FullName = s.FullName,
                Email = s.Email,
                Phone = s.Phone,
                JoinDate = s.JoinDate
            }).ToList();

            return Ok(response);
        }

        // GET: api/students/paged?pageIndex=1&pageSize=10
        [HttpGet("paged")]
        public IActionResult GetPaged([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            if (pageIndex < 1 || pageSize < 1)
                return BadRequest("PageIndex and PageSize must be greater than 0.");

            var pagedStudents = _studentService.GetPagedStudents(pageIndex, pageSize);
            var response = pagedStudents.Select(s => new StudentResponseDto
            {
                StudentId = s.StudentId,
                FullName = s.FullName,
                Email = s.Email,
                Phone = s.Phone,
                JoinDate = s.JoinDate
            }).ToList();

            return Ok(response);
        }
    }
}