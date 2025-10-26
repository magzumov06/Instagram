using System.Text;
using Domain.DTOs.EmailDto;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Data.Seeder;
using Infrastructure.ExtensionMethod;
using Infrastructure.FileStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
//Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug)
    .WriteTo.File(
        "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .Enrich.FromLogContext()
    .MinimumLevel.Debug() 
    .CreateLogger();
//DataContext
builder.Services.RegisterDataContext(builder.Configuration);

//File

builder.Services.AddScoped<IFileStorage>(
    sp => new FileStorage(builder.Environment.ContentRootPath));

//Email

builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSetting"));

//Services
builder.Services.RegisterServices();

builder.Services.AddHttpContextAccessor();
//Identity
builder.Services.RegisterIdentity();

//Swagger
builder.Services.RegisterSwagger();

// JWT Auth
builder.Services.RegisterJwt(builder.Configuration);

builder.Services.AddAuthorization(opt => { opt.AddPolicy("AdminOnly", p => p.RequireRole("Admin")); });


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
        var data =  services.GetRequiredService<DataContext>();
        await Seed.SeedAdmin(userManager, roleManager);
        await Seed.SeedRole(roleManager);
        await data.Database.MigrateAsync();
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
app.Run();