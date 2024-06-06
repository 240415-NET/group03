using Microsoft.EntityFrameworkCore;
using LibrarySystem.API.Models;
using LibrarySystem.API.Controllers;
using LibrarySystem.API.Services;
using LibrarySystem.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// This adds our UserService, that our UserController then asks for
builder.Services.AddScoped<IUserService, UserService>();
// This adds our UserStorageEFRepo (data-access layer), that our UserService asks for.
builder.Services.AddScoped<IUserDataAccess, UserDataAccess>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Connections
string connectionString = File.ReadAllText(@"connectionstring.txt");
builder.Services.AddDbContext<LibrarySystemContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
