using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extensions
{
    public static class IdentityServiceExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)     
           .AddJwtBearer(options =>
             {
                  options.TokenValidationParameters = new TokenValidationParameters                             /// token authentication (Enough info to take a look at the token)
                  {
                      ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                         ValidateIssuer = false,
                         ValidateAudience = false,
                      };
              });
            return services;
        }
    }
}
