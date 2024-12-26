using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using WebApplication1.Models;

namespace WebApplication1
{
    public class ApplicationDbContext : IdentityDbContext<UserModel>
    {
        public DbSet<CourseModel> Courses { get; set; }
        public DbSet<UserCourseModel> UserCourses { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
