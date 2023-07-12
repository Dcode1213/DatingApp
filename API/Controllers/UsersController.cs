using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]   //api/users  -- for access
    //[Route("[controller]")]
    [Authorize]
    public class UsersController : BaseApiController   //created custom base class
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        //private readonly DataContext context;
        public UsersController(IUserRepository userRepository,IMapper mapper)
        {
            this.userRepository=userRepository;
            this.mapper=mapper;
            //this.context = context;  // instance
        }
      //[AllowAnonymous]
      [HttpGet]
      public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers(){
           var users =  await userRepository.getMembersAsync();
            return Ok(users);

            // var usersToReturn = mapper.Map<IEnumerable<MemberDto>>(users);    //not need that
            //  return Ok(await userRepository.GetUsersAsync());                  //context.Users.ToListAsync();
            // return users;
        }

      [HttpGet("{username}")] 
      public async Task<ActionResult<MemberDto>> GetUser(string username){    //for id //id     //now username  //now MemberDto

            return await userRepository.GetMemberAsync(username);

            //  var user = context.Users.Find(id);
            //return mapper.Map<MemberDto>(user);  //removed because it now handdle on repository
            //return await context.Users.FindAsync(id);       
            // return user;    -- another way
        }
                

        // public IActionResult Index()
        // {
        //     return View();
        // }
    }
}
