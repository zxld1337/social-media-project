using MySql.Data.MySqlClient;
using System.Data;
using Scalar.AspNetCore;
using SocialMediaAPI.Repositories;
using Microsoft.AspNetCore.Identity;
using SocialMediaAPI.Services;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

//Add Services to the Container:
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

//Reminder: Look into user authentication!!!
//Add Authentication Services:
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
    {
        options.Cookie.Name = "SocialAppAuth"; // Custom name for the authentication cookie
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // How long the cookie lasts
        options.SlidingExpiration = true; // Resets expiration on activity
        options.LoginPath = "/login"; // Optional: specify where to redirect unauthorized users
    });

//Register the BCrypt Hashing Service:
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

//Register the MySQL Dapper Repository:
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

//Register built-in HTML Encoder:
builder.Services.AddSingleton(HtmlEncoder.Default);

//Retrieve MySQL Connection String:
builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));



var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthentication(); //Used for Login
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Welcome to the Social Media API!");

app.Run();
