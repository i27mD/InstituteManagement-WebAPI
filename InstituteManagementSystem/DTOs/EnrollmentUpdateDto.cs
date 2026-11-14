using System.ComponentModel.DataAnnotations;

namespace InstituteManagementSystem.DTOs
{
    public class EnrollmentUpdateDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }
    }
}