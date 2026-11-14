using InstituteManagementSystem.Data;
using InstituteManagementSystem.Models;

namespace InstituteManagementSystem.Services
{
    public class CourseService
    {
        private readonly AppDbContext dbContext;

        public CourseService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Course> GetAllCourses() => dbContext.Set<Course>().ToList();
        public Course? GetCourseById(int id) => dbContext.Find<Course>(id);

        public void AddCourse(Course course)
        {
            dbContext.Add(course);
            dbContext.SaveChanges();
        }

        public void UpdateCourse(Course updatedCourse)
        {
            var course = dbContext.Find<Course>(updatedCourse.CourseId);
            if (course != null)
            {
                course.CourseName = updatedCourse.CourseName;
                course.Description = updatedCourse.Description;
                course.StartDate = updatedCourse.StartDate;
                course.EndDate = updatedCourse.EndDate;
                course.TeacherId = updatedCourse.TeacherId;
                dbContext.SaveChanges();
            }
        }

        public void DeleteCourse(int id)
        {
            var course = dbContext.Find<Course>(id);
            if (course != null)
            {
                dbContext.Remove(course);
                dbContext.SaveChanges();
            }
        }

        public List<Course> SearchCourses(string keyword)
        {
            return dbContext.Set<Course>()
                .Where(c => c.CourseName.Contains(keyword) ||
                            c.Description.Contains(keyword))
                .ToList();
        }

        public List<Course> GetPagedCourses(int pageIndex, int pageSize)
        {
            return dbContext.Set<Course>()
                .OrderBy(c => c.CourseId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
}