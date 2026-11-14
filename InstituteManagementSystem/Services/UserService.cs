using InstituteManagementSystem.Data;
using InstituteManagementSystem.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InstituteManagementSystem.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        // Constructor واحد فقط يجمع قاعدة البيانات وإعدادات JWT
        public UserService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // ----------------------------------------------------
        // 1. دالة المصادقة وإنشاء التوكن (JWT)
        // ----------------------------------------------------
        public string Authenticate(string username, string password)
        {
            // التحقق من وجود المستخدم
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == password);
            if (user == null) return null;

            // تجهيز المفتاح السري
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // تجهيز بيانات المستخدم (Claims)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // إنشاء التوكن
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2), // التوكن صالح لمدة ساعتين
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // ----------------------------------------------------
        // 2. دوال إدارة المستخدمين القديمة (CRUD)
        // ----------------------------------------------------

        public List<User> GetAllUsers() => _context.Set<User>().ToList();

        public User? GetUserById(int id) => _context.Find<User>(id);

        public void AddUser(User user)
        {
            _context.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User updatedUser)
        {
            var user = _context.Find<User>(updatedUser.UserId);
            if (user != null)
            {
                user.Username = updatedUser.Username;
                user.Email = updatedUser.Email;
                user.Role = updatedUser.Role;
                // Password لا يتم تحديثه هنا لدواعي الأمان
                _context.SaveChanges();
            }
        }

        public void DeleteUser(int id)
        {
            var user = _context.Find<User>(id);
            if (user != null)
            {
                _context.Remove(user);
                _context.SaveChanges();
            }
        }

        // ----------------------------------------------------
        // 3. دوال البحث والتقسيم (Search & Pagination)
        // ----------------------------------------------------

        public List<User> SearchUsers(string keyword)
        {
            return _context.Set<User>()
                .Where(u => u.Username.Contains(keyword) ||
                            u.Email.Contains(keyword) ||
                            u.Role.Contains(keyword))
                .ToList();
        }

        public List<User> GetPagedUsers(int pageIndex, int pageSize)
        {
            return _context.Set<User>()
                .OrderBy(u => u.UserId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
}