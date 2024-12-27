using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Extensions
{
    public static class DatabaseExtension
    {
        public static IServiceCollection AddSqlServer(this IServiceCollection services, ConfigurationManager configuration)
        {
            var connectionString = configuration.GetConnectionString("DBConnectionString") ?? throw new InvalidOperationException("Connection string 'DBConnectionString' not found.");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            services.AddIdentity<UserModel, IdentityRole>()
                .AddTokenProvider<DataProtectorTokenProvider<UserModel>>(TokenOptions.DefaultProvider)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
