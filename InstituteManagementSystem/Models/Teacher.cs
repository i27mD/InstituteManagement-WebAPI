using System.ComponentModel.DataAnnotations;

namespace InstituteManagementSystem.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Specialization { get; set; } = string.Empty;

        // Collection Navigation Property للربط مع جدول الدورات (علاقة 1 إلى متعدد)
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}