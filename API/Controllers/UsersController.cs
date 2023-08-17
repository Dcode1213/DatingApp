using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        private readonly IPhotoService photoService;

        //private readonly DataContext context;
        public UsersController(IUserRepository userRepository,IMapper mapper,IPhotoService photoService)
        {
            this.userRepository=userRepository;
            this.mapper=mapper;
            this.photoService=photoService;
            //this.context = context;  // instance
        }
        //[AllowAnonymous]
      //[Authorize(Roles = "Admin")]
      [HttpGet]
      public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery]UserParams userParams){

            var currentUser = await userRepository.getUserByUserNameAsync(User.GetUsername());
            userParams.CurrentUsername = currentUser.UserName;

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = currentUser.Gender == "male" ? "female" : "male";
            }


           var users =  await userRepository.getMembersAsync(userParams);
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize,
                users.TotalCount, users.TotalPages));
            return Ok(users);
             
            // var usersToReturn = mapper.Map<IEnumerable<MemberDto>>(users);    //not need that
            //  return Ok(await userRepository.GetUsersAsync());                  //context.Users.ToListAsync();
            // return users;
        }

      //[Authorize(Roles = "Member")]
      [HttpGet("{username}")] 
      public async Task<ActionResult<MemberDto>> GetUser(string username){    //for id //id     //now username  //now MemberDto

            return await userRepository.GetMemberAsync(username);

            //  var user = context.Users.Find(id);
            //return mapper.Map<MemberDto>(user);  //removed because it now handdle on repository
            //return await context.Users.FindAsync(id);       
            // return user;    -- another way
        }

        [HttpPut] 
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            //var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           // var username = User.GetUsername();     // created claims extension

            var user = await userRepository.getUserByUserNameAsync(User.GetUsername());

            if (user == null) return NotFound();

            mapper.Map(memberUpdateDto, user);
            if (await userRepository.SaveAllAsync()) return NoContent();   //its return okay result (204) 
            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await userRepository.getUserByUserNameAsync(User.GetUsername());

            if (user == null) return NotFound();
            var result = await photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                publicId = result.PublicId
            };
            if (user.Photos.Count == 0) photo.IsMain = true;    
            user.Photos.Add(photo);

            if (await userRepository.SaveAllAsync())                      //return mapper.Map<PhotoDto>(photo);
            {
                return CreatedAtAction(nameof(GetUser), new { username = user.UserName },mapper.Map<PhotoDto>(photo));      //return 201 created response unless of 200
            }

            return BadRequest("Problem occurred via adding photo"); 
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await userRepository.getUserByUserNameAsync(User.GetUsername());

            if (user == null) return NotFound();

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("this is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Problem setting the main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await userRepository.getUserByUserNameAsync(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null) return NotFound();
            if (photo.IsMain) return BadRequest("You can not delete your main photo");
            if(photo.publicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.publicId);
                if (result.Error != null) return BadRequest(result.Error);

            }   
            user.Photos.Remove(photo);
            if (await userRepository.SaveAllAsync()) return Ok();
            return BadRequest("problem deleting photo");
        }
    }
}
