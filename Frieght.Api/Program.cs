using Frieght.Api.Endpoints;
using Frieght.Api.Infrastructure;
using Frieght.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddScoped<ILoadRepository, LoadRepository>();
builder.Services.AddRepositories(builder.Configuration);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

await app.Services.InitializeDbAsync();
app.MapLoadsEndpoints();//Load endpoints
app.MapCarriersEndpoints();//Carrier endpoints

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();


