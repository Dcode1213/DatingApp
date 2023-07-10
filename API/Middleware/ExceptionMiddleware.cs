using API.Errors;
using System.Net;
using System.Text.Json;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            this.next = next;
            this.logger=logger;
            this.env=env;
        }
        public  async Task InvokeAsync(HttpContext context)     //httpContext give access to currently pass http req in middleware
        {
            try
            {
                await next(context); 
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, ex.Message);     //for log the error
                context.Response.ContentType = "application/json";             //in api controller inside by defaut but here specified
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;    //give is 500

                var response = env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())              //used ternery
                    : new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error");

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
