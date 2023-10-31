using Auth.API.Data;
using Auth.API.Dtos;
using Auth.API.Mappers;
using Auth.API.Models;
using Auth.API.Repository;
using Auth.API.Services;
using Auth.API.Services.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//register the sql server connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//add identity framework
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//add mapper configuration
IMapper mapper = MapperConfig.RegisterMapping().CreateMapper();
builder.Services.AddSingleton(mapper);
//use dependency injection to inject the mapping profile
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//add jwt authentication
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

// Response
builder.Services.AddScoped<ResponseDto>();

// Services
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();

//Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();


// add controllers
builder.Services.AddControllers();

// Configure  Swagger/OpenAPI
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Map controllers
app.MapControllers();

//apply migration if any exists
applyMigrations();

app.Run();

// create method that will automatically apply migrations if there's any pending one 
void applyMigrations()
{
    using (var serviceScope = app.Services.CreateScope())
    {
        var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
        if (dbContext?.Database.GetMigrations().Count() > 0)
        {
            dbContext.Database.Migrate();
        }
    };
}