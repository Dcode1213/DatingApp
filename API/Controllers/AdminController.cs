using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
            this.userManager=userManager;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            //return Ok("Only Admins can see this");
            var users = await userManager.Users
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                        u.Id,
                        Username = u.UserName,
                        Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                })
                .ToListAsync();
            return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]           //it should be HttpPut
        public async Task<ActionResult> EditRoles(string username,[FromQuery]string roles)
        {
            if (string.IsNullOrEmpty(roles)) return BadRequest("you must be select at least one role");
            var selectedRoles = roles.Split(",").ToArray();
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return NotFound();

            var userRoles = await userManager.GetRolesAsync(user);       //hold the existing user role that user inside of
            var result = await userManager.AddToRolesAsync(user,selectedRoles.Except(userRoles));      //add the roles that the user not currently in and add them to that role
            if (!result.Succeeded) return BadRequest("Failed to add to roles");
            result = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));  //any roles that user inside of and not contained inside selected role list or array that are created than it removed
            if (!result.Succeeded) return BadRequest("Failed to remove from roles");
            return Ok(await userManager.GetRolesAsync(user));   //return updated list of roles that user is in
        }


        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public ActionResult GetPhotosForModeration()
        {
            return Ok("Admins or Moderators can see this");
        }
    }
}
