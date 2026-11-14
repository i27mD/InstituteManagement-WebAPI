using System.ComponentModel.DataAnnotations;

namespace InstituteManagementSystem.DTOs
{
    public class EnrollmentCreateDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        // EnrollmentDate تأخذ الوقت الحالي في الـ Model، لا نحتاج لاستقبالها
        // Status تأخذ 'Active' افتراضياً
    }
}