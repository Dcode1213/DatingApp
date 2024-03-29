using API.Data;
using API.Entities;
using API.Extensions;
using API.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);       //Extensions
builder.Services.AddIdentityServices(builder.Configuration);

//{{
//builder.Services.AddControllers().AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());          //for error my error section 15
//    // added this
//    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
//});

//builder.Services.AddControllers().AddJsonOptions(x =>
//   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);     
//}}

var app = builder.Build();                                      //some changes when not run...

//if(builder.Environment.IsDevelopment())        
//{
//    app.UseDeveloperExceptionPage();
//}


// Configure the HTTP request pipeline.
// 
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));           

app.UseAuthentication();
app.UseAuthorization();         

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
//app.UseHttpsRedirection();
//app.UseAuthorization();


app.MapControllers();
//(--> for Migration)
using var scope = app.Services.CreateScope();    //this give access to all of the services inside this program class
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(userManager,roleManager);      //context
}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "Un Error Occured During Migration");
}

app.Run();
    