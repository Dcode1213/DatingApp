using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> userManager;

        //private readonly DataContext context;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        //public AccountController(DataContext context,ITokenService tokenService, IMapper mapper) 
        public AccountController(UserManager<AppUser> userManager,ITokenService tokenService, IMapper mapper)
        {
             this.userManager = userManager;
            //this.context=context;
            this.tokenService=tokenService;
            this.mapper=mapper;
        }
            
        [HttpPost("register")]         // POST : api/account.register       -- EndPoint

        //public async Task<ActionResult<AppUser>> register(string username, string password)
       // public async Task<ActionResult<AppUser>> register(RegisterDto registerDto)
        public async Task<ActionResult<UserDto>> register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            var user = mapper.Map<AppUser>(registerDto);

            //using var hmac = new HMACSHA512();   //hashing algorithm  //i
            //var user = new AppUser       //CREATED INSTANCE OF APPUSER CLASS
            //{
                //UserName = username,  
                //PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),    //converts string into array(Byte)
              
                user.UserName = registerDto.Username.ToLower();
            //user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password)); //i
            //user.PasswordSalt = hmac.Key;//i

            //};
            //context.Users.Add(user);
            //await context.SaveChangesAsync();
            var result = await userManager.CreateAsync(user, registerDto.password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await userManager.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded) return BadRequest(result.Errors);
            //return user;

            return new UserDto 
            {
                Username = user.UserName,
                Token = await tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
              
            };
        }

        [HttpPost("login")]
        //public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await userManager.Users
                .Include(p => p.Photos).SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (user == null) return Unauthorized("Invalid Username");
            //using var hmac = new HMACSHA512(user.PasswordSalt);   //binary                 //i
            //var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));

            //for(int i =  0; i < computedHash.Length; i++)
            //{
            //    if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            //}
            // return user;

            var result = await userManager.CheckPasswordAsync(user, loginDto.password);
            if (!result) return Unauthorized("Invalid password");

            return new UserDto
            {
                Username = user.UserName,
                Token =  await tokenService.CreateToken(user),
               // PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
