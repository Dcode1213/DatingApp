using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;    //USED FOR ENCRYPTION 
        private readonly IConfiguration config;
        private readonly UserManager<AppUser> userManager;

        public TokenService(IConfiguration config, UserManager<AppUser> userManager)     
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            this.config=config;
            this.userManager=userManager;
        }
        public async Task<string> CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName),
            };

            var roles = await userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));   

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);    //for signing credentials 

            var tokenDescriptor = new SecurityTokenDescriptor       //describe the token
            {
                Subject = new ClaimsIdentity(claims),    //pass list of claims
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds      //pass signing credentials

            };

            var tokenHandler = new JwtSecurityTokenHandler();           //Token Handler 
            var token = tokenHandler.CreateToken(tokenDescriptor);      // create token method and pass token descriptor
            return tokenHandler.WriteToken(token);                   //passout token in Write method
        }
    }
}
