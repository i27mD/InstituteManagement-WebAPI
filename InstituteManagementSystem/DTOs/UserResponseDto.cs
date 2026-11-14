namespace InstituteManagementSystem.DTOs
{
    public class UserResponseDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        // تم استبعاد PasswordHash من الاستجابة التزاماً بالشروط
    }
}