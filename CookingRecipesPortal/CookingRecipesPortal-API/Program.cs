using CookingRecipesPortal_API;
using CookingRecipesPortal_Infrastructure.MappingConfigurations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext(builder.Configuration.GetConnectionString("DefaultConnectionString"));
builder.Services.AddAutoMapper(typeof(MappingConfig));

// register services
builder.Services.RegisterServices();

builder.Services.ConfigureGlobalFilters();

builder.Services.ConfigureJwtAuthentication(builder.Configuration.GetSection("JWTKey").Value);


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

app.MapControllers();

app.Run();
