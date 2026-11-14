using System.ComponentModel.DataAnnotations;

namespace InstituteManagementSystem.DTOs
{
    public class StudentUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        // لم نقم بتضمين StudentId هنا لأنه سيأتي من الرابط (Route)
        // ولم نقم بتضمين JoinDate لأننا لا نريد تعديله بعد الإضافة
    }
}