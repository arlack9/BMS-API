using BMS.BLL.Services;
using BMS.BLL.Services.Validation;
using BMS.DAL.DB;
using BMS.DAL.Repository;
using BMS.Models.Models;
using BMS_UI.ViewModels;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();




//DI registers
builder.Services.AddScoped<IValidation, Validation>();
builder.Services.AddScoped<IBookAccess<Book>, BookAccess>();
builder.Services.AddScoped<IDbServices<Book>, DbServices>();

//AppDbContext register
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Automapper register

//builder.Services.AddAutoMapper(typeof(Program).Assembly);





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/")
    .WithStaticAssets();


app.Run();
