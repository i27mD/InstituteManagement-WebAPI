using InstituteManagementSystem.Data;
using InstituteManagementSystem.Models;

namespace InstituteManagementSystem.Services
{
    public class StudentService
    {
        private readonly AppDbContext dbContext;

        public StudentService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Student> GetAllStudents() => dbContext.Set<Student>().ToList();
        public Student? GetStudentById(int id) => dbContext.Find<Student>(id);

        public void AddStudent(Student student)
        {
            dbContext.Add(student);
            dbContext.SaveChanges();
        }

        public void UpdateStudent(Student updatedStudent)
        {
            var student = dbContext.Find<Student>(updatedStudent.StudentId);
            if (student != null)
            {
                student.FullName = updatedStudent.FullName;
                student.Email = updatedStudent.Email;
                student.Phone = updatedStudent.Phone;
                dbContext.SaveChanges();
            }
        }

        public void DeleteStudent(int id)
        {
            var student = dbContext.Find<Student>(id);
            if (student != null)
            {
                dbContext.Remove(student);
                dbContext.SaveChanges();
            }
        }

        // إضافة دالة البحث كما في التمرين
        public List<Student> SearchStudents(string keyword)
        {
            return dbContext.Set<Student>()
                .Where(s => s.FullName.Contains(keyword) ||
                            s.Email.Contains(keyword) ||
                            s.Phone.Contains(keyword))
                .ToList();
        }

        // إضافة دالة التقسيم كما في التمرين
        public List<Student> GetPagedStudents(int pageIndex, int pageSize)
        {
            return dbContext.Set<Student>()
                .OrderBy(s => s.StudentId) // الترتيب ضروري قبل التقسيم
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
}