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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
