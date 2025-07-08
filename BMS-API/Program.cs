using BMS.BLL.Services.Validation;
using BMS.DAL.DB;
using BMS.DAL.Repository;
using BMS.Models.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//DI registers
builder.Services.AddScoped<IValidation, Validation>();
builder.Services.AddScoped<IBookAccess<Book>, BookAccess>();

//AppDbContext register
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Swagger registers
builder.Services.AddEndpointsApiExplorer(); // Required
builder.Services.AddSwaggerGen();           // swagger generator


var app = builder.Build();

// Configure the HTTP request pipeline.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
