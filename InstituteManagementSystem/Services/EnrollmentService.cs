using InstituteManagementSystem.Data;
using InstituteManagementSystem.Models;

namespace InstituteManagementSystem.Services
{
    public class EnrollmentService
    {
        private readonly AppDbContext dbContext;

        public EnrollmentService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Enrollment> GetAllEnrollments() => dbContext.Set<Enrollment>().ToList();
        public Enrollment? GetEnrollmentById(int id) => dbContext.Find<Enrollment>(id);

        public void AddEnrollment(Enrollment enrollment)
        {
            dbContext.Add(enrollment);
            dbContext.SaveChanges();
        }

        public void UpdateEnrollment(Enrollment updatedEnrollment)
        {
            var enrollment = dbContext.Find<Enrollment>(updatedEnrollment.EnrollmentId);
            if (enrollment != null)
            {
                enrollment.StudentId = updatedEnrollment.StudentId;
                enrollment.CourseId = updatedEnrollment.CourseId;
                dbContext.SaveChanges();
            }
        }

        public void DeleteEnrollment(int id)
        {
            var enrollment = dbContext.Find<Enrollment>(id);
            if (enrollment != null)
            {
                dbContext.Remove(enrollment);
                dbContext.SaveChanges();
            }
        }

        public List<Enrollment> SearchEnrollments(string keyword)
        {
            // البحث عبر الحالة وعبر خصائص التنقل (الطالب والدورة)
            return dbContext.Set<Enrollment>()
                .Where(e => e.Status.Contains(keyword) ||
                            e.Student.FullName.Contains(keyword) ||
                            e.Course.CourseName.Contains(keyword))
                .ToList();
        }

        public List<Enrollment> GetPagedEnrollments(int pageIndex, int pageSize)
        {
            return dbContext.Set<Enrollment>()
                .OrderBy(e => e.EnrollmentId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        // تطبيق دالة التحديث المخصص للحالة كما في التمرين
        public void UpdateEnrollmentStatus(int id, string newStatus)
        {
            var enrollment = dbContext.Find<Enrollment>(id);
            if (enrollment != null)
            {
                enrollment.Status = newStatus;
                dbContext.SaveChanges();
            }
        }
    }
}