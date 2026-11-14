using System.ComponentModel.DataAnnotations;

namespace InstituteManagementSystem.DTOs
{
    public class UserCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty; // سيتم تشفيرها لاحقاً إلى PasswordHash

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "User";
    }
}