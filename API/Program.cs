using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);       //Extensions
builder.Services.AddIdentityServices(builder.Configuration);


var app = builder.Build();                                      //some changes when not run...

// Configure the HTTP request pipeline.                                         
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

app.Run();
    