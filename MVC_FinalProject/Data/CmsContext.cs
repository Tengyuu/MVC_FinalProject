using Microsoft.EntityFrameworkCore;
using MVC_FinalProject.Models;

namespace MVC_FinalProject.Data
{
    public class CmsContext:DbContext
    {
        public CmsContext(DbContextOptions<CmsContext> options):base(options)
        {

        }
        public DbSet<Student> Table1121645 { get; set; }
        public DbSet<Enrollment> TableEnrollments1121645 {  get; set; }
        public DbSet<Course> TableCourses1121645 { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Table1121645)
                .WithMany(s => s.TableEnrollments1121645)
                .HasForeignKey(e => e.StudentId);
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.TableCourses1121645)
                .WithMany(s => s.TableEnrollments1121645)
                .HasForeignKey(e => e.CourseId);
        }
    }
}
