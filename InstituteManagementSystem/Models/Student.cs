using System.ComponentModel.DataAnnotations;

namespace InstituteManagementSystem.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        public DateTime JoinDate { get; set; } = DateTime.Now;

        // Collection Navigation Property للربط مع جدول التسجيل (علاقة 1 إلى متعدد)
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}