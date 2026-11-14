using InstituteManagementSystem.Data;
using InstituteManagementSystem.Models;

namespace InstituteManagementSystem.Services
{
    public class TeacherService
    {
        private readonly AppDbContext dbContext;

        public TeacherService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Teacher> GetAllTeachers() => dbContext.Set<Teacher>().ToList();
        public Teacher? GetTeacherById(int id) => dbContext.Find<Teacher>(id);

        public void AddTeacher(Teacher teacher)
        {
            dbContext.Add(teacher);
            dbContext.SaveChanges();
        }

        public void UpdateTeacher(Teacher updatedTeacher)
        {
            var teacher = dbContext.Find<Teacher>(updatedTeacher.TeacherId);
            if (teacher != null)
            {
                teacher.FullName = updatedTeacher.FullName;
                teacher.Email = updatedTeacher.Email;
                teacher.Phone = updatedTeacher.Phone;
                teacher.Specialization = updatedTeacher.Specialization;
                dbContext.SaveChanges();
            }
        }

        public void DeleteTeacher(int id)
        {
            var teacher = dbContext.Find<Teacher>(id);
            if (teacher != null)
            {
                dbContext.Remove(teacher);
                dbContext.SaveChanges();
            }
        }

        public List<Teacher> SearchTeachers(string keyword)
        {
            return dbContext.Set<Teacher>()
                .Where(t => t.FullName.Contains(keyword) ||
                            t.Email.Contains(keyword) ||
                            t.Specialization.Contains(keyword))
                .ToList();
        }

        public List<Teacher> GetPagedTeachers(int pageIndex, int pageSize)
        {
            return dbContext.Set<Teacher>()
                .OrderBy(t => t.TeacherId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
}