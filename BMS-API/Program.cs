using BMS.BLL.Services.Validation;
using BMS.DAL.Repository;
using BMS.Models.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//DI registers
builder.Services.AddScoped<IValidation, Validation>();

builder.Services.AddScoped<IBookAccess<Book>, BookAccess>();

//Swagger registers


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
