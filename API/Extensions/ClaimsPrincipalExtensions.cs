using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
           return user.FindFirst(ClaimTypes.Name)?.Value;  //to get the user's username
        }
        public static int GetUserId(this ClaimsPrincipal user)      //string instead of int
        {
            return int.Parse( user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
