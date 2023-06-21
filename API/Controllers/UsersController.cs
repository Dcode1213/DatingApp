using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
   [ApiController]
   [Route("api/[controller]")]   //api/users  -- for access
    //[Route("[controller]")]
    public class UsersController : ControllerBase
    {
      private readonly DataContext context;
      public UsersController(DataContext context)
      {
        this.context = context;  // instance
      }
      [HttpGet]
      public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
         var users = await context.Users.ToListAsync();
         return users;
      }

      [HttpGet("{id}")]
      public async Task<ActionResult<AppUser>> GetUser(int id){    //for id //id
            //  var user = context.Users.Find(id);
             return await context.Users.FindAsync(id);
            // return user;    -- another way
        }
        

        // public IActionResult Index()
        // {
        //     return View();
        // }
    }
}
