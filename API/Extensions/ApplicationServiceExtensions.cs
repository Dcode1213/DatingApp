using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration config)
        {
            services.AddDbContext<DataContext>(opt =>
            {

                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            services.AddCors();                                            // ---->> added for CORS for calling api
            services.AddScoped<ITokenService, TokenService>();            // token service
            services.AddScoped<IUserRepository, UserRepository>();        // for repository
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());     //auto mapper configuration
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();               //for cloudinary photo upload
            services.AddScoped<LogUserActivity>();                           //for actionFilter
            return services;   
        }
    }
}
 