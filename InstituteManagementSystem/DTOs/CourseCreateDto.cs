using System.ComponentModel.DataAnnotations;

namespace InstituteManagementSystem.DTOs
{
    public class CourseCreateDto
    {
        [Required]
        [MaxLength(150)]
        public string CourseName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required]
        public int TeacherId { get; set; }
    }
}