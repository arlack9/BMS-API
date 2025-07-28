using BMS.BLL.Services.Validation;
using BMS.DAL.DB;
using BMS.DAL.Repository;
using BMS.Models.Models;
using Microsoft.EntityFrameworkCore;
using BMS.BLL.Services.DbServices;

using BMS_API.EventHandlers;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//DI registers
builder.Services.AddScoped<IValidation, Validation>();
builder.Services.AddScoped<IBookAccess<Book>, BookAccess>();
builder.Services.AddScoped<IDbServices<Book>, DbServices>();

builder.Services.AddScoped<LibraryEventHandlers>();

//AppDbContext register
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Sqlite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//Swagger registers
builder.Services.AddEndpointsApiExplorer(); // Required
builder.Services.AddSwaggerGen();           // swagger generator

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