using System.ComponentModel.DataAnnotations;

namespace InstituteManagementSystem.DTOs
{
    public class UpdateEnrollmentStatusDto
    {
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
    }
}