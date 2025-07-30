using BMS.BLL.Services.DbServices;
using BMS.BLL.Services.Validation;
using BMS.DAL.DB;
using BMS.DAL.Repository;
using BMS.Models.Models;
using BMS_API.EventHandlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


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

//registering users identity and roles
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


//registering jwt token service
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false, // disables issuer 
        ValidateAudience = false, // disables audience
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});

builder.Services.AddAuthorization();

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


//seeding the database 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    RoleSeeder.SeedRolesAsync(services).Wait();
    UserSeeder.SeedUsersAsync(services).Wait();

}


app.UseHttpsRedirection();



app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();

app.Run();