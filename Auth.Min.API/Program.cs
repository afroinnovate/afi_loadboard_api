using System.Net.Mime;
using System.Security.Claims;
using Auth.Min.API.Data;
using Auth.Min.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Auth.Min.API.Endpoints;
using System.Text;
using Auth.Min.API;
using Auth.Min.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => 
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the bearer scheme, Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        Array.Empty<string>()
    }
    });
});


// setitngs for docker container
// var defaultConnection = Environment.GetEnvironmentVariable("DefaultConnection");

// // Add DB context injection for docker container
// builder.Services.AddDbContext<AppDbContext>(option => 
//     option.UseNpgsql(defaultConnection));

// Add DB context injection for dotnet run in appsettings.json
builder.Services.AddDbContext<AppDbContext>(option => 
    option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    
// Register the EmailSender service as transient to create a new instance each time it's needed
builder.Services.AddTransient<IEmailConfigService, EmailService>();

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
// Read JwtOptions from appsettings
var jwtConfig = builder.Configuration.GetSection("ApiSettings:JwtOptions");
builder.Services.Configure<JwtOptions>(jwtConfig);

var secretKey = jwtConfig["SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key is not configured");

// Add JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig["Issuer"],
            ValidAudience = jwtConfig["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

// Add authorization
builder.Services.AddAuthorization();

// Add Identity Endpoints
builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
                

// Read Roles from appsettings and register
var rolesConfig = new Roles();
builder.Configuration.GetSection("Roles").Bind(rolesConfig);
builder.Services.AddSingleton(rolesConfig);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

var roleConfig =  app.Services.GetRequiredService<Roles>();
var logger = app.Services.GetRequiredService<ILogger<Program>>();
// Map Identity API and Auth endpoints
// app.MapIdentityApi<IdentityUser>();
app.MapAuthEndpoints(roleConfig, logger);

// app.MapAuthEndpoints(roleConfig);

app.Run();
