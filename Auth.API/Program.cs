using System.Text;
using Auth.API.Data;
using Auth.API.Dtos;
using Auth.API.Mappers;
using Auth.API.Models;
using Auth.API.Repository;
using Auth.API.Services;
using Auth.API.Services.IServices;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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

//test the mappers before running the api
mapper.ConfigurationProvider.AssertConfigurationIsValid();


//add jwt authentication
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

// Response
builder.Services.AddScoped<ResponseDto>();

// Services
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();

//Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

////setup the controllers to be guarded by a token.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["ApiSettings:JwtOptions:Issuer"],
        ValidAudience = builder.Configuration["ApiSettings:JwtOptions:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["ApiSettings:JwtOptions:Secret"]))
    };
});

// add controllers
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//Setup authorization in the openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option => {
    option.AddSecurityDefinition(name: "Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the bearer scheme, Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[]{}
        }
    });
});
//Setup th fluent request validator
builder.Services.AddControllers()
    .AddFluentValidation(fv => {
        fv.RegisterValidatorsFromAssemblyContaining<LoginRequestDtoValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<RegistrationRequestDtoValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<RoleRequestValidator>();
  });

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

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

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