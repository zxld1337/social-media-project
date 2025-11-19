using MySql.Data.MySqlClient;
using System.Data;
using Scalar.AspNetCore;
using SocialMediaAPI.Repositories;
using Microsoft.AspNetCore.Identity;
using SocialMediaAPI.Services;
using System.Text.Encodings.Web;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container:
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register the BCrypt Hashing Service:
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

//Register the MySQL Dapper Repository:
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

//Register built-in HTML Encoder:
builder.Services.AddSingleton(HtmlEncoder.Default);

//Retrieve MySQL Connection String:
builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Welcome to the Social Media API!");

app.Run();
