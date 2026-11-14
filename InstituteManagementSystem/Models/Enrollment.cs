using System.ComponentModel.DataAnnotations;

namespace InstituteManagementSystem.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string Status { get; set; } = "Active";    // Active, Completed, Dropped
    }
}