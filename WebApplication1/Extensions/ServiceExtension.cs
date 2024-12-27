using WebApplication1.Services;

namespace WebApplication1.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICourseServiceManager, CourseServiceManager>();
            return services;
        }
    }
}
