using System.ComponentModel.DataAnnotations;

namespace InstituteManagementSystem.DTOs
{
    public class StudentCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        public DateTime JoinDate { get; set; } = DateTime.Now;
    }
}