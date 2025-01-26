using AuthenticationServiNamazSchedulerApp.API.Utils;
using Microsoft.EntityFrameworkCore;
using NamazSchedulerApp.API.Context;
using NamazSchedulerApp.API.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<GetHashPassword>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var secretKey = configuration["JwtSettings:SecretKey"];
    var issuer = configuration["JwtSettings:Issuer"];
    var audience = configuration["JwtSettings:Audience"];
    return new GenerateToken(secretKey, issuer, audience);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
