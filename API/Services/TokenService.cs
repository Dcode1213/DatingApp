using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;    //USED FOR ENCRYPTION 
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
           
        }
        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId,user.UserName)
            };
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
