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
    public class TeachersController : ControllerBase
    {
        private readonly TeacherService _teacherService;

        public TeachersController(TeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var teachers = _teacherService.GetAllTeachers();
            var response = teachers.Select(t => new TeacherResponseDto
            {
                TeacherId = t.TeacherId,
                FullName = t.FullName,
                Email = t.Email,
                Phone = t.Phone,
                Specialization = t.Specialization
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id:int:min(1)}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var teacher = _teacherService.GetTeacherById(id);
            if (teacher == null) return NotFound($"Teacher with ID {id} was not found.");

            var response = new TeacherResponseDto
            {
                TeacherId = teacher.TeacherId,
                FullName = teacher.FullName,
                Email = teacher.Email,
                Phone = teacher.Phone,
                Specialization = teacher.Specialization
            };
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Add([FromBody] TeacherCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newTeacher = new Teacher
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Specialization = dto.Specialization
            };

            _teacherService.AddTeacher(newTeacher);
            return Ok(new { message = "Teacher added successfully." });
        }

        [HttpPut("{id:int:min(1)}")]
        public IActionResult Update([FromRoute] int id, [FromBody] TeacherUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingTeacher = _teacherService.GetTeacherById(id);
            if (existingTeacher == null) return NotFound($"Teacher with ID {id} was not found.");

            var updatedTeacher = new Teacher
            {
                TeacherId = id,
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Specialization = dto.Specialization
            };

            _teacherService.UpdateTeacher(updatedTeacher);
            return Ok(new { message = "Teacher Updated successfully." });
        }

        [HttpDelete("{id:int:min(1)}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var existingTeacher = _teacherService.GetTeacherById(id);
            if (existingTeacher == null) return NotFound($"Teacher with ID {id} was not found.");

            _teacherService.DeleteTeacher(id);
            return Ok(new { message = "Teacher Deleted successfully." });
        }

        [HttpGet("search")]
        public IActionResult SearchTeachers([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return BadRequest("Search keyword cannot be empty.");

            var teachers = _teacherService.SearchTeachers(keyword);
            var response = teachers.Select(t => new TeacherResponseDto
            {
                TeacherId = t.TeacherId,
                FullName = t.FullName,
                Email = t.Email,
                Phone = t.Phone,
                Specialization = t.Specialization
            }).ToList();

            return Ok(response);
        }

        [HttpGet("paged")]
        public IActionResult GetPaged([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            if (pageIndex < 1 || pageSize < 1) return BadRequest("PageIndex and PageSize must be greater than 0.");

            var pagedTeachers = _teacherService.GetPagedTeachers(pageIndex, pageSize);
            var response = pagedTeachers.Select(t => new TeacherResponseDto
            {
                TeacherId = t.TeacherId,
                FullName = t.FullName,
                Email = t.Email,
                Phone = t.Phone,
                Specialization = t.Specialization
            }).ToList();

            return Ok(response);
        }
    }
}