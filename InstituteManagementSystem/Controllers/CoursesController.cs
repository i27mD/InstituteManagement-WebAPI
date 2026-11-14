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
    public class CoursesController : ControllerBase
    {
        private readonly CourseService _courseService;

        public CoursesController(CourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var courses = _courseService.GetAllCourses();
            var response = courses.Select(c => new CourseResponseDto
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                Description = c.Description,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                TeacherId = c.TeacherId
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id:int:min(1)}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var course = _courseService.GetCourseById(id);
            if (course == null) return NotFound($"Course with ID {id} was not found.");

            var response = new CourseResponseDto
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Description = course.Description,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                TeacherId = course.TeacherId
            };
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Add([FromBody] CourseCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newCourse = new Course
            {
                CourseName = dto.CourseName,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                TeacherId = dto.TeacherId
            };

            _courseService.AddCourse(newCourse);
            return Ok(new { message = "Course added successfully." });
        }

        [HttpPut("{id:int:min(1)}")]
        public IActionResult Update([FromRoute] int id, [FromBody] CourseUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingCourse = _courseService.GetCourseById(id);
            if (existingCourse == null) return NotFound($"Course with ID {id} was not found.");

            var updatedCourse = new Course
            {
                CourseId = id,
                CourseName = dto.CourseName,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                TeacherId = dto.TeacherId
            };

            _courseService.UpdateCourse(updatedCourse);
            return Ok(new { message = "Course Updated successfully." });
        }

        [HttpDelete("{id:int:min(1)}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var existingCourse = _courseService.GetCourseById(id);
            if (existingCourse == null) return NotFound($"Course with ID {id} was not found.");

            _courseService.DeleteCourse(id);
            return Ok(new { message = "Course Deleted successfully." });
        }

        [HttpGet("search")]
        public IActionResult SearchCourses([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return BadRequest("Search keyword cannot be empty.");

            var courses = _courseService.SearchCourses(keyword);
            var response = courses.Select(c => new CourseResponseDto
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                Description = c.Description,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                TeacherId = c.TeacherId
            }).ToList();

            return Ok(response);
        }

        [HttpGet("paged")]
        public IActionResult GetPaged([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            if (pageIndex < 1 || pageSize < 1) return BadRequest("PageIndex and PageSize must be greater than 0.");

            var pagedCourses = _courseService.GetPagedCourses(pageIndex, pageSize);
            var response = pagedCourses.Select(c => new CourseResponseDto
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                Description = c.Description,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                TeacherId = c.TeacherId
            }).ToList();

            return Ok(response);
        }
    }
}