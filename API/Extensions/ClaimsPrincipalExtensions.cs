using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
           return user.FindFirst(ClaimTypes.Name)?.Value;  //to get the user's username
        }
        public static string GetUserId(this ClaimsPrincipal user)      //string isnted of int
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
