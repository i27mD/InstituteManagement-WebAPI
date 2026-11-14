using InstituteManagementSystem.DTOs;
using InstituteManagementSystem.Models;
using InstituteManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InstituteManagementSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous] // يُسمح للجميع بالوصول لهذه الدالة بدون توكن
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var token = _userService.Authenticate(loginDto.Username, loginDto.Password);

            if (token == null)
                return Unauthorized(new { message = "Invalid username or password" });

            // إرجاع التوكن بصيغة JSON
            return Ok(new { token = token });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAllUsers();
            var response = users.Select(u => new UserResponseDto
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role
                // لم نقم بإضافة PasswordHash هنا لحماية البيانات
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id:int:min(1)}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null) return NotFound($"User with ID {id} was not found.");

            var response = new UserResponseDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Add([FromBody] UserCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newUser = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                // نمرر كلمة المرور حالياً كما هي إلى PasswordHash، وسنقوم بتشفيرها لاحقاً في مرحلة المصادقة
                PasswordHash = dto.Password,
                Role = dto.Role
            };

            _userService.AddUser(newUser);
            return Ok(new { message = "User Added successfully." });
        }

        [HttpPut("{id:int:min(1)}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UserUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingUser = _userService.GetUserById(id);
            if (existingUser == null) return NotFound($"User with ID {id} was not found.");

            var updatedUser = new User
            {
                UserId = id,
                Username = dto.Username,
                Email = dto.Email,
                Role = dto.Role
                // لن نعدل PasswordHash هنا لأن UpdateDto لا يحتوي عليه أصلاً
            };

            _userService.UpdateUser(updatedUser);
            return Ok(new { message = "User Updated successfully." });
        }

        [HttpDelete("{id:int:min(1)}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var existingUser = _userService.GetUserById(id);
            if (existingUser == null) return NotFound($"User with ID {id} was not found.");

            _userService.DeleteUser(id);
            return Ok(new { message = "User Deleted successfully." });
        }

        [HttpGet("search")]
        public IActionResult SearchUsers([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return BadRequest("Search keyword cannot be empty.");

            var users = _userService.SearchUsers(keyword);
            var response = users.Select(u => new UserResponseDto
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role
            }).ToList();

            return Ok(response);
        }

        [HttpGet("paged")]
        public IActionResult GetPaged([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            if (pageIndex < 1 || pageSize < 1) return BadRequest("PageIndex and PageSize must be greater than 0.");

            var pagedUsers = _userService.GetPagedUsers(pageIndex, pageSize);
            var response = pagedUsers.Select(u => new UserResponseDto
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role
            }).ToList();

            return Ok(response);
        }



    }
}