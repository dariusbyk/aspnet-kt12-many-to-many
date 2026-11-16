using Microsoft.EntityFrameworkCore;
using StudyApp.Models;

namespace StudyApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Courses)
                .WithOne(c => c.Teacher)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            // EF Core создаёт промежуточную таблицу записей на курсы.
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Courses)
                .WithMany(c => c.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentCourses",
                    j => j.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId").OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Student>().WithMany()
                        .HasForeignKey("StudentId").OnDelete(DeleteBehavior.Cascade),
                    j => j.HasKey("StudentId", "CourseId"));
        }
    }
}
