using System.ComponentModel.DataAnnotations;

namespace InstituteManagementSystem.DTOs
{
    public class UserUpdateDto
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "User";

        // لا نسمح بتحديث كلمة المرور من خلال هذا الـ DTO لأسباب أمنية
    }
}