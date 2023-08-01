using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;         //not neccessory but it's good to use
            //var username = resultContext.HttpContext.User.GetUsername();   -- for biq query
            var userId = resultContext.HttpContext.User.GetUserId();
            var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            var user = await repo.getUserByIdAsync(int.Parse(userId));
            user.LastActive = DateTime.UtcNow;
            await repo.SaveAllAsync();

        }
    }
}
