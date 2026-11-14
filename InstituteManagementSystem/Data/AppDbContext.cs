using InstituteManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InstituteManagementSystem.Data
{
    // يجب أن يرث الكلاس من DbContext كما في المحاضرات
    public class AppDbContext : DbContext
    {
        // Constructor لاستقبال خيارات الاتصال وتمريرها للكلاس الأساسي
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // تعريف الجداول (DbSets) التي سيتم إنشاؤها في قاعدة البيانات
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Fluent API: علاقة المدرس مع الدورة (One-to-Many)
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TeacherId);

            // 2. Fluent API: علاقة الطالب مع التسجيل (One-to-Many)
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId);

            // 3. Fluent API: علاقة الدورة مع التسجيل (One-to-Many)
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);
        }
    }
}