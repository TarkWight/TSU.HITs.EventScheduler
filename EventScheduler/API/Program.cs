using EventScheduler.API.Google;
using EventScheduler.Data;
using EventScheduler.Data.Repositories;
using EventScheduler.Domain.Entities;
using EventScheduler.Domain.Interfaces;
using EventScheduler.Domain.Services;
using EventScheduler.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

//#### Add services to the container.
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventService, EventService>();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IAPIRepository, APIRepository>();

//#

//#### JWT
var tokenConfigurations = new TokenConfigurations();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = TokenConfigurations.Issuer,
        ValidateAudience = true,
        ValidAudience = TokenConfigurations.Audience,
        ValidateLifetime = true,
        IssuerSigningKey = TokenConfigurations.GetSymmetricSecurityKey(),
        ValidateIssuerSigningKey = true,
    };
});
//#

//#### Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//#


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

//#### Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//#

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        if (!dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.EnsureCreated();
            Console.WriteLine("Automatically applied migration.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error occurred while migrating: {ex.Message}");
    }
    var accessToken = await DatabaseInitializer.Initialize(services);
    Console.WriteLine($"Access Token for Dean: {accessToken}");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
