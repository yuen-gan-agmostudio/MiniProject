using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using WebApplication1.Models;

namespace WebApplication1
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<CourseModel> Courses { get; set; }
        public DbSet<UserCourseModel> UserCourses { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(LocalDb)\\MSSQLLocalDB;Database=Test;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
